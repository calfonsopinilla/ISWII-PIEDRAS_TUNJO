toc.dat                                                                                             0000600 0004000 0002000 00000324376 13632062303 0014454 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        PGDMP       *    
        
        x            piedras    10.7    11.1 �    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                       false         �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                       false         �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                       false         �           1262    73808    piedras    DATABASE     �   CREATE DATABASE piedras WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'Spanish_Colombia.1252' LC_CTYPE = 'Spanish_Colombia.1252';
    DROP DATABASE piedras;
             postgres    false                     2615    73809    parametrizacion    SCHEMA        CREATE SCHEMA parametrizacion;
    DROP SCHEMA parametrizacion;
             postgres    false         
            2615    73810    parque    SCHEMA        CREATE SCHEMA parque;
    DROP SCHEMA parque;
             postgres    false         �           0    0    SCHEMA parque    COMMENT     �   COMMENT ON SCHEMA parque IS 'Esquema dedicado a contener todas las tablas,triggets, procedimientos almacenados, jobs correspondientes a todas las tablas dedicadas a lo directamente relacionado con el parque';
                  postgres    false    10                     2615    73811    security    SCHEMA        CREATE SCHEMA security;
    DROP SCHEMA security;
             postgres    false         �           0    0    SCHEMA security    COMMENT     W   COMMENT ON SCHEMA security IS 'Esquema dedicado a almacenar la seguridad del sistema';
                  postgres    false    6         �            1255    73812    f_log_auditoria()    FUNCTION     y  CREATE FUNCTION security.f_log_auditoria() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
	 DECLARE
		_pk TEXT :='';		-- Representa la llave primaria de la tabla que esta siedno modificada.
		_sql TEXT;		-- Variable para la creacion del procedured.
		_column_guia RECORD; 	-- Variable para el FOR guarda los nombre de las columnas.
		_column_key RECORD; 	-- Variable para el FOR guarda los PK de las columnas.
		_token TEXT;	-- Almacena el usuario que genera el cambio.
		_user_db TEXT;		-- Almacena el usuario de bd que genera la transaccion.
		_control INT;		-- Variabel de control par alas llaves primarias.
		_count_key INT = 0;	-- Cantidad de columnas pertenecientes al PK.
		_sql_insert TEXT;	-- Variable para la construcción del insert del json de forma dinamica.
		_sql_delete TEXT;	-- Variable para la construcción del delete del json de forma dinamica.
		_sql_update TEXT;	-- Variable para la construcción del update del json de forma dinamica.
		_new_data RECORD; 	-- Fila que representa los campos nuevos del registro.
		_old_data RECORD;	-- Fila que representa los campos viejos del registro.
	BEGIN
			-- Se genera la evaluacion para determianr el tipo de accion sobre la tabla
		 IF (TG_OP = 'INSERT') THEN
			_new_data := NEW;
			_old_data := NEW;
		ELSEIF (TG_OP = 'UPDATE') THEN
			_new_data := NEW;
			_old_data := OLD;
		ELSE
			_new_data := OLD;
			_old_data := OLD;
		END IF;

		-- Se genera la evaluacion para determianr el tipo de accion sobre la tabla
		IF ((SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = TG_TABLE_SCHEMA AND table_name = TG_TABLE_NAME AND column_name = 'id' ) > 0) THEN
			_pk := _new_data.id;
		ELSE
			_pk := '-1';
		END IF;

		-- Se valida que exista el campo modified_by
		IF ((SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = TG_TABLE_SCHEMA AND table_name = TG_TABLE_NAME AND column_name = 'token') > 0) THEN
			_token := _new_data.token;
		ELSE
			_token := '';
		END IF;

		-- Se guarda el susuario de bd que genera la transaccion
		_user_db := (SELECT CURRENT_USER);

		-- Se evalua que exista el procedimeinto adecuado
		IF (SELECT COUNT(*) FROM security.function_db_view acfdv WHERE acfdv.b_function = 'field_audit' AND acfdv.b_type_parameters = TG_TABLE_SCHEMA || '.'|| TG_TABLE_NAME || ', '|| TG_TABLE_SCHEMA || '.'|| TG_TABLE_NAME || ', character varying, character varying, character varying, text, character varying, text, text') > 0
			THEN
				-- Se realiza la invocación del procedured generado dinamivamente
				PERFORM security.field_audit(_new_data, _old_data, TG_OP, _token, _user_db , _pk, ''::text);
		ELSE
			-- Se empieza la construcción del Procedured generico
			_sql := 'CREATE OR REPLACE FUNCTION security.field_audit( _data_new '|| TG_TABLE_SCHEMA || '.'|| TG_TABLE_NAME || ', _data_old '|| TG_TABLE_SCHEMA || '.'|| TG_TABLE_NAME || ', _accion character varying, _token text, _user_db character varying, _table_pk text, _init text)'
			|| ' RETURNS TEXT AS ''
'
			|| '
'
	|| '	DECLARE
'
	|| '		_column_data TEXT;
	 	_datos jsonb;
	 	
'
	|| '	BEGIN
			_datos = ''''{}'''';
';
			-- Se evalua si hay que actualizar la pk del registro de auditoria.
			IF _pk = '-1'
				THEN
					_sql := _sql
					|| '
		_column_data := ';

					-- Se genera el update con la clave pk de la tabla
					SELECT
						COUNT(isk.column_name)
					INTO
						_control
					FROM
						information_schema.table_constraints istc JOIN information_schema.key_column_usage isk ON isk.constraint_name = istc.constraint_name
					WHERE
						istc.table_schema = TG_TABLE_SCHEMA
					 AND	istc.table_name = TG_TABLE_NAME
					 AND	istc.constraint_type ilike '%primary%';

					-- Se agregan las columnas que componen la pk de la tabla.
					FOR _column_key IN SELECT
							isk.column_name
						FROM
							information_schema.table_constraints istc JOIN information_schema.key_column_usage isk ON isk.constraint_name = istc.constraint_name
						WHERE
							istc.table_schema = TG_TABLE_SCHEMA
						 AND	istc.table_name = TG_TABLE_NAME
						 AND	istc.constraint_type ilike '%primary%'
						ORDER BY 
							isk.ordinal_position  LOOP

						_sql := _sql || ' _data_new.' || _column_key.column_name;
						
						_count_key := _count_key + 1 ;
						
						IF _count_key < _control THEN
							_sql :=	_sql || ' || ' || ''''',''''' || ' ||';
						END IF;
					END LOOP;
				_sql := _sql || ';';
			END IF;

			_sql_insert:='
		IF _accion = ''''INSERT''''
			THEN
				';
			_sql_delete:='
		ELSEIF _accion = ''''DELETE''''
			THEN
				';
			_sql_update:='
		ELSE
			';

			-- Se genera el ciclo de agregado de columnas para el nuevo procedured
			FOR _column_guia IN SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = TG_TABLE_SCHEMA AND table_name = TG_TABLE_NAME
				LOOP
						
					_sql_insert:= _sql_insert || '_datos := _datos || json_build_object('''''
					|| _column_guia.column_name
					|| '_nuevo'
					|| ''''', '
					|| '_data_new.'
					|| _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea', 'USER-DEFINED') THEN 
						_sql_insert:= _sql_insert
						||'::text';
					END IF;

					_sql_insert:= _sql_insert || ')::jsonb;
				';

					_sql_delete := _sql_delete || '_datos := _datos || json_build_object('''''
					|| _column_guia.column_name
					|| '_anterior'
					|| ''''', '
					|| '_data_old.'
					|| _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea', 'USER-DEFINED') THEN 
						_sql_delete:= _sql_delete
						||'::text';
					END IF;

					_sql_delete:= _sql_delete || ')::jsonb;
				';

					_sql_update := _sql_update || 'IF _data_old.' || _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea','USER-DEFINED') THEN 
						_sql_update:= _sql_update
						||'::text';
					END IF;

					_sql_update:= _sql_update || ' <> _data_new.' || _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea','USER-DEFINED') THEN 
						_sql_update:= _sql_update
						||'::text';
					END IF;

					_sql_update:= _sql_update || '
				THEN _datos := _datos || json_build_object('''''
					|| _column_guia.column_name
					|| '_anterior'
					|| ''''', '
					|| '_data_old.'
					|| _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea','USER-DEFINED') THEN 
						_sql_update:= _sql_update
						||'::text';
					END IF;

					_sql_update:= _sql_update
					|| ', '''''
					|| _column_guia.column_name
					|| '_nuevo'
					|| ''''', _data_new.'
					|| _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea', 'USER-DEFINED') THEN 
						_sql_update:= _sql_update
						||'::text';
					END IF;

					_sql_update:= _sql_update
					|| ')::jsonb;
			END IF;
			';
			END LOOP;

			-- Se le agrega la parte final del procedured generico
			
			_sql:= _sql || _sql_insert || _sql_delete || _sql_update
			|| ' 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			''''' || TG_TABLE_SCHEMA || ''''',
			''''' || TG_TABLE_NAME || ''''',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;'''
|| '
LANGUAGE plpgsql;';

			-- Se genera la ejecución de _sql, es decir se crea el nuevo procedured de forma generica.
			EXECUTE _sql;

		-- Se realiza la invocación del procedured generado dinamivamente
			PERFORM security.field_audit(_new_data, _old_data, TG_OP::character varying, _token, _user_db, _pk, ''::text);

		END IF;

		RETURN NULL;

END;
$$;
 *   DROP FUNCTION security.f_log_auditoria();
       security       postgres    false    6         �            1259    73814    cabana    TABLE     I  CREATE TABLE parque.cabana (
    id integer NOT NULL,
    nombre text NOT NULL,
    capacidad integer NOT NULL,
    precio double precision NOT NULL,
    imagenes_url text NOT NULL,
    calificacion double precision NOT NULL,
    comentarios_id text NOT NULL,
    token text,
    last_modification timestamp without time zone
);
    DROP TABLE parque.cabana;
       parque         postgres    false    10         �           0    0    TABLE cabana    COMMENT     �   COMMENT ON TABLE parque.cabana IS 'Tabla dedicada a almacenar los datos de las cabañas del parque arqueológico de facatativa.';
            parque       postgres    false    199         �            1255    73820 a   field_audit(parque.cabana, parque.cabana, character varying, text, character varying, text, text)    FUNCTION     '  CREATE FUNCTION security.field_audit(_data_new parque.cabana, _data_old parque.cabana, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				_datos := _datos || json_build_object('capacidad_nuevo', _data_new.capacidad)::jsonb;
				_datos := _datos || json_build_object('precio_nuevo', _data_new.precio)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('capacidad_anterior', _data_old.capacidad)::jsonb;
				_datos := _datos || json_build_object('precio_anterior', _data_old.precio)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			IF _data_old.capacidad <> _data_new.capacidad
				THEN _datos := _datos || json_build_object('capacidad_anterior', _data_old.capacidad, 'capacidad_nuevo', _data_new.capacidad)::jsonb;
			END IF;
			IF _data_old.precio <> _data_new.precio
				THEN _datos := _datos || json_build_object('precio_anterior', _data_old.precio, 'precio_nuevo', _data_new.precio)::jsonb;
			END IF;
			IF _data_old.imagenes_url <> _data_new.imagenes_url
				THEN _datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url, 'imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.comentarios_id <> _data_new.comentarios_id
				THEN _datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id, 'comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'cabana',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.cabana, _data_old parque.cabana, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    199    199    6         �            1259    73821    comentarios    TABLE     G  CREATE TABLE parque.comentarios (
    id bigint NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    descripcion text NOT NULL,
    calificacion double precision NOT NULL,
    reportado boolean NOT NULL,
    usuario_id integer NOT NULL,
    last_modification timestamp without time zone,
    token text
);
    DROP TABLE parque.comentarios;
       parque         postgres    false    10         �           0    0    TABLE comentarios    COMMENT     �   COMMENT ON TABLE parque.comentarios IS 'tabla dedicada a almacenar los datos de los comentarios del sistema,tanto como de las noticias,eventos y pictogramas del parque arqueologico';
            parque       postgres    false    200         �            1255    73827 k   field_audit(parque.comentarios, parque.comentarios, character varying, text, character varying, text, text)    FUNCTION     �  CREATE FUNCTION security.field_audit(_data_new parque.comentarios, _data_old parque.comentarios, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('descripcion_nuevo', _data_new.descripcion)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('reportado_nuevo', _data_new.reportado)::jsonb;
				_datos := _datos || json_build_object('usuario_id_nuevo', _data_new.usuario_id)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('reportado_anterior', _data_old.reportado)::jsonb;
				_datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.fecha_publicacion <> _data_new.fecha_publicacion
				THEN _datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion, 'fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
			END IF;
			IF _data_old.descripcion <> _data_new.descripcion
				THEN _datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion, 'descripcion_nuevo', _data_new.descripcion)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.reportado <> _data_new.reportado
				THEN _datos := _datos || json_build_object('reportado_anterior', _data_old.reportado, 'reportado_nuevo', _data_new.reportado)::jsonb;
			END IF;
			IF _data_old.usuario_id <> _data_new.usuario_id
				THEN _datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id, 'usuario_id_nuevo', _data_new.usuario_id)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'comentarios',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.comentarios, _data_old parque.comentarios, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    6    200    200         �            1259    73828 
   estado_pqr    TABLE     V   CREATE TABLE parque.estado_pqr (
    id integer NOT NULL,
    nombre text NOT NULL
);
    DROP TABLE parque.estado_pqr;
       parque         postgres    false    10         �            1255    73834 i   field_audit(parque.estado_pqr, parque.estado_pqr, character varying, text, character varying, text, text)    FUNCTION     P  CREATE FUNCTION security.field_audit(_data_new parque.estado_pqr, _data_old parque.estado_pqr, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'estado_pqr',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.estado_pqr, _data_old parque.estado_pqr, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    201    201    6         �            1259    73835    estado_reserva    TABLE     Z   CREATE TABLE parque.estado_reserva (
    id integer NOT NULL,
    nombre text NOT NULL
);
 "   DROP TABLE parque.estado_reserva;
       parque         postgres    false    10         �           0    0    TABLE estado_reserva    COMMENT     p   COMMENT ON TABLE parque.estado_reserva IS 'Tabla dedicada a almacenar los estados de los procesos del sistema';
            parque       postgres    false    202         �            1255    73841 q   field_audit(parque.estado_reserva, parque.estado_reserva, character varying, text, character varying, text, text)    FUNCTION     \  CREATE FUNCTION security.field_audit(_data_new parque.estado_reserva, _data_old parque.estado_reserva, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'estado_reserva',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.estado_reserva, _data_old parque.estado_reserva, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    202    6    202         �            1259    73842    evento    TABLE     |  CREATE TABLE parque.evento (
    id integer NOT NULL,
    nombre text NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    descripcion text NOT NULL,
    imagenes_url text NOT NULL,
    comentarios_id text NOT NULL,
    calificacion double precision,
    token text,
    last_modification timestamp without time zone,
    fecha timestamp without time zone
);
    DROP TABLE parque.evento;
       parque         postgres    false    10         �           0    0    TABLE evento    COMMENT        COMMENT ON TABLE parque.evento IS 'Tabla dedicada a almacenar los datos de los eventos del parque arqueologico de facatativa';
            parque       postgres    false    203         �            1255    73848 a   field_audit(parque.evento, parque.evento, character varying, text, character varying, text, text)    FUNCTION     �  CREATE FUNCTION security.field_audit(_data_new parque.evento, _data_old parque.evento, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('descripcion_nuevo', _data_new.descripcion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				_datos := _datos || json_build_object('fecha_nuevo', _data_new.fecha)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				_datos := _datos || json_build_object('fecha_anterior', _data_old.fecha)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			IF _data_old.fecha_publicacion <> _data_new.fecha_publicacion
				THEN _datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion, 'fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
			END IF;
			IF _data_old.descripcion <> _data_new.descripcion
				THEN _datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion, 'descripcion_nuevo', _data_new.descripcion)::jsonb;
			END IF;
			IF _data_old.imagenes_url <> _data_new.imagenes_url
				THEN _datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url, 'imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
			END IF;
			IF _data_old.comentarios_id <> _data_new.comentarios_id
				THEN _datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id, 'comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			IF _data_old.fecha <> _data_new.fecha
				THEN _datos := _datos || json_build_object('fecha_anterior', _data_old.fecha, 'fecha_nuevo', _data_new.fecha)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'evento',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.evento, _data_old parque.evento, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    6    203    203         �            1259    73849    informacion_parque    TABLE     �   CREATE TABLE parque.informacion_parque (
    id integer NOT NULL,
    propety text NOT NULL,
    descripcion text NOT NULL,
    imagenes_url text NOT NULL,
    token text,
    last_modification timestamp without time zone
);
 &   DROP TABLE parque.informacion_parque;
       parque         postgres    false    10         �           0    0    TABLE informacion_parque    COMMENT     �   COMMENT ON TABLE parque.informacion_parque IS 'Tabla dedicada a almacenar la informacion del parque, tal como descripcion,ubicacion, reseña historica';
            parque       postgres    false    204         �            1255    73855 y   field_audit(parque.informacion_parque, parque.informacion_parque, character varying, text, character varying, text, text)    FUNCTION     �  CREATE FUNCTION security.field_audit(_data_new parque.informacion_parque, _data_old parque.informacion_parque, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('propety_nuevo', _data_new.propety)::jsonb;
				_datos := _datos || json_build_object('descripcion_nuevo', _data_new.descripcion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('propety_anterior', _data_old.propety)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.propety <> _data_new.propety
				THEN _datos := _datos || json_build_object('propety_anterior', _data_old.propety, 'propety_nuevo', _data_new.propety)::jsonb;
			END IF;
			IF _data_old.descripcion <> _data_new.descripcion
				THEN _datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion, 'descripcion_nuevo', _data_new.descripcion)::jsonb;
			END IF;
			IF _data_old.imagenes_url <> _data_new.imagenes_url
				THEN _datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url, 'imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'informacion_parque',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.informacion_parque, _data_old parque.informacion_parque, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    204    6    204         �            1259    73856    noticia    TABLE     K  CREATE TABLE parque.noticia (
    id integer NOT NULL,
    titulo text NOT NULL,
    descripcion text NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    imagen_url text NOT NULL,
    comentarios_id text,
    calificacion double precision,
    token text,
    last_modification timestamp without time zone
);
    DROP TABLE parque.noticia;
       parque         postgres    false    10         �            1255    73862 c   field_audit(parque.noticia, parque.noticia, character varying, text, character varying, text, text)    FUNCTION     �  CREATE FUNCTION security.field_audit(_data_new parque.noticia, _data_old parque.noticia, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('titulo_nuevo', _data_new.titulo)::jsonb;
				_datos := _datos || json_build_object('descripcion_nuevo', _data_new.descripcion)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('imagen_url_nuevo', _data_new.imagen_url)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('titulo_anterior', _data_old.titulo)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('imagen_url_anterior', _data_old.imagen_url)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.titulo <> _data_new.titulo
				THEN _datos := _datos || json_build_object('titulo_anterior', _data_old.titulo, 'titulo_nuevo', _data_new.titulo)::jsonb;
			END IF;
			IF _data_old.descripcion <> _data_new.descripcion
				THEN _datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion, 'descripcion_nuevo', _data_new.descripcion)::jsonb;
			END IF;
			IF _data_old.fecha_publicacion <> _data_new.fecha_publicacion
				THEN _datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion, 'fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
			END IF;
			IF _data_old.imagen_url <> _data_new.imagen_url
				THEN _datos := _datos || json_build_object('imagen_url_anterior', _data_old.imagen_url, 'imagen_url_nuevo', _data_new.imagen_url)::jsonb;
			END IF;
			IF _data_old.comentarios_id <> _data_new.comentarios_id
				THEN _datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id, 'comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'noticia',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.noticia, _data_old parque.noticia, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    205    6    205         �            1259    73863 
   pictograma    TABLE       CREATE TABLE parque.pictograma (
    id integer NOT NULL,
    nombre text NOT NULL,
    calificacion double precision NOT NULL,
    imagenes_url text NOT NULL,
    descripcion text NOT NULL,
    comentarios_id text,
    token text,
    last_modification timestamp without time zone
);
    DROP TABLE parque.pictograma;
       parque         postgres    false    10         �           0    0    TABLE pictograma    COMMENT     z   COMMENT ON TABLE parque.pictograma IS 'Tabla dedicada a almacenar los datos de los pictogramas del parque arqueologico ';
            parque       postgres    false    206                     1255    73869 i   field_audit(parque.pictograma, parque.pictograma, character varying, text, character varying, text, text)    FUNCTION     �  CREATE FUNCTION security.field_audit(_data_new parque.pictograma, _data_old parque.pictograma, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('descripcion_nuevo', _data_new.descripcion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.imagenes_url <> _data_new.imagenes_url
				THEN _datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url, 'imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
			END IF;
			IF _data_old.descripcion <> _data_new.descripcion
				THEN _datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion, 'descripcion_nuevo', _data_new.descripcion)::jsonb;
			END IF;
			IF _data_old.comentarios_id <> _data_new.comentarios_id
				THEN _datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id, 'comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'pictograma',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.pictograma, _data_old parque.pictograma, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    6    206    206         �            1259    73870    pqr    TABLE     $  CREATE TABLE parque.pqr (
    id bigint NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    pregunta text NOT NULL,
    respuesta text,
    usuario_id integer NOT NULL,
    estado_id integer NOT NULL,
    token text,
    last_modification timestamp without time zone
);
    DROP TABLE parque.pqr;
       parque         postgres    false    10         �           0    0 	   TABLE pqr    COMMENT     X   COMMENT ON TABLE parque.pqr IS 'Tabla dedicada a almacenar las preguntas del sistema ';
            parque       postgres    false    207                    1255    73876 [   field_audit(parque.pqr, parque.pqr, character varying, text, character varying, text, text)    FUNCTION     �  CREATE FUNCTION security.field_audit(_data_new parque.pqr, _data_old parque.pqr, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('pregunta_nuevo', _data_new.pregunta)::jsonb;
				_datos := _datos || json_build_object('respuesta_nuevo', _data_new.respuesta)::jsonb;
				_datos := _datos || json_build_object('usuario_id_nuevo', _data_new.usuario_id)::jsonb;
				_datos := _datos || json_build_object('estado_id_nuevo', _data_new.estado_id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('pregunta_anterior', _data_old.pregunta)::jsonb;
				_datos := _datos || json_build_object('respuesta_anterior', _data_old.respuesta)::jsonb;
				_datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id)::jsonb;
				_datos := _datos || json_build_object('estado_id_anterior', _data_old.estado_id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.fecha_publicacion <> _data_new.fecha_publicacion
				THEN _datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion, 'fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
			END IF;
			IF _data_old.pregunta <> _data_new.pregunta
				THEN _datos := _datos || json_build_object('pregunta_anterior', _data_old.pregunta, 'pregunta_nuevo', _data_new.pregunta)::jsonb;
			END IF;
			IF _data_old.respuesta <> _data_new.respuesta
				THEN _datos := _datos || json_build_object('respuesta_anterior', _data_old.respuesta, 'respuesta_nuevo', _data_new.respuesta)::jsonb;
			END IF;
			IF _data_old.usuario_id <> _data_new.usuario_id
				THEN _datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id, 'usuario_id_nuevo', _data_new.usuario_id)::jsonb;
			END IF;
			IF _data_old.estado_id <> _data_new.estado_id
				THEN _datos := _datos || json_build_object('estado_id_anterior', _data_old.estado_id, 'estado_id_nuevo', _data_new.estado_id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'pqr',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.pqr, _data_old parque.pqr, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    207    207    6         �            1259    73877    reserva    TABLE       CREATE TABLE parque.reserva (
    id bigint NOT NULL,
    fecha_compra timestamp without time zone NOT NULL,
    precio double precision NOT NULL,
    usuario_id integer NOT NULL,
    estado_id integer NOT NULL,
    token text,
    last_modification timestamp without time zone
);
    DROP TABLE parque.reserva;
       parque         postgres    false    10         �           0    0    TABLE reserva    COMMENT     �   COMMENT ON TABLE parque.reserva IS 'Tabla dedicada a almacenar los datos de las reservas del sistema, esta sera heredada por otras tablas para independizar la información correspondiente ';
            parque       postgres    false    208                    1255    73883 c   field_audit(parque.reserva, parque.reserva, character varying, text, character varying, text, text)    FUNCTION       CREATE FUNCTION security.field_audit(_data_new parque.reserva, _data_old parque.reserva, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('fecha_compra_nuevo', _data_new.fecha_compra)::jsonb;
				_datos := _datos || json_build_object('precio_nuevo', _data_new.precio)::jsonb;
				_datos := _datos || json_build_object('usuario_id_nuevo', _data_new.usuario_id)::jsonb;
				_datos := _datos || json_build_object('estado_id_nuevo', _data_new.estado_id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('fecha_compra_anterior', _data_old.fecha_compra)::jsonb;
				_datos := _datos || json_build_object('precio_anterior', _data_old.precio)::jsonb;
				_datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id)::jsonb;
				_datos := _datos || json_build_object('estado_id_anterior', _data_old.estado_id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.fecha_compra <> _data_new.fecha_compra
				THEN _datos := _datos || json_build_object('fecha_compra_anterior', _data_old.fecha_compra, 'fecha_compra_nuevo', _data_new.fecha_compra)::jsonb;
			END IF;
			IF _data_old.precio <> _data_new.precio
				THEN _datos := _datos || json_build_object('precio_anterior', _data_old.precio, 'precio_nuevo', _data_new.precio)::jsonb;
			END IF;
			IF _data_old.usuario_id <> _data_new.usuario_id
				THEN _datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id, 'usuario_id_nuevo', _data_new.usuario_id)::jsonb;
			END IF;
			IF _data_old.estado_id <> _data_new.estado_id
				THEN _datos := _datos || json_build_object('estado_id_anterior', _data_old.estado_id, 'estado_id_nuevo', _data_new.estado_id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'reserva',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.reserva, _data_old parque.reserva, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    6    208    208         �            1259    73884    rol    TABLE     O   CREATE TABLE parque.rol (
    id integer NOT NULL,
    nombre text NOT NULL
);
    DROP TABLE parque.rol;
       parque         postgres    false    10         �           0    0 	   TABLE rol    COMMENT     d   COMMENT ON TABLE parque.rol IS 'Tabla dedicada a almacenar los roles correspondientes al sistema ';
            parque       postgres    false    209                    1255    73890 [   field_audit(parque.rol, parque.rol, character varying, text, character varying, text, text)    FUNCTION     ;  CREATE FUNCTION security.field_audit(_data_new parque.rol, _data_old parque.rol, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'rol',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.rol, _data_old parque.rol, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    209    209    6         �            1259    73891    ticket    TABLE     �   CREATE TABLE parque.ticket (
    id integer NOT NULL,
    nombre text NOT NULL,
    precio double precision NOT NULL,
    token text,
    last_modification timestamp without time zone
);
    DROP TABLE parque.ticket;
       parque         postgres    false    10                     0    0    TABLE ticket    COMMENT     �   COMMENT ON TABLE parque.ticket IS 'Tabla destinada a almacenar los datos de los tiquetes de los usuarios del sistema,profe si llega a ver esto pasenos';
            parque       postgres    false    210                    1255    73897 a   field_audit(parque.ticket, parque.ticket, character varying, text, character varying, text, text)    FUNCTION     �	  CREATE FUNCTION security.field_audit(_data_new parque.ticket, _data_old parque.ticket, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				_datos := _datos || json_build_object('precio_nuevo', _data_new.precio)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('precio_anterior', _data_old.precio)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			IF _data_old.precio <> _data_new.precio
				THEN _datos := _datos || json_build_object('precio_anterior', _data_old.precio, 'precio_nuevo', _data_new.precio)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'ticket',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.ticket, _data_old parque.ticket, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    210    210    6         �            1259    73898    usuario    TABLE     �  CREATE TABLE parque.usuario (
    id integer NOT NULL,
    nombre text NOT NULL,
    apellido text NOT NULL,
    tipo_documento text NOT NULL,
    numero_documento double precision NOT NULL,
    lugar_expedicion text,
    correo_electronico text NOT NULL,
    clave text NOT NULL,
    icono_url text NOT NULL,
    verificacion_cuenta boolean NOT NULL,
    estado_cuenta boolean NOT NULL,
    rol_id integer NOT NULL,
    token text,
    last_modification timestamp without time zone
);
    DROP TABLE parque.usuario;
       parque         postgres    false    10                    0    0    TABLE usuario    COMMENT     �   COMMENT ON TABLE parque.usuario IS 'Tabla dedicada a almacenar los datos correspondientes a los usuarios que se registren en el sistema';
            parque       postgres    false    211                    1255    73904 c   field_audit(parque.usuario, parque.usuario, character varying, text, character varying, text, text)    FUNCTION     �  CREATE FUNCTION security.field_audit(_data_new parque.usuario, _data_old parque.usuario, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				_datos := _datos || json_build_object('apellido_nuevo', _data_new.apellido)::jsonb;
				_datos := _datos || json_build_object('tipo_documento_nuevo', _data_new.tipo_documento)::jsonb;
				_datos := _datos || json_build_object('numero_documento_nuevo', _data_new.numero_documento)::jsonb;
				_datos := _datos || json_build_object('lugar_expedicion_nuevo', _data_new.lugar_expedicion)::jsonb;
				_datos := _datos || json_build_object('correo_electronico_nuevo', _data_new.correo_electronico)::jsonb;
				_datos := _datos || json_build_object('clave_nuevo', _data_new.clave)::jsonb;
				_datos := _datos || json_build_object('icono_url_nuevo', _data_new.icono_url)::jsonb;
				_datos := _datos || json_build_object('verificacion_cuenta_nuevo', _data_new.verificacion_cuenta)::jsonb;
				_datos := _datos || json_build_object('estado_cuenta_nuevo', _data_new.estado_cuenta)::jsonb;
				_datos := _datos || json_build_object('rol_id_nuevo', _data_new.rol_id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('apellido_anterior', _data_old.apellido)::jsonb;
				_datos := _datos || json_build_object('tipo_documento_anterior', _data_old.tipo_documento)::jsonb;
				_datos := _datos || json_build_object('numero_documento_anterior', _data_old.numero_documento)::jsonb;
				_datos := _datos || json_build_object('lugar_expedicion_anterior', _data_old.lugar_expedicion)::jsonb;
				_datos := _datos || json_build_object('correo_electronico_anterior', _data_old.correo_electronico)::jsonb;
				_datos := _datos || json_build_object('clave_anterior', _data_old.clave)::jsonb;
				_datos := _datos || json_build_object('icono_url_anterior', _data_old.icono_url)::jsonb;
				_datos := _datos || json_build_object('verificacion_cuenta_anterior', _data_old.verificacion_cuenta)::jsonb;
				_datos := _datos || json_build_object('estado_cuenta_anterior', _data_old.estado_cuenta)::jsonb;
				_datos := _datos || json_build_object('rol_id_anterior', _data_old.rol_id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			IF _data_old.apellido <> _data_new.apellido
				THEN _datos := _datos || json_build_object('apellido_anterior', _data_old.apellido, 'apellido_nuevo', _data_new.apellido)::jsonb;
			END IF;
			IF _data_old.tipo_documento <> _data_new.tipo_documento
				THEN _datos := _datos || json_build_object('tipo_documento_anterior', _data_old.tipo_documento, 'tipo_documento_nuevo', _data_new.tipo_documento)::jsonb;
			END IF;
			IF _data_old.numero_documento <> _data_new.numero_documento
				THEN _datos := _datos || json_build_object('numero_documento_anterior', _data_old.numero_documento, 'numero_documento_nuevo', _data_new.numero_documento)::jsonb;
			END IF;
			IF _data_old.lugar_expedicion <> _data_new.lugar_expedicion
				THEN _datos := _datos || json_build_object('lugar_expedicion_anterior', _data_old.lugar_expedicion, 'lugar_expedicion_nuevo', _data_new.lugar_expedicion)::jsonb;
			END IF;
			IF _data_old.correo_electronico <> _data_new.correo_electronico
				THEN _datos := _datos || json_build_object('correo_electronico_anterior', _data_old.correo_electronico, 'correo_electronico_nuevo', _data_new.correo_electronico)::jsonb;
			END IF;
			IF _data_old.clave <> _data_new.clave
				THEN _datos := _datos || json_build_object('clave_anterior', _data_old.clave, 'clave_nuevo', _data_new.clave)::jsonb;
			END IF;
			IF _data_old.icono_url <> _data_new.icono_url
				THEN _datos := _datos || json_build_object('icono_url_anterior', _data_old.icono_url, 'icono_url_nuevo', _data_new.icono_url)::jsonb;
			END IF;
			IF _data_old.verificacion_cuenta <> _data_new.verificacion_cuenta
				THEN _datos := _datos || json_build_object('verificacion_cuenta_anterior', _data_old.verificacion_cuenta, 'verificacion_cuenta_nuevo', _data_new.verificacion_cuenta)::jsonb;
			END IF;
			IF _data_old.estado_cuenta <> _data_new.estado_cuenta
				THEN _datos := _datos || json_build_object('estado_cuenta_anterior', _data_old.estado_cuenta, 'estado_cuenta_nuevo', _data_new.estado_cuenta)::jsonb;
			END IF;
			IF _data_old.rol_id <> _data_new.rol_id
				THEN _datos := _datos || json_build_object('rol_id_anterior', _data_old.rol_id, 'rol_id_nuevo', _data_new.rol_id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'usuario',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new parque.usuario, _data_old parque.usuario, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    6    211    211         �            1259    73905    tokens    TABLE       CREATE TABLE security.tokens (
    id integer NOT NULL,
    token text NOT NULL,
    fecha_generacion timestamp without time zone NOT NULL,
    fecha_vencimiento timestamp without time zone NOT NULL,
    aplicacion_id integer NOT NULL,
    user_id integer NOT NULL
);
    DROP TABLE security.tokens;
       security         postgres    false    6                    1255    73911 e   field_audit(security.tokens, security.tokens, character varying, text, character varying, text, text)    FUNCTION     �  CREATE FUNCTION security.field_audit(_data_new security.tokens, _data_old security.tokens, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('fecha_generacion_nuevo', _data_new.fecha_generacion)::jsonb;
				_datos := _datos || json_build_object('fecha_vencimiento_nuevo', _data_new.fecha_vencimiento)::jsonb;
				_datos := _datos || json_build_object('aplicacion_id_nuevo', _data_new.aplicacion_id)::jsonb;
				_datos := _datos || json_build_object('user_id_nuevo', _data_new.user_id)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('fecha_generacion_anterior', _data_old.fecha_generacion)::jsonb;
				_datos := _datos || json_build_object('fecha_vencimiento_anterior', _data_old.fecha_vencimiento)::jsonb;
				_datos := _datos || json_build_object('aplicacion_id_anterior', _data_old.aplicacion_id)::jsonb;
				_datos := _datos || json_build_object('user_id_anterior', _data_old.user_id)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.fecha_generacion <> _data_new.fecha_generacion
				THEN _datos := _datos || json_build_object('fecha_generacion_anterior', _data_old.fecha_generacion, 'fecha_generacion_nuevo', _data_new.fecha_generacion)::jsonb;
			END IF;
			IF _data_old.fecha_vencimiento <> _data_new.fecha_vencimiento
				THEN _datos := _datos || json_build_object('fecha_vencimiento_anterior', _data_old.fecha_vencimiento, 'fecha_vencimiento_nuevo', _data_new.fecha_vencimiento)::jsonb;
			END IF;
			IF _data_old.aplicacion_id <> _data_new.aplicacion_id
				THEN _datos := _datos || json_build_object('aplicacion_id_anterior', _data_old.aplicacion_id, 'aplicacion_id_nuevo', _data_new.aplicacion_id)::jsonb;
			END IF;
			IF _data_old.user_id <> _data_new.user_id
				THEN _datos := _datos || json_build_object('user_id_anterior', _data_old.user_id, 'user_id_nuevo', _data_new.user_id)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'security',
			'tokens',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;
 �   DROP FUNCTION security.field_audit(_data_new security.tokens, _data_old security.tokens, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text);
       security       postgres    false    212    212    6         �            1259    73912 
   aplicacion    TABLE     V   CREATE TABLE parametrizacion.aplicacion (
    id integer NOT NULL,
    nombre text
);
 '   DROP TABLE parametrizacion.aplicacion;
       parametrizacion         postgres    false    5         �            1259    73918    cabana_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.cabana_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE parque.cabana_id_seq;
       parque       postgres    false    199    10                    0    0    cabana_id_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE parque.cabana_id_seq OWNED BY parque.cabana.id;
            parque       postgres    false    214         �            1259    73920    comentario_cabana    TABLE     h   CREATE TABLE parque.comentario_cabana (
    cabana_id integer NOT NULL
)
INHERITS (parque.comentarios);
 %   DROP TABLE parque.comentario_cabana;
       parque         postgres    false    200    10         �            1259    73926    comentario_evento    TABLE     h   CREATE TABLE parque.comentario_evento (
    evento_id integer NOT NULL
)
INHERITS (parque.comentarios);
 %   DROP TABLE parque.comentario_evento;
       parque         postgres    false    10    200         �            1259    73932    comentario_noticia    TABLE     j   CREATE TABLE parque.comentario_noticia (
    noticia_id integer NOT NULL
)
INHERITS (parque.comentarios);
 &   DROP TABLE parque.comentario_noticia;
       parque         postgres    false    10    200         �            1259    73938    comentario_pictograma    TABLE     p   CREATE TABLE parque.comentario_pictograma (
    pictograma_id integer NOT NULL
)
INHERITS (parque.comentarios);
 )   DROP TABLE parque.comentario_pictograma;
       parque         postgres    false    10    200         �            1259    73944    comentarios_id_seq    SEQUENCE     {   CREATE SEQUENCE parque.comentarios_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE parque.comentarios_id_seq;
       parque       postgres    false    200    10                    0    0    comentarios_id_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE parque.comentarios_id_seq OWNED BY parque.comentarios.id;
            parque       postgres    false    219         �            1259    73946    estado_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.estado_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE parque.estado_id_seq;
       parque       postgres    false    10    202                    0    0    estado_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE parque.estado_id_seq OWNED BY parque.estado_reserva.id;
            parque       postgres    false    220         �            1259    73948    evento_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.evento_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE parque.evento_id_seq;
       parque       postgres    false    10    203                    0    0    evento_id_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE parque.evento_id_seq OWNED BY parque.evento.id;
            parque       postgres    false    221         �            1259    73950    informacion_parque_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.informacion_parque_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 0   DROP SEQUENCE parque.informacion_parque_id_seq;
       parque       postgres    false    10    204                    0    0    informacion_parque_id_seq    SEQUENCE OWNED BY     W   ALTER SEQUENCE parque.informacion_parque_id_seq OWNED BY parque.informacion_parque.id;
            parque       postgres    false    222         �            1259    73952    noticia_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.noticia_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE parque.noticia_id_seq;
       parque       postgres    false    205    10                    0    0    noticia_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE parque.noticia_id_seq OWNED BY parque.noticia.id;
            parque       postgres    false    223         �            1259    73954    pictograma_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.pictograma_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE parque.pictograma_id_seq;
       parque       postgres    false    10    206                    0    0    pictograma_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE parque.pictograma_id_seq OWNED BY parque.pictograma.id;
            parque       postgres    false    224         �            1259    73956 
   pqr_id_seq    SEQUENCE     s   CREATE SEQUENCE parque.pqr_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 !   DROP SEQUENCE parque.pqr_id_seq;
       parque       postgres    false    207    10         	           0    0 
   pqr_id_seq    SEQUENCE OWNED BY     9   ALTER SEQUENCE parque.pqr_id_seq OWNED BY parque.pqr.id;
            parque       postgres    false    225         �            1259    73958    reserva_cabana    TABLE     a   CREATE TABLE parque.reserva_cabana (
    ticket_id integer NOT NULL
)
INHERITS (parque.reserva);
 "   DROP TABLE parque.reserva_cabana;
       parque         postgres    false    10    208         �            1259    73964    reserva_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.reserva_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE parque.reserva_id_seq;
       parque       postgres    false    10    208         
           0    0    reserva_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE parque.reserva_id_seq OWNED BY parque.reserva.id;
            parque       postgres    false    227         �            1259    73966    reserva_ticket    TABLE     �   CREATE TABLE parque.reserva_ticket (
    ticket_id bigint NOT NULL,
    qr text NOT NULL,
    cantidad double precision NOT NULL
)
INHERITS (parque.reserva);
 "   DROP TABLE parque.reserva_ticket;
       parque         postgres    false    208    10                    0    0    TABLE reserva_ticket    COMMENT     �   COMMENT ON TABLE parque.reserva_ticket IS 'Tabla dedicada a almacenar los datos de los tiquetes de los usuarios heredada de la tabla reserva ';
            parque       postgres    false    228         �            1259    73972 
   rol_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.rol_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 !   DROP SEQUENCE parque.rol_id_seq;
       parque       postgres    false    10    209                    0    0 
   rol_id_seq    SEQUENCE OWNED BY     9   ALTER SEQUENCE parque.rol_id_seq OWNED BY parque.rol.id;
            parque       postgres    false    229         �            1259    73974    ticket_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.ticket_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE parque.ticket_id_seq;
       parque       postgres    false    10    210                    0    0    ticket_id_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE parque.ticket_id_seq OWNED BY parque.ticket.id;
            parque       postgres    false    230         �            1259    73976    usuario_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.usuario_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE parque.usuario_id_seq;
       parque       postgres    false    10    211                    0    0    usuario_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE parque.usuario_id_seq OWNED BY parque.usuario.id;
            parque       postgres    false    231         �            1259    73978 	   auditoria    TABLE     R  CREATE TABLE security.auditoria (
    id bigint NOT NULL,
    fecha timestamp without time zone NOT NULL,
    accion character varying(100),
    schema character varying(200) NOT NULL,
    tabla character varying(200),
    token text NOT NULL,
    user_bd character varying(100) NOT NULL,
    data jsonb NOT NULL,
    pk text NOT NULL
);
    DROP TABLE security.auditoria;
       security         postgres    false    6         �            1259    73984    auditoria_id_seq    SEQUENCE     {   CREATE SEQUENCE security.auditoria_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE security.auditoria_id_seq;
       security       postgres    false    6    232                    0    0    auditoria_id_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE security.auditoria_id_seq OWNED BY security.auditoria.id;
            security       postgres    false    233         �            1259    73986    function_db_view    VIEW     �  CREATE VIEW security.function_db_view AS
 SELECT pp.proname AS b_function,
    oidvectortypes(pp.proargtypes) AS b_type_parameters
   FROM (pg_proc pp
     JOIN pg_namespace pn ON ((pn.oid = pp.pronamespace)))
  WHERE ((pn.nspname)::text <> ALL (ARRAY[('pg_catalog'::character varying)::text, ('information_schema'::character varying)::text, ('admin_control'::character varying)::text, ('vial'::character varying)::text]));
 %   DROP VIEW security.function_db_view;
       security       postgres    false    6         �            1259    73991    token_id_seq    SEQUENCE     �   CREATE SEQUENCE security.token_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE security.token_id_seq;
       security       postgres    false    212    6                    0    0    token_id_seq    SEQUENCE OWNED BY     B   ALTER SEQUENCE security.token_id_seq OWNED BY security.tokens.id;
            security       postgres    false    235                    2604    73993 	   cabana id    DEFAULT     f   ALTER TABLE ONLY parque.cabana ALTER COLUMN id SET DEFAULT nextval('parque.cabana_id_seq'::regclass);
 8   ALTER TABLE parque.cabana ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    214    199                    2604    73994    comentario_cabana id    DEFAULT     v   ALTER TABLE ONLY parque.comentario_cabana ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);
 C   ALTER TABLE parque.comentario_cabana ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    215    219                    2604    73995    comentario_evento id    DEFAULT     v   ALTER TABLE ONLY parque.comentario_evento ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);
 C   ALTER TABLE parque.comentario_evento ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    219    216                    2604    73996    comentario_noticia id    DEFAULT     w   ALTER TABLE ONLY parque.comentario_noticia ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);
 D   ALTER TABLE parque.comentario_noticia ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    217    219                    2604    73997    comentario_pictograma id    DEFAULT     z   ALTER TABLE ONLY parque.comentario_pictograma ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);
 G   ALTER TABLE parque.comentario_pictograma ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    219    218         	           2604    73998    comentarios id    DEFAULT     p   ALTER TABLE ONLY parque.comentarios ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);
 =   ALTER TABLE parque.comentarios ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    219    200         
           2604    73999    estado_reserva id    DEFAULT     n   ALTER TABLE ONLY parque.estado_reserva ALTER COLUMN id SET DEFAULT nextval('parque.estado_id_seq'::regclass);
 @   ALTER TABLE parque.estado_reserva ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    220    202                    2604    74000 	   evento id    DEFAULT     f   ALTER TABLE ONLY parque.evento ALTER COLUMN id SET DEFAULT nextval('parque.evento_id_seq'::regclass);
 8   ALTER TABLE parque.evento ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    221    203                    2604    74001    informacion_parque id    DEFAULT     ~   ALTER TABLE ONLY parque.informacion_parque ALTER COLUMN id SET DEFAULT nextval('parque.informacion_parque_id_seq'::regclass);
 D   ALTER TABLE parque.informacion_parque ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    222    204                    2604    74002 
   noticia id    DEFAULT     h   ALTER TABLE ONLY parque.noticia ALTER COLUMN id SET DEFAULT nextval('parque.noticia_id_seq'::regclass);
 9   ALTER TABLE parque.noticia ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    223    205                    2604    74003    pictograma id    DEFAULT     n   ALTER TABLE ONLY parque.pictograma ALTER COLUMN id SET DEFAULT nextval('parque.pictograma_id_seq'::regclass);
 <   ALTER TABLE parque.pictograma ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    224    206                    2604    74004    pqr id    DEFAULT     `   ALTER TABLE ONLY parque.pqr ALTER COLUMN id SET DEFAULT nextval('parque.pqr_id_seq'::regclass);
 5   ALTER TABLE parque.pqr ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    225    207                    2604    74005 
   reserva id    DEFAULT     h   ALTER TABLE ONLY parque.reserva ALTER COLUMN id SET DEFAULT nextval('parque.reserva_id_seq'::regclass);
 9   ALTER TABLE parque.reserva ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    227    208                    2604    74006    reserva_cabana id    DEFAULT     o   ALTER TABLE ONLY parque.reserva_cabana ALTER COLUMN id SET DEFAULT nextval('parque.reserva_id_seq'::regclass);
 @   ALTER TABLE parque.reserva_cabana ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    227    226                    2604    74007    reserva_ticket id    DEFAULT     o   ALTER TABLE ONLY parque.reserva_ticket ALTER COLUMN id SET DEFAULT nextval('parque.reserva_id_seq'::regclass);
 @   ALTER TABLE parque.reserva_ticket ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    227    228                    2604    74008    rol id    DEFAULT     `   ALTER TABLE ONLY parque.rol ALTER COLUMN id SET DEFAULT nextval('parque.rol_id_seq'::regclass);
 5   ALTER TABLE parque.rol ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    229    209                    2604    74009 	   ticket id    DEFAULT     f   ALTER TABLE ONLY parque.ticket ALTER COLUMN id SET DEFAULT nextval('parque.ticket_id_seq'::regclass);
 8   ALTER TABLE parque.ticket ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    230    210                    2604    74010 
   usuario id    DEFAULT     h   ALTER TABLE ONLY parque.usuario ALTER COLUMN id SET DEFAULT nextval('parque.usuario_id_seq'::regclass);
 9   ALTER TABLE parque.usuario ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    231    211                    2604    74011    auditoria id    DEFAULT     p   ALTER TABLE ONLY security.auditoria ALTER COLUMN id SET DEFAULT nextval('security.auditoria_id_seq'::regclass);
 =   ALTER TABLE security.auditoria ALTER COLUMN id DROP DEFAULT;
       security       postgres    false    233    232                    2604    74012 	   tokens id    DEFAULT     i   ALTER TABLE ONLY security.tokens ALTER COLUMN id SET DEFAULT nextval('security.token_id_seq'::regclass);
 :   ALTER TABLE security.tokens ALTER COLUMN id DROP DEFAULT;
       security       postgres    false    235    212         �          0    73912 
   aplicacion 
   TABLE DATA               9   COPY parametrizacion.aplicacion (id, nombre) FROM stdin;
    parametrizacion       postgres    false    213       3033.dat �          0    73814    cabana 
   TABLE DATA               �   COPY parque.cabana (id, nombre, capacidad, precio, imagenes_url, calificacion, comentarios_id, token, last_modification) FROM stdin;
    parque       postgres    false    199       3019.dat �          0    73920    comentario_cabana 
   TABLE DATA               �   COPY parque.comentario_cabana (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, cabana_id) FROM stdin;
    parque       postgres    false    215       3035.dat �          0    73926    comentario_evento 
   TABLE DATA               �   COPY parque.comentario_evento (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, evento_id) FROM stdin;
    parque       postgres    false    216       3036.dat �          0    73932    comentario_noticia 
   TABLE DATA               �   COPY parque.comentario_noticia (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, noticia_id) FROM stdin;
    parque       postgres    false    217       3037.dat �          0    73938    comentario_pictograma 
   TABLE DATA               �   COPY parque.comentario_pictograma (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, pictograma_id) FROM stdin;
    parque       postgres    false    218       3038.dat �          0    73821    comentarios 
   TABLE DATA               �   COPY parque.comentarios (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token) FROM stdin;
    parque       postgres    false    200       3020.dat �          0    73828 
   estado_pqr 
   TABLE DATA               0   COPY parque.estado_pqr (id, nombre) FROM stdin;
    parque       postgres    false    201       3021.dat �          0    73835    estado_reserva 
   TABLE DATA               4   COPY parque.estado_reserva (id, nombre) FROM stdin;
    parque       postgres    false    202       3022.dat �          0    73842    evento 
   TABLE DATA               �   COPY parque.evento (id, nombre, fecha_publicacion, descripcion, imagenes_url, comentarios_id, calificacion, token, last_modification, fecha) FROM stdin;
    parque       postgres    false    203       3023.dat �          0    73849    informacion_parque 
   TABLE DATA               n   COPY parque.informacion_parque (id, propety, descripcion, imagenes_url, token, last_modification) FROM stdin;
    parque       postgres    false    204       3024.dat �          0    73856    noticia 
   TABLE DATA               �   COPY parque.noticia (id, titulo, descripcion, fecha_publicacion, imagen_url, comentarios_id, calificacion, token, last_modification) FROM stdin;
    parque       postgres    false    205       3025.dat �          0    73863 
   pictograma 
   TABLE DATA               �   COPY parque.pictograma (id, nombre, calificacion, imagenes_url, descripcion, comentarios_id, token, last_modification) FROM stdin;
    parque       postgres    false    206       3026.dat �          0    73870    pqr 
   TABLE DATA               z   COPY parque.pqr (id, fecha_publicacion, pregunta, respuesta, usuario_id, estado_id, token, last_modification) FROM stdin;
    parque       postgres    false    207       3027.dat �          0    73877    reserva 
   TABLE DATA               l   COPY parque.reserva (id, fecha_compra, precio, usuario_id, estado_id, token, last_modification) FROM stdin;
    parque       postgres    false    208       3028.dat �          0    73958    reserva_cabana 
   TABLE DATA               ~   COPY parque.reserva_cabana (id, fecha_compra, precio, usuario_id, estado_id, token, last_modification, ticket_id) FROM stdin;
    parque       postgres    false    226       3046.dat �          0    73966    reserva_ticket 
   TABLE DATA               �   COPY parque.reserva_ticket (id, fecha_compra, precio, usuario_id, estado_id, token, last_modification, ticket_id, qr, cantidad) FROM stdin;
    parque       postgres    false    228       3048.dat �          0    73884    rol 
   TABLE DATA               )   COPY parque.rol (id, nombre) FROM stdin;
    parque       postgres    false    209       3029.dat �          0    73891    ticket 
   TABLE DATA               N   COPY parque.ticket (id, nombre, precio, token, last_modification) FROM stdin;
    parque       postgres    false    210       3030.dat �          0    73898    usuario 
   TABLE DATA               �   COPY parque.usuario (id, nombre, apellido, tipo_documento, numero_documento, lugar_expedicion, correo_electronico, clave, icono_url, verificacion_cuenta, estado_cuenta, rol_id, token, last_modification) FROM stdin;
    parque       postgres    false    211       3031.dat �          0    73978 	   auditoria 
   TABLE DATA               a   COPY security.auditoria (id, fecha, accion, schema, tabla, token, user_bd, data, pk) FROM stdin;
    security       postgres    false    232       3052.dat �          0    73905    tokens 
   TABLE DATA               j   COPY security.tokens (id, token, fecha_generacion, fecha_vencimiento, aplicacion_id, user_id) FROM stdin;
    security       postgres    false    212       3032.dat            0    0    cabana_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('parque.cabana_id_seq', 26, true);
            parque       postgres    false    214                    0    0    comentarios_id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('parque.comentarios_id_seq', 6, true);
            parque       postgres    false    219                    0    0    estado_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('parque.estado_id_seq', 1, true);
            parque       postgres    false    220                    0    0    evento_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('parque.evento_id_seq', 8, true);
            parque       postgres    false    221                    0    0    informacion_parque_id_seq    SEQUENCE SET     G   SELECT pg_catalog.setval('parque.informacion_parque_id_seq', 1, true);
            parque       postgres    false    222                    0    0    noticia_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('parque.noticia_id_seq', 7, true);
            parque       postgres    false    223                    0    0    pictograma_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('parque.pictograma_id_seq', 1, true);
            parque       postgres    false    224                    0    0 
   pqr_id_seq    SEQUENCE SET     8   SELECT pg_catalog.setval('parque.pqr_id_seq', 1, true);
            parque       postgres    false    225                    0    0    reserva_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('parque.reserva_id_seq', 1, true);
            parque       postgres    false    227                    0    0 
   rol_id_seq    SEQUENCE SET     8   SELECT pg_catalog.setval('parque.rol_id_seq', 2, true);
            parque       postgres    false    229                    0    0    ticket_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('parque.ticket_id_seq', 1, true);
            parque       postgres    false    230                    0    0    usuario_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('parque.usuario_id_seq', 7, true);
            parque       postgres    false    231                    0    0    auditoria_id_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('security.auditoria_id_seq', 54, true);
            security       postgres    false    233                    0    0    token_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('security.token_id_seq', 4, true);
            security       postgres    false    235         9           2606    74014    aplicacion aplicacion_pkey 
   CONSTRAINT     a   ALTER TABLE ONLY parametrizacion.aplicacion
    ADD CONSTRAINT aplicacion_pkey PRIMARY KEY (id);
 M   ALTER TABLE ONLY parametrizacion.aplicacion DROP CONSTRAINT aplicacion_pkey;
       parametrizacion         postgres    false    213         +           2606    74016    pictograma pictograma_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY parque.pictograma
    ADD CONSTRAINT pictograma_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY parque.pictograma DROP CONSTRAINT pictograma_pkey;
       parque         postgres    false    206                    2606    74018    cabana pk_cabana 
   CONSTRAINT     N   ALTER TABLE ONLY parque.cabana
    ADD CONSTRAINT pk_cabana PRIMARY KEY (id);
 :   ALTER TABLE ONLY parque.cabana DROP CONSTRAINT pk_cabana;
       parque         postgres    false    199                    2606    74020    comentarios pk_comentarios 
   CONSTRAINT     X   ALTER TABLE ONLY parque.comentarios
    ADD CONSTRAINT pk_comentarios PRIMARY KEY (id);
 D   ALTER TABLE ONLY parque.comentarios DROP CONSTRAINT pk_comentarios;
       parque         postgres    false    200         #           2606    74022    estado_reserva pk_estado 
   CONSTRAINT     V   ALTER TABLE ONLY parque.estado_reserva
    ADD CONSTRAINT pk_estado PRIMARY KEY (id);
 B   ALTER TABLE ONLY parque.estado_reserva DROP CONSTRAINT pk_estado;
       parque         postgres    false    202         !           2606    74024    estado_pqr pk_estado_pqr 
   CONSTRAINT     V   ALTER TABLE ONLY parque.estado_pqr
    ADD CONSTRAINT pk_estado_pqr PRIMARY KEY (id);
 B   ALTER TABLE ONLY parque.estado_pqr DROP CONSTRAINT pk_estado_pqr;
       parque         postgres    false    201         %           2606    74026    evento pk_evento 
   CONSTRAINT     N   ALTER TABLE ONLY parque.evento
    ADD CONSTRAINT pk_evento PRIMARY KEY (id);
 :   ALTER TABLE ONLY parque.evento DROP CONSTRAINT pk_evento;
       parque         postgres    false    203         '           2606    74028 (   informacion_parque pk_informacion_parque 
   CONSTRAINT     f   ALTER TABLE ONLY parque.informacion_parque
    ADD CONSTRAINT pk_informacion_parque PRIMARY KEY (id);
 R   ALTER TABLE ONLY parque.informacion_parque DROP CONSTRAINT pk_informacion_parque;
       parque         postgres    false    204         )           2606    74030    noticia pk_noticia 
   CONSTRAINT     P   ALTER TABLE ONLY parque.noticia
    ADD CONSTRAINT pk_noticia PRIMARY KEY (id);
 <   ALTER TABLE ONLY parque.noticia DROP CONSTRAINT pk_noticia;
       parque         postgres    false    205         -           2606    74032 
   pqr pk_pqr 
   CONSTRAINT     H   ALTER TABLE ONLY parque.pqr
    ADD CONSTRAINT pk_pqr PRIMARY KEY (id);
 4   ALTER TABLE ONLY parque.pqr DROP CONSTRAINT pk_pqr;
       parque         postgres    false    207         /           2606    74034    reserva pk_reserva 
   CONSTRAINT     P   ALTER TABLE ONLY parque.reserva
    ADD CONSTRAINT pk_reserva PRIMARY KEY (id);
 <   ALTER TABLE ONLY parque.reserva DROP CONSTRAINT pk_reserva;
       parque         postgres    false    208         1           2606    74036 
   rol pk_rol 
   CONSTRAINT     H   ALTER TABLE ONLY parque.rol
    ADD CONSTRAINT pk_rol PRIMARY KEY (id);
 4   ALTER TABLE ONLY parque.rol DROP CONSTRAINT pk_rol;
       parque         postgres    false    209         3           2606    74038    ticket pk_ticket 
   CONSTRAINT     N   ALTER TABLE ONLY parque.ticket
    ADD CONSTRAINT pk_ticket PRIMARY KEY (id);
 :   ALTER TABLE ONLY parque.ticket DROP CONSTRAINT pk_ticket;
       parque         postgres    false    210         5           2606    74040    usuario pk_usuario 
   CONSTRAINT     P   ALTER TABLE ONLY parque.usuario
    ADD CONSTRAINT pk_usuario PRIMARY KEY (id);
 <   ALTER TABLE ONLY parque.usuario DROP CONSTRAINT pk_usuario;
       parque         postgres    false    211         ;           2606    74042    auditoria auditoria_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY security.auditoria
    ADD CONSTRAINT auditoria_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY security.auditoria DROP CONSTRAINT auditoria_pkey;
       security         postgres    false    232         7           2606    74044    tokens pk_tokens 
   CONSTRAINT     P   ALTER TABLE ONLY security.tokens
    ADD CONSTRAINT pk_tokens PRIMARY KEY (id);
 <   ALTER TABLE ONLY security.tokens DROP CONSTRAINT pk_tokens;
       security         postgres    false    212         J           2620    74045 $   aplicacion tg_parametrizacion_tokens    TRIGGER     �   CREATE TRIGGER tg_parametrizacion_tokens AFTER INSERT OR DELETE OR UPDATE ON parametrizacion.aplicacion FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 F   DROP TRIGGER tg_parametrizacion_tokens ON parametrizacion.aplicacion;
       parametrizacion       postgres    false    236    213         <           2620    74046    cabana tg_parque_cabana    TRIGGER     �   CREATE TRIGGER tg_parque_cabana AFTER INSERT OR DELETE OR UPDATE ON parque.cabana FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 0   DROP TRIGGER tg_parque_cabana ON parque.cabana;
       parque       postgres    false    236    199         K           2620    74047 -   comentario_cabana tg_parque_comentario_cabana    TRIGGER     �   CREATE TRIGGER tg_parque_comentario_cabana AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_cabana FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 F   DROP TRIGGER tg_parque_comentario_cabana ON parque.comentario_cabana;
       parque       postgres    false    215    236         L           2620    74048 -   comentario_evento tg_parque_comentario_evento    TRIGGER     �   CREATE TRIGGER tg_parque_comentario_evento AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_evento FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 F   DROP TRIGGER tg_parque_comentario_evento ON parque.comentario_evento;
       parque       postgres    false    216    236         M           2620    74049 /   comentario_noticia tg_parque_comentario_noticia    TRIGGER     �   CREATE TRIGGER tg_parque_comentario_noticia AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_noticia FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 H   DROP TRIGGER tg_parque_comentario_noticia ON parque.comentario_noticia;
       parque       postgres    false    217    236         N           2620    74050 5   comentario_pictograma tg_parque_comentario_pictograma    TRIGGER     �   CREATE TRIGGER tg_parque_comentario_pictograma AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_pictograma FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 N   DROP TRIGGER tg_parque_comentario_pictograma ON parque.comentario_pictograma;
       parque       postgres    false    236    218         =           2620    74051 !   comentarios tg_parque_comentarios    TRIGGER     �   CREATE TRIGGER tg_parque_comentarios AFTER INSERT OR DELETE OR UPDATE ON parque.comentarios FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 :   DROP TRIGGER tg_parque_comentarios ON parque.comentarios;
       parque       postgres    false    236    200         >           2620    74052    estado_pqr tg_parque_estado_pqr    TRIGGER     �   CREATE TRIGGER tg_parque_estado_pqr AFTER INSERT OR DELETE OR UPDATE ON parque.estado_pqr FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 8   DROP TRIGGER tg_parque_estado_pqr ON parque.estado_pqr;
       parque       postgres    false    236    201         ?           2620    74053 '   estado_reserva tg_parque_estado_reserva    TRIGGER     �   CREATE TRIGGER tg_parque_estado_reserva AFTER INSERT OR DELETE OR UPDATE ON parque.estado_reserva FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 @   DROP TRIGGER tg_parque_estado_reserva ON parque.estado_reserva;
       parque       postgres    false    236    202         @           2620    74054    evento tg_parque_evento    TRIGGER     �   CREATE TRIGGER tg_parque_evento AFTER INSERT OR DELETE OR UPDATE ON parque.evento FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 0   DROP TRIGGER tg_parque_evento ON parque.evento;
       parque       postgres    false    236    203         A           2620    74055 /   informacion_parque tg_parque_informacion_parque    TRIGGER     �   CREATE TRIGGER tg_parque_informacion_parque AFTER INSERT OR DELETE OR UPDATE ON parque.informacion_parque FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 H   DROP TRIGGER tg_parque_informacion_parque ON parque.informacion_parque;
       parque       postgres    false    236    204         B           2620    74056    noticia tg_parque_noticia    TRIGGER     �   CREATE TRIGGER tg_parque_noticia AFTER INSERT OR DELETE OR UPDATE ON parque.noticia FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 2   DROP TRIGGER tg_parque_noticia ON parque.noticia;
       parque       postgres    false    236    205         C           2620    74057    pictograma tg_parque_pictograma    TRIGGER     �   CREATE TRIGGER tg_parque_pictograma AFTER INSERT OR DELETE OR UPDATE ON parque.pictograma FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 8   DROP TRIGGER tg_parque_pictograma ON parque.pictograma;
       parque       postgres    false    236    206         D           2620    74058    pqr tg_parque_pqr    TRIGGER     �   CREATE TRIGGER tg_parque_pqr AFTER INSERT OR DELETE OR UPDATE ON parque.pqr FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 *   DROP TRIGGER tg_parque_pqr ON parque.pqr;
       parque       postgres    false    236    207         E           2620    74059    reserva tg_parque_reserva    TRIGGER     �   CREATE TRIGGER tg_parque_reserva AFTER INSERT OR DELETE OR UPDATE ON parque.reserva FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 2   DROP TRIGGER tg_parque_reserva ON parque.reserva;
       parque       postgres    false    236    208         O           2620    74060 '   reserva_cabana tg_parque_reserva_cabana    TRIGGER     �   CREATE TRIGGER tg_parque_reserva_cabana AFTER INSERT OR DELETE OR UPDATE ON parque.reserva_cabana FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 @   DROP TRIGGER tg_parque_reserva_cabana ON parque.reserva_cabana;
       parque       postgres    false    226    236         P           2620    74061 '   reserva_ticket tg_parque_reserva_ticket    TRIGGER     �   CREATE TRIGGER tg_parque_reserva_ticket AFTER INSERT OR DELETE OR UPDATE ON parque.reserva_ticket FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 @   DROP TRIGGER tg_parque_reserva_ticket ON parque.reserva_ticket;
       parque       postgres    false    228    236         F           2620    74062    rol tg_parque_rol    TRIGGER     �   CREATE TRIGGER tg_parque_rol AFTER INSERT OR DELETE OR UPDATE ON parque.rol FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 *   DROP TRIGGER tg_parque_rol ON parque.rol;
       parque       postgres    false    236    209         G           2620    74063    ticket tg_parque_ticket    TRIGGER     �   CREATE TRIGGER tg_parque_ticket AFTER INSERT OR DELETE OR UPDATE ON parque.ticket FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 0   DROP TRIGGER tg_parque_ticket ON parque.ticket;
       parque       postgres    false    210    236         H           2620    74064    usuario tg_parque_usuario    TRIGGER     �   CREATE TRIGGER tg_parque_usuario AFTER INSERT OR DELETE OR UPDATE ON parque.usuario FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 2   DROP TRIGGER tg_parque_usuario ON parque.usuario;
       parque       postgres    false    236    211         I           2620    74065     tokens tg_parametrizacion_tokens    TRIGGER     �   CREATE TRIGGER tg_parametrizacion_tokens AFTER INSERT OR DELETE OR UPDATE ON security.tokens FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 ;   DROP TRIGGER tg_parametrizacion_tokens ON security.tokens;
       security       postgres    false    212    236                                                                                                                                                                                                                                                                          3033.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014233 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3019.dat                                                                                            0000600 0004000 0002000 00000000134 13632062303 0014242 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        25	asdw	2	1	asdsad	123	asd	\N	\N
26	sdfds	2	2	sdfdsf	234	sdfds	sdf	1999-01-01 00:00:00
\.


                                                                                                                                                                                                                                                                                                                                                                                                                                    3035.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014235 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3036.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014236 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3037.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014237 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3038.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014240 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3020.dat                                                                                            0000600 0004000 0002000 00000000057 13632062303 0014236 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        6	1999-01-01 00:00:00	asdasd	12	t	1	\N	\N
\.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 3021.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014230 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3022.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014231 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3023.dat                                                                                            0000600 0004000 0002000 00000001371 13632062303 0014241 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        4	Evento	2020-03-10 17:53:16.98863	Descripcion	evento1.jpg@evento2.jpt@evento3.jpg		12	fakkfakdf	2020-03-10 17:54:43.964423	2020-04-15 17:53:16.98863
5	Evento	2020-03-10 17:53:34.499307	Descripcion	evento1.jpg@evento2.jpt@evento3.jpg		12	fakkfakdf	2020-03-10 17:54:43.964423	2020-04-15 17:53:34.499307
6	Evento	2020-03-10 17:53:35.137207	Descripcion	evento1.jpg@evento2.jpt@evento3.jpg		12	fakkfakdf	2020-03-10 17:54:43.964423	2020-04-15 17:53:35.137207
7	Evento	2020-03-10 17:53:44.41219	Descripcion	evento1.jpg@evento2.jpt@evento3.jpg		12	fakkfakdf	2020-03-10 17:54:43.964423	2020-03-15 17:53:44.41219
8	Evento	2020-03-10 17:53:45.053499	Descripcion	evento1.jpg@evento2.jpt@evento3.jpg		12	fakkfakdf	2020-03-10 17:54:43.964423	2020-03-15 17:53:45.053499
\.


                                                                                                                                                                                                                                                                       3024.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014233 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3025.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014234 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3026.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014235 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3027.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014236 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3028.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014237 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3046.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014237 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3048.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014241 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3029.dat                                                                                            0000600 0004000 0002000 00000000017 13632062303 0014243 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        2	cliente
\.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 3030.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014230 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3031.dat                                                                                            0000600 0004000 0002000 00000000143 13632062303 0014234 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        7	usuario	aaa	cc	123456789	faca	usuario1@gmail.com	1234	img1	t	t	2	asdasd	2020-03-10 00:00:00
\.


                                                                                                                                                                                                                                                                                                                                                                                                                             3052.dat                                                                                            0000600 0004000 0002000 00000027260 13632062303 0014250 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        1	2020-03-09 19:57:34.438211	DELETE	parque	cabana		postgres	{"id_anterior": 24, "nombre_anterior": "Casi", "precio_anterior": 23, "capacidad_anterior": 2, "calificacion_anterior": 12, "imagenes_url_anterior": "asdd", "comentarios_id_anterior": "asd"}	24
2	2020-03-09 20:00:39.946159	DELETE	parque	comentarios		postgres	{"id_anterior": 1, "reportado_anterior": true, "usuario_id_anterior": 7, "descripcion_anterior": "hgjhjk", "calificacion_anterior": 987, "fecha_publicacion_anterior": "1999-01-01T00:00:00"}	1
3	2020-03-09 20:08:55.864435	INSERT	parque	cabana		postgres	{"id_nuevo": 25, "nombre_nuevo": "asdw", "precio_nuevo": 1, "capacidad_nuevo": 2, "calificacion_nuevo": 123, "imagenes_url_nuevo": "asdsad", "comentarios_id_nuevo": "asd"}	25
4	2020-03-10 16:55:32.302839	INSERT	parque	cabana	sdf	postgres	{"id_nuevo": 26, "token_nuevo": "sdf", "nombre_nuevo": "sdfds", "precio_nuevo": 2, "capacidad_nuevo": 2, "calificacion_nuevo": 234, "imagenes_url_nuevo": "sdfdsf", "comentarios_id_nuevo": "sdfds", "last_modification_nuevo": "1999-01-01T00:00:00"}	26
5	2020-03-10 17:05:03.354948	INSERT	parque	comentarios	asd	postgres	{"id_nuevo": 3, "token_nuevo": "asd", "reportado_nuevo": true, "usuario_id_nuevo": 1, "descripcion_nuevo": "asdsad", "calificacion_nuevo": 1, "fecha_publicacion_nuevo": "1999-01-01T00:00:00", "last_modification_nuevo": "1999-01-01T00:00:00"}	3
6	2020-03-10 17:05:09.312083	DELETE	parque	comentarios	asd	postgres	{"id_anterior": 3, "token_anterior": "asd", "reportado_anterior": true, "usuario_id_anterior": 1, "descripcion_anterior": "asdsad", "calificacion_anterior": 1, "fecha_publicacion_anterior": "1999-01-01T00:00:00", "last_modification_anterior": "1999-01-01T00:00:00"}	3
7	2020-03-10 17:10:43.963879	INSERT	parque	estado_pqr		postgres	{"id_nuevo": 1, "nombre_nuevo": "asdas"}	1
8	2020-03-10 17:11:17.955338	DELETE	parque	estado_pqr		postgres	{"id_anterior": 1, "nombre_anterior": "asdas"}	1
9	2020-03-10 17:11:46.530445	INSERT	parque	estado_reserva		postgres	{"id_nuevo": 1, "nombre_nuevo": "sad"}	1
10	2020-03-10 17:11:51.029358	DELETE	parque	estado_reserva		postgres	{"id_anterior": 1, "nombre_anterior": "sad"}	1
11	2020-03-10 17:12:30.517204	INSERT	parque	evento	asdsad	postgres	{"id_nuevo": 2, "token_nuevo": "asdsad", "nombre_nuevo": "asdas", "descripcion_nuevo": "asdsad", "calificacion_nuevo": 12, "imagenes_url_nuevo": "asdasd", "comentarios_id_nuevo": "asdasd", "fecha_publicacion_nuevo": "1999-01-01T00:00:00", "last_modification_nuevo": null}	2
12	2020-03-10 17:12:33.736802	DELETE	parque	evento	asdsad	postgres	{"id_anterior": 2, "token_anterior": "asdsad", "nombre_anterior": "asdas", "descripcion_anterior": "asdsad", "calificacion_anterior": 12, "imagenes_url_anterior": "asdasd", "comentarios_id_anterior": "asdasd", "fecha_publicacion_anterior": "1999-01-01T00:00:00", "last_modification_anterior": null}	2
13	2020-03-10 17:13:05.189565	INSERT	parque	informacion_parque	asdasd	postgres	{"id_nuevo": 1, "token_nuevo": "asdasd", "propety_nuevo": "asds", "descripcion_nuevo": "asdas", "imagenes_url_nuevo": "asdsa", "last_modification_nuevo": null}	1
14	2020-03-10 17:13:08.555029	DELETE	parque	informacion_parque	asdasd	postgres	{"id_anterior": 1, "token_anterior": "asdasd", "propety_anterior": "asds", "descripcion_anterior": "asdas", "imagenes_url_anterior": "asdsa", "last_modification_anterior": null}	1
19	2020-03-10 17:15:39.335215	INSERT	parque	comentarios	sadas	postgres	{"id_nuevo": 5, "token_nuevo": "sadas", "reportado_nuevo": true, "usuario_id_nuevo": 2, "descripcion_nuevo": "asdasd", "calificacion_nuevo": 12, "fecha_publicacion_nuevo": "1999-01-01T00:00:00", "last_modification_nuevo": "1999-01-01T00:00:00"}	5
22	2020-03-10 17:17:08.922109	DELETE	parque	comentarios	sadas	postgres	{"id_anterior": 5, "token_anterior": "sadas", "reportado_anterior": true, "usuario_id_anterior": 2, "descripcion_anterior": "asdasd", "calificacion_anterior": 12, "fecha_publicacion_anterior": "1999-01-01T00:00:00", "last_modification_anterior": "1999-01-01T00:00:00"}	5
23	2020-03-10 17:17:39.880755	INSERT	parque	comentarios		postgres	{"id_nuevo": 6, "reportado_nuevo": true, "usuario_id_nuevo": 1, "descripcion_nuevo": "asdasd", "calificacion_nuevo": 12, "fecha_publicacion_nuevo": "1999-01-01T00:00:00", "last_modification_nuevo": null}	6
24	2020-03-10 17:19:33.568317	INSERT	parque	noticia	asdsa	postgres	{"id_nuevo": 7, "token_nuevo": "asdsa", "titulo_nuevo": "asdasd", "imagen_url_nuevo": "asdsad", "descripcion_nuevo": "asdas", "calificacion_nuevo": 1, "comentarios_id_nuevo": "1", "fecha_publicacion_nuevo": "1999-01-01T00:00:00", "last_modification_nuevo": "1999-01-01T00:00:00"}	7
25	2020-03-10 17:19:43.754092	DELETE	parque	noticia	asdsa	postgres	{"id_anterior": 7, "token_anterior": "asdsa", "titulo_anterior": "asdasd", "imagen_url_anterior": "asdsad", "descripcion_anterior": "asdas", "calificacion_anterior": 1, "comentarios_id_anterior": "1", "fecha_publicacion_anterior": "1999-01-01T00:00:00", "last_modification_anterior": "1999-01-01T00:00:00"}	7
26	2020-03-10 17:20:19.411508	INSERT	parque	pictograma	asdsa	postgres	{"id_nuevo": 1, "token_nuevo": "asdsa", "nombre_nuevo": "asdsad", "descripcion_nuevo": "asdas", "calificacion_nuevo": 12, "imagenes_url_nuevo": "asdsad", "comentarios_id_nuevo": "asdasd", "last_modification_nuevo": "1999-01-01T00:00:00"}	1
27	2020-03-10 17:20:24.132975	DELETE	parque	pictograma	asdsa	postgres	{"id_anterior": 1, "token_anterior": "asdsa", "nombre_anterior": "asdsad", "descripcion_anterior": "asdas", "calificacion_anterior": 12, "imagenes_url_anterior": "asdsad", "comentarios_id_anterior": "asdasd", "last_modification_anterior": "1999-01-01T00:00:00"}	1
28	2020-03-10 17:21:07.01602	INSERT	parque	pqr	asdas	postgres	{"id_nuevo": 1, "token_nuevo": "asdas", "pregunta_nuevo": "asdas", "estado_id_nuevo": 1, "respuesta_nuevo": "asdas", "usuario_id_nuevo": 1, "fecha_publicacion_nuevo": "1999-01-01T00:00:00", "last_modification_nuevo": null}	1
29	2020-03-10 17:21:11.27882	DELETE	parque	pqr	asdas	postgres	{"id_anterior": 1, "token_anterior": "asdas", "pregunta_anterior": "asdas", "estado_id_anterior": 1, "respuesta_anterior": "asdas", "usuario_id_anterior": 1, "fecha_publicacion_anterior": "1999-01-01T00:00:00", "last_modification_anterior": null}	1
30	2020-03-10 17:21:46.242941	INSERT	parque	reserva	asdsad	postgres	{"id_nuevo": 1, "token_nuevo": "asdsad", "precio_nuevo": 123, "estado_id_nuevo": 1, "usuario_id_nuevo": 1, "fecha_compra_nuevo": "1999-01-01T00:00:00", "last_modification_nuevo": null}	1
31	2020-03-10 17:21:48.716672	DELETE	parque	reserva	asdsad	postgres	{"id_anterior": 1, "token_anterior": "asdsad", "precio_anterior": 123, "estado_id_anterior": 1, "usuario_id_anterior": 1, "fecha_compra_anterior": "1999-01-01T00:00:00", "last_modification_anterior": null}	1
32	2020-03-10 17:22:44.13991	INSERT	parque	rol		postgres	{"id_nuevo": 1, "nombre_nuevo": "asdasd"}	1
33	2020-03-10 17:22:50.15966	DELETE	parque	rol		postgres	{"id_anterior": 1, "nombre_anterior": "asdasd"}	1
34	2020-03-10 17:23:15.259207	INSERT	parque	ticket	sadas	postgres	{"id_nuevo": 1, "token_nuevo": "sadas", "nombre_nuevo": "asd", "precio_nuevo": 121, "last_modification_nuevo": null}	1
35	2020-03-10 17:23:19.581181	DELETE	parque	ticket	sadas	postgres	{"id_anterior": 1, "token_anterior": "sadas", "nombre_anterior": "asd", "precio_anterior": 121, "last_modification_anterior": null}	1
36	2020-03-10 17:23:53.089811	INSERT	parque	usuario	asd	postgres	{"id_nuevo": 1, "clave_nuevo": "asd", "token_nuevo": "asd", "nombre_nuevo": "asd", "rol_id_nuevo": 1, "apellido_nuevo": "asd", "icono_url_nuevo": "asd", "estado_cuenta_nuevo": true, "tipo_documento_nuevo": "asd", "lugar_expedicion_nuevo": "asd", "numero_documento_nuevo": 12, "last_modification_nuevo": null, "correo_electronico_nuevo": "asd", "verificacion_cuenta_nuevo": true}	1
37	2020-03-10 17:23:56.299387	DELETE	parque	usuario	asd	postgres	{"id_anterior": 1, "clave_anterior": "asd", "token_anterior": "asd", "nombre_anterior": "asd", "rol_id_anterior": 1, "apellido_anterior": "asd", "icono_url_anterior": "asd", "estado_cuenta_anterior": true, "tipo_documento_anterior": "asd", "lugar_expedicion_anterior": "asd", "numero_documento_anterior": 12, "last_modification_anterior": null, "correo_electronico_anterior": "asd", "verificacion_cuenta_anterior": true}	1
38	2020-03-10 17:26:36.870146	INSERT	security	tokens	asd	postgres	{"id_nuevo": 1, "token_nuevo": "asd", "user_id_nuevo": 1, "aplicacion_id_nuevo": 1, "fecha_generacion_nuevo": "1999-01-01T00:00:00", "fecha_vencimiento_nuevo": "1999-01-01T00:00:00"}	1
39	2020-03-10 17:26:40.918378	DELETE	security	tokens	asd	postgres	{"id_anterior": 1, "token_anterior": "asd", "user_id_anterior": 1, "aplicacion_id_anterior": 1, "fecha_generacion_anterior": "1999-01-01T00:00:00", "fecha_vencimiento_anterior": "1999-01-01T00:00:00"}	1
41	2020-03-10 17:53:16.98863	INSERT	parque	evento	fakkfakdf	postgres	{"id_nuevo": 4, "fecha_nuevo": "2020-04-15T17:53:16.98863", "token_nuevo": "fakkfakdf", "nombre_nuevo": "Evento", "descripcion_nuevo": "Descripcion", "calificacion_nuevo": 12, "imagenes_url_nuevo": "evento1.jpg@evento2.jpt@evento3.jpg", "comentarios_id_nuevo": "", "fecha_publicacion_nuevo": "2020-03-10T17:53:16.98863", "last_modification_nuevo": null}	4
42	2020-03-10 17:53:34.499307	INSERT	parque	evento	fakkfakdf	postgres	{"id_nuevo": 5, "fecha_nuevo": "2020-04-15T17:53:34.499307", "token_nuevo": "fakkfakdf", "nombre_nuevo": "Evento", "descripcion_nuevo": "Descripcion", "calificacion_nuevo": 12, "imagenes_url_nuevo": "evento1.jpg@evento2.jpt@evento3.jpg", "comentarios_id_nuevo": "", "fecha_publicacion_nuevo": "2020-03-10T17:53:34.499307", "last_modification_nuevo": null}	5
43	2020-03-10 17:53:35.137207	INSERT	parque	evento	fakkfakdf	postgres	{"id_nuevo": 6, "fecha_nuevo": "2020-04-15T17:53:35.137207", "token_nuevo": "fakkfakdf", "nombre_nuevo": "Evento", "descripcion_nuevo": "Descripcion", "calificacion_nuevo": 12, "imagenes_url_nuevo": "evento1.jpg@evento2.jpt@evento3.jpg", "comentarios_id_nuevo": "", "fecha_publicacion_nuevo": "2020-03-10T17:53:35.137207", "last_modification_nuevo": null}	6
44	2020-03-10 17:53:44.41219	INSERT	parque	evento	fakkfakdf	postgres	{"id_nuevo": 7, "fecha_nuevo": "2020-03-15T17:53:44.41219", "token_nuevo": "fakkfakdf", "nombre_nuevo": "Evento", "descripcion_nuevo": "Descripcion", "calificacion_nuevo": 12, "imagenes_url_nuevo": "evento1.jpg@evento2.jpt@evento3.jpg", "comentarios_id_nuevo": "", "fecha_publicacion_nuevo": "2020-03-10T17:53:44.41219", "last_modification_nuevo": null}	7
45	2020-03-10 17:53:45.053499	INSERT	parque	evento	fakkfakdf	postgres	{"id_nuevo": 8, "fecha_nuevo": "2020-03-15T17:53:45.053499", "token_nuevo": "fakkfakdf", "nombre_nuevo": "Evento", "descripcion_nuevo": "Descripcion", "calificacion_nuevo": 12, "imagenes_url_nuevo": "evento1.jpg@evento2.jpt@evento3.jpg", "comentarios_id_nuevo": "", "fecha_publicacion_nuevo": "2020-03-10T17:53:45.053499", "last_modification_nuevo": null}	8
46	2020-03-10 17:54:43.964423	UPDATE	parque	evento	fakkfakdf	postgres	{}	4
47	2020-03-10 17:54:43.964423	UPDATE	parque	evento	fakkfakdf	postgres	{}	5
48	2020-03-10 17:54:43.964423	UPDATE	parque	evento	fakkfakdf	postgres	{}	6
49	2020-03-10 17:54:43.964423	UPDATE	parque	evento	fakkfakdf	postgres	{}	7
50	2020-03-10 17:54:43.964423	UPDATE	parque	evento	fakkfakdf	postgres	{}	8
51	2020-03-10 20:37:35.198304	INSERT	parque	rol		postgres	{"id_nuevo": 2, "nombre_nuevo": "cliente"}	2
54	2020-03-10 21:54:12.947103	INSERT	parque	usuario	asdasd	postgres	{"id_nuevo": 7, "clave_nuevo": "1234", "token_nuevo": "asdasd", "nombre_nuevo": "usuario", "rol_id_nuevo": 2, "apellido_nuevo": "aaa", "icono_url_nuevo": "img1", "estado_cuenta_nuevo": true, "tipo_documento_nuevo": "cc", "lugar_expedicion_nuevo": "faca", "numero_documento_nuevo": 123456789, "last_modification_nuevo": "2020-03-10T00:00:00", "correo_electronico_nuevo": "usuario1@gmail.com", "verificacion_cuenta_nuevo": true}	7
\.


                                                                                                                                                                                                                                                                                                                                                3032.dat                                                                                            0000600 0004000 0002000 00000000005 13632062303 0014232 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           restore.sql                                                                                         0000600 0004000 0002000 00000302672 13632062303 0015374 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        --
-- NOTE:
--
-- File paths need to be edited. Search for $$PATH$$ and
-- replace it with the path to the directory containing
-- the extracted data files.
--
--
-- PostgreSQL database dump
--

-- Dumped from database version 10.7
-- Dumped by pg_dump version 11.1

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE piedras;
--
-- Name: piedras; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE piedras WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'Spanish_Colombia.1252' LC_CTYPE = 'Spanish_Colombia.1252';


ALTER DATABASE piedras OWNER TO postgres;

\connect piedras

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: parametrizacion; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA parametrizacion;


ALTER SCHEMA parametrizacion OWNER TO postgres;

--
-- Name: parque; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA parque;


ALTER SCHEMA parque OWNER TO postgres;

--
-- Name: SCHEMA parque; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA parque IS 'Esquema dedicado a contener todas las tablas,triggets, procedimientos almacenados, jobs correspondientes a todas las tablas dedicadas a lo directamente relacionado con el parque';


--
-- Name: security; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA security;


ALTER SCHEMA security OWNER TO postgres;

--
-- Name: SCHEMA security; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA security IS 'Esquema dedicado a almacenar la seguridad del sistema';


--
-- Name: f_log_auditoria(); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.f_log_auditoria() RETURNS trigger
    LANGUAGE plpgsql
    AS $$
	 DECLARE
		_pk TEXT :='';		-- Representa la llave primaria de la tabla que esta siedno modificada.
		_sql TEXT;		-- Variable para la creacion del procedured.
		_column_guia RECORD; 	-- Variable para el FOR guarda los nombre de las columnas.
		_column_key RECORD; 	-- Variable para el FOR guarda los PK de las columnas.
		_token TEXT;	-- Almacena el usuario que genera el cambio.
		_user_db TEXT;		-- Almacena el usuario de bd que genera la transaccion.
		_control INT;		-- Variabel de control par alas llaves primarias.
		_count_key INT = 0;	-- Cantidad de columnas pertenecientes al PK.
		_sql_insert TEXT;	-- Variable para la construcción del insert del json de forma dinamica.
		_sql_delete TEXT;	-- Variable para la construcción del delete del json de forma dinamica.
		_sql_update TEXT;	-- Variable para la construcción del update del json de forma dinamica.
		_new_data RECORD; 	-- Fila que representa los campos nuevos del registro.
		_old_data RECORD;	-- Fila que representa los campos viejos del registro.
	BEGIN
			-- Se genera la evaluacion para determianr el tipo de accion sobre la tabla
		 IF (TG_OP = 'INSERT') THEN
			_new_data := NEW;
			_old_data := NEW;
		ELSEIF (TG_OP = 'UPDATE') THEN
			_new_data := NEW;
			_old_data := OLD;
		ELSE
			_new_data := OLD;
			_old_data := OLD;
		END IF;

		-- Se genera la evaluacion para determianr el tipo de accion sobre la tabla
		IF ((SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = TG_TABLE_SCHEMA AND table_name = TG_TABLE_NAME AND column_name = 'id' ) > 0) THEN
			_pk := _new_data.id;
		ELSE
			_pk := '-1';
		END IF;

		-- Se valida que exista el campo modified_by
		IF ((SELECT COUNT(*) FROM information_schema.columns WHERE table_schema = TG_TABLE_SCHEMA AND table_name = TG_TABLE_NAME AND column_name = 'token') > 0) THEN
			_token := _new_data.token;
		ELSE
			_token := '';
		END IF;

		-- Se guarda el susuario de bd que genera la transaccion
		_user_db := (SELECT CURRENT_USER);

		-- Se evalua que exista el procedimeinto adecuado
		IF (SELECT COUNT(*) FROM security.function_db_view acfdv WHERE acfdv.b_function = 'field_audit' AND acfdv.b_type_parameters = TG_TABLE_SCHEMA || '.'|| TG_TABLE_NAME || ', '|| TG_TABLE_SCHEMA || '.'|| TG_TABLE_NAME || ', character varying, character varying, character varying, text, character varying, text, text') > 0
			THEN
				-- Se realiza la invocación del procedured generado dinamivamente
				PERFORM security.field_audit(_new_data, _old_data, TG_OP, _token, _user_db , _pk, ''::text);
		ELSE
			-- Se empieza la construcción del Procedured generico
			_sql := 'CREATE OR REPLACE FUNCTION security.field_audit( _data_new '|| TG_TABLE_SCHEMA || '.'|| TG_TABLE_NAME || ', _data_old '|| TG_TABLE_SCHEMA || '.'|| TG_TABLE_NAME || ', _accion character varying, _token text, _user_db character varying, _table_pk text, _init text)'
			|| ' RETURNS TEXT AS ''
'
			|| '
'
	|| '	DECLARE
'
	|| '		_column_data TEXT;
	 	_datos jsonb;
	 	
'
	|| '	BEGIN
			_datos = ''''{}'''';
';
			-- Se evalua si hay que actualizar la pk del registro de auditoria.
			IF _pk = '-1'
				THEN
					_sql := _sql
					|| '
		_column_data := ';

					-- Se genera el update con la clave pk de la tabla
					SELECT
						COUNT(isk.column_name)
					INTO
						_control
					FROM
						information_schema.table_constraints istc JOIN information_schema.key_column_usage isk ON isk.constraint_name = istc.constraint_name
					WHERE
						istc.table_schema = TG_TABLE_SCHEMA
					 AND	istc.table_name = TG_TABLE_NAME
					 AND	istc.constraint_type ilike '%primary%';

					-- Se agregan las columnas que componen la pk de la tabla.
					FOR _column_key IN SELECT
							isk.column_name
						FROM
							information_schema.table_constraints istc JOIN information_schema.key_column_usage isk ON isk.constraint_name = istc.constraint_name
						WHERE
							istc.table_schema = TG_TABLE_SCHEMA
						 AND	istc.table_name = TG_TABLE_NAME
						 AND	istc.constraint_type ilike '%primary%'
						ORDER BY 
							isk.ordinal_position  LOOP

						_sql := _sql || ' _data_new.' || _column_key.column_name;
						
						_count_key := _count_key + 1 ;
						
						IF _count_key < _control THEN
							_sql :=	_sql || ' || ' || ''''',''''' || ' ||';
						END IF;
					END LOOP;
				_sql := _sql || ';';
			END IF;

			_sql_insert:='
		IF _accion = ''''INSERT''''
			THEN
				';
			_sql_delete:='
		ELSEIF _accion = ''''DELETE''''
			THEN
				';
			_sql_update:='
		ELSE
			';

			-- Se genera el ciclo de agregado de columnas para el nuevo procedured
			FOR _column_guia IN SELECT column_name, data_type FROM information_schema.columns WHERE table_schema = TG_TABLE_SCHEMA AND table_name = TG_TABLE_NAME
				LOOP
						
					_sql_insert:= _sql_insert || '_datos := _datos || json_build_object('''''
					|| _column_guia.column_name
					|| '_nuevo'
					|| ''''', '
					|| '_data_new.'
					|| _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea', 'USER-DEFINED') THEN 
						_sql_insert:= _sql_insert
						||'::text';
					END IF;

					_sql_insert:= _sql_insert || ')::jsonb;
				';

					_sql_delete := _sql_delete || '_datos := _datos || json_build_object('''''
					|| _column_guia.column_name
					|| '_anterior'
					|| ''''', '
					|| '_data_old.'
					|| _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea', 'USER-DEFINED') THEN 
						_sql_delete:= _sql_delete
						||'::text';
					END IF;

					_sql_delete:= _sql_delete || ')::jsonb;
				';

					_sql_update := _sql_update || 'IF _data_old.' || _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea','USER-DEFINED') THEN 
						_sql_update:= _sql_update
						||'::text';
					END IF;

					_sql_update:= _sql_update || ' <> _data_new.' || _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea','USER-DEFINED') THEN 
						_sql_update:= _sql_update
						||'::text';
					END IF;

					_sql_update:= _sql_update || '
				THEN _datos := _datos || json_build_object('''''
					|| _column_guia.column_name
					|| '_anterior'
					|| ''''', '
					|| '_data_old.'
					|| _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea','USER-DEFINED') THEN 
						_sql_update:= _sql_update
						||'::text';
					END IF;

					_sql_update:= _sql_update
					|| ', '''''
					|| _column_guia.column_name
					|| '_nuevo'
					|| ''''', _data_new.'
					|| _column_guia.column_name;

					IF _column_guia.data_type IN ('bytea', 'USER-DEFINED') THEN 
						_sql_update:= _sql_update
						||'::text';
					END IF;

					_sql_update:= _sql_update
					|| ')::jsonb;
			END IF;
			';
			END LOOP;

			-- Se le agrega la parte final del procedured generico
			
			_sql:= _sql || _sql_insert || _sql_delete || _sql_update
			|| ' 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			''''' || TG_TABLE_SCHEMA || ''''',
			''''' || TG_TABLE_NAME || ''''',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;'''
|| '
LANGUAGE plpgsql;';

			-- Se genera la ejecución de _sql, es decir se crea el nuevo procedured de forma generica.
			EXECUTE _sql;

		-- Se realiza la invocación del procedured generado dinamivamente
			PERFORM security.field_audit(_new_data, _old_data, TG_OP::character varying, _token, _user_db, _pk, ''::text);

		END IF;

		RETURN NULL;

END;
$$;


ALTER FUNCTION security.f_log_auditoria() OWNER TO postgres;

SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: cabana; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.cabana (
    id integer NOT NULL,
    nombre text NOT NULL,
    capacidad integer NOT NULL,
    precio double precision NOT NULL,
    imagenes_url text NOT NULL,
    calificacion double precision NOT NULL,
    comentarios_id text NOT NULL,
    token text,
    last_modification timestamp without time zone
);


ALTER TABLE parque.cabana OWNER TO postgres;

--
-- Name: TABLE cabana; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.cabana IS 'Tabla dedicada a almacenar los datos de las cabañas del parque arqueológico de facatativa.';


--
-- Name: field_audit(parque.cabana, parque.cabana, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.cabana, _data_old parque.cabana, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				_datos := _datos || json_build_object('capacidad_nuevo', _data_new.capacidad)::jsonb;
				_datos := _datos || json_build_object('precio_nuevo', _data_new.precio)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('capacidad_anterior', _data_old.capacidad)::jsonb;
				_datos := _datos || json_build_object('precio_anterior', _data_old.precio)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			IF _data_old.capacidad <> _data_new.capacidad
				THEN _datos := _datos || json_build_object('capacidad_anterior', _data_old.capacidad, 'capacidad_nuevo', _data_new.capacidad)::jsonb;
			END IF;
			IF _data_old.precio <> _data_new.precio
				THEN _datos := _datos || json_build_object('precio_anterior', _data_old.precio, 'precio_nuevo', _data_new.precio)::jsonb;
			END IF;
			IF _data_old.imagenes_url <> _data_new.imagenes_url
				THEN _datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url, 'imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.comentarios_id <> _data_new.comentarios_id
				THEN _datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id, 'comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'cabana',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.cabana, _data_old parque.cabana, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: comentarios; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.comentarios (
    id bigint NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    descripcion text NOT NULL,
    calificacion double precision NOT NULL,
    reportado boolean NOT NULL,
    usuario_id integer NOT NULL,
    last_modification timestamp without time zone,
    token text
);


ALTER TABLE parque.comentarios OWNER TO postgres;

--
-- Name: TABLE comentarios; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.comentarios IS 'tabla dedicada a almacenar los datos de los comentarios del sistema,tanto como de las noticias,eventos y pictogramas del parque arqueologico';


--
-- Name: field_audit(parque.comentarios, parque.comentarios, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.comentarios, _data_old parque.comentarios, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('descripcion_nuevo', _data_new.descripcion)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('reportado_nuevo', _data_new.reportado)::jsonb;
				_datos := _datos || json_build_object('usuario_id_nuevo', _data_new.usuario_id)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('reportado_anterior', _data_old.reportado)::jsonb;
				_datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.fecha_publicacion <> _data_new.fecha_publicacion
				THEN _datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion, 'fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
			END IF;
			IF _data_old.descripcion <> _data_new.descripcion
				THEN _datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion, 'descripcion_nuevo', _data_new.descripcion)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.reportado <> _data_new.reportado
				THEN _datos := _datos || json_build_object('reportado_anterior', _data_old.reportado, 'reportado_nuevo', _data_new.reportado)::jsonb;
			END IF;
			IF _data_old.usuario_id <> _data_new.usuario_id
				THEN _datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id, 'usuario_id_nuevo', _data_new.usuario_id)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'comentarios',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.comentarios, _data_old parque.comentarios, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: estado_pqr; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.estado_pqr (
    id integer NOT NULL,
    nombre text NOT NULL
);


ALTER TABLE parque.estado_pqr OWNER TO postgres;

--
-- Name: field_audit(parque.estado_pqr, parque.estado_pqr, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.estado_pqr, _data_old parque.estado_pqr, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'estado_pqr',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.estado_pqr, _data_old parque.estado_pqr, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: estado_reserva; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.estado_reserva (
    id integer NOT NULL,
    nombre text NOT NULL
);


ALTER TABLE parque.estado_reserva OWNER TO postgres;

--
-- Name: TABLE estado_reserva; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.estado_reserva IS 'Tabla dedicada a almacenar los estados de los procesos del sistema';


--
-- Name: field_audit(parque.estado_reserva, parque.estado_reserva, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.estado_reserva, _data_old parque.estado_reserva, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'estado_reserva',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.estado_reserva, _data_old parque.estado_reserva, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: evento; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.evento (
    id integer NOT NULL,
    nombre text NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    descripcion text NOT NULL,
    imagenes_url text NOT NULL,
    comentarios_id text NOT NULL,
    calificacion double precision,
    token text,
    last_modification timestamp without time zone,
    fecha timestamp without time zone
);


ALTER TABLE parque.evento OWNER TO postgres;

--
-- Name: TABLE evento; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.evento IS 'Tabla dedicada a almacenar los datos de los eventos del parque arqueologico de facatativa';


--
-- Name: field_audit(parque.evento, parque.evento, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.evento, _data_old parque.evento, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('descripcion_nuevo', _data_new.descripcion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				_datos := _datos || json_build_object('fecha_nuevo', _data_new.fecha)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				_datos := _datos || json_build_object('fecha_anterior', _data_old.fecha)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			IF _data_old.fecha_publicacion <> _data_new.fecha_publicacion
				THEN _datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion, 'fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
			END IF;
			IF _data_old.descripcion <> _data_new.descripcion
				THEN _datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion, 'descripcion_nuevo', _data_new.descripcion)::jsonb;
			END IF;
			IF _data_old.imagenes_url <> _data_new.imagenes_url
				THEN _datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url, 'imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
			END IF;
			IF _data_old.comentarios_id <> _data_new.comentarios_id
				THEN _datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id, 'comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			IF _data_old.fecha <> _data_new.fecha
				THEN _datos := _datos || json_build_object('fecha_anterior', _data_old.fecha, 'fecha_nuevo', _data_new.fecha)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'evento',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.evento, _data_old parque.evento, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: informacion_parque; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.informacion_parque (
    id integer NOT NULL,
    propety text NOT NULL,
    descripcion text NOT NULL,
    imagenes_url text NOT NULL,
    token text,
    last_modification timestamp without time zone
);


ALTER TABLE parque.informacion_parque OWNER TO postgres;

--
-- Name: TABLE informacion_parque; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.informacion_parque IS 'Tabla dedicada a almacenar la informacion del parque, tal como descripcion,ubicacion, reseña historica';


--
-- Name: field_audit(parque.informacion_parque, parque.informacion_parque, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.informacion_parque, _data_old parque.informacion_parque, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('propety_nuevo', _data_new.propety)::jsonb;
				_datos := _datos || json_build_object('descripcion_nuevo', _data_new.descripcion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('propety_anterior', _data_old.propety)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.propety <> _data_new.propety
				THEN _datos := _datos || json_build_object('propety_anterior', _data_old.propety, 'propety_nuevo', _data_new.propety)::jsonb;
			END IF;
			IF _data_old.descripcion <> _data_new.descripcion
				THEN _datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion, 'descripcion_nuevo', _data_new.descripcion)::jsonb;
			END IF;
			IF _data_old.imagenes_url <> _data_new.imagenes_url
				THEN _datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url, 'imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'informacion_parque',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.informacion_parque, _data_old parque.informacion_parque, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: noticia; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.noticia (
    id integer NOT NULL,
    titulo text NOT NULL,
    descripcion text NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    imagen_url text NOT NULL,
    comentarios_id text,
    calificacion double precision,
    token text,
    last_modification timestamp without time zone
);


ALTER TABLE parque.noticia OWNER TO postgres;

--
-- Name: field_audit(parque.noticia, parque.noticia, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.noticia, _data_old parque.noticia, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('titulo_nuevo', _data_new.titulo)::jsonb;
				_datos := _datos || json_build_object('descripcion_nuevo', _data_new.descripcion)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('imagen_url_nuevo', _data_new.imagen_url)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('titulo_anterior', _data_old.titulo)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('imagen_url_anterior', _data_old.imagen_url)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.titulo <> _data_new.titulo
				THEN _datos := _datos || json_build_object('titulo_anterior', _data_old.titulo, 'titulo_nuevo', _data_new.titulo)::jsonb;
			END IF;
			IF _data_old.descripcion <> _data_new.descripcion
				THEN _datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion, 'descripcion_nuevo', _data_new.descripcion)::jsonb;
			END IF;
			IF _data_old.fecha_publicacion <> _data_new.fecha_publicacion
				THEN _datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion, 'fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
			END IF;
			IF _data_old.imagen_url <> _data_new.imagen_url
				THEN _datos := _datos || json_build_object('imagen_url_anterior', _data_old.imagen_url, 'imagen_url_nuevo', _data_new.imagen_url)::jsonb;
			END IF;
			IF _data_old.comentarios_id <> _data_new.comentarios_id
				THEN _datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id, 'comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'noticia',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.noticia, _data_old parque.noticia, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: pictograma; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.pictograma (
    id integer NOT NULL,
    nombre text NOT NULL,
    calificacion double precision NOT NULL,
    imagenes_url text NOT NULL,
    descripcion text NOT NULL,
    comentarios_id text,
    token text,
    last_modification timestamp without time zone
);


ALTER TABLE parque.pictograma OWNER TO postgres;

--
-- Name: TABLE pictograma; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.pictograma IS 'Tabla dedicada a almacenar los datos de los pictogramas del parque arqueologico ';


--
-- Name: field_audit(parque.pictograma, parque.pictograma, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.pictograma, _data_old parque.pictograma, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('descripcion_nuevo', _data_new.descripcion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.imagenes_url <> _data_new.imagenes_url
				THEN _datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url, 'imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
			END IF;
			IF _data_old.descripcion <> _data_new.descripcion
				THEN _datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion, 'descripcion_nuevo', _data_new.descripcion)::jsonb;
			END IF;
			IF _data_old.comentarios_id <> _data_new.comentarios_id
				THEN _datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id, 'comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'pictograma',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.pictograma, _data_old parque.pictograma, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: pqr; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.pqr (
    id bigint NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    pregunta text NOT NULL,
    respuesta text,
    usuario_id integer NOT NULL,
    estado_id integer NOT NULL,
    token text,
    last_modification timestamp without time zone
);


ALTER TABLE parque.pqr OWNER TO postgres;

--
-- Name: TABLE pqr; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.pqr IS 'Tabla dedicada a almacenar las preguntas del sistema ';


--
-- Name: field_audit(parque.pqr, parque.pqr, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.pqr, _data_old parque.pqr, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('pregunta_nuevo', _data_new.pregunta)::jsonb;
				_datos := _datos || json_build_object('respuesta_nuevo', _data_new.respuesta)::jsonb;
				_datos := _datos || json_build_object('usuario_id_nuevo', _data_new.usuario_id)::jsonb;
				_datos := _datos || json_build_object('estado_id_nuevo', _data_new.estado_id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('pregunta_anterior', _data_old.pregunta)::jsonb;
				_datos := _datos || json_build_object('respuesta_anterior', _data_old.respuesta)::jsonb;
				_datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id)::jsonb;
				_datos := _datos || json_build_object('estado_id_anterior', _data_old.estado_id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.fecha_publicacion <> _data_new.fecha_publicacion
				THEN _datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion, 'fecha_publicacion_nuevo', _data_new.fecha_publicacion)::jsonb;
			END IF;
			IF _data_old.pregunta <> _data_new.pregunta
				THEN _datos := _datos || json_build_object('pregunta_anterior', _data_old.pregunta, 'pregunta_nuevo', _data_new.pregunta)::jsonb;
			END IF;
			IF _data_old.respuesta <> _data_new.respuesta
				THEN _datos := _datos || json_build_object('respuesta_anterior', _data_old.respuesta, 'respuesta_nuevo', _data_new.respuesta)::jsonb;
			END IF;
			IF _data_old.usuario_id <> _data_new.usuario_id
				THEN _datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id, 'usuario_id_nuevo', _data_new.usuario_id)::jsonb;
			END IF;
			IF _data_old.estado_id <> _data_new.estado_id
				THEN _datos := _datos || json_build_object('estado_id_anterior', _data_old.estado_id, 'estado_id_nuevo', _data_new.estado_id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'pqr',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.pqr, _data_old parque.pqr, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: reserva; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.reserva (
    id bigint NOT NULL,
    fecha_compra timestamp without time zone NOT NULL,
    precio double precision NOT NULL,
    usuario_id integer NOT NULL,
    estado_id integer NOT NULL,
    token text,
    last_modification timestamp without time zone
);


ALTER TABLE parque.reserva OWNER TO postgres;

--
-- Name: TABLE reserva; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.reserva IS 'Tabla dedicada a almacenar los datos de las reservas del sistema, esta sera heredada por otras tablas para independizar la información correspondiente ';


--
-- Name: field_audit(parque.reserva, parque.reserva, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.reserva, _data_old parque.reserva, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('fecha_compra_nuevo', _data_new.fecha_compra)::jsonb;
				_datos := _datos || json_build_object('precio_nuevo', _data_new.precio)::jsonb;
				_datos := _datos || json_build_object('usuario_id_nuevo', _data_new.usuario_id)::jsonb;
				_datos := _datos || json_build_object('estado_id_nuevo', _data_new.estado_id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('fecha_compra_anterior', _data_old.fecha_compra)::jsonb;
				_datos := _datos || json_build_object('precio_anterior', _data_old.precio)::jsonb;
				_datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id)::jsonb;
				_datos := _datos || json_build_object('estado_id_anterior', _data_old.estado_id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.fecha_compra <> _data_new.fecha_compra
				THEN _datos := _datos || json_build_object('fecha_compra_anterior', _data_old.fecha_compra, 'fecha_compra_nuevo', _data_new.fecha_compra)::jsonb;
			END IF;
			IF _data_old.precio <> _data_new.precio
				THEN _datos := _datos || json_build_object('precio_anterior', _data_old.precio, 'precio_nuevo', _data_new.precio)::jsonb;
			END IF;
			IF _data_old.usuario_id <> _data_new.usuario_id
				THEN _datos := _datos || json_build_object('usuario_id_anterior', _data_old.usuario_id, 'usuario_id_nuevo', _data_new.usuario_id)::jsonb;
			END IF;
			IF _data_old.estado_id <> _data_new.estado_id
				THEN _datos := _datos || json_build_object('estado_id_anterior', _data_old.estado_id, 'estado_id_nuevo', _data_new.estado_id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'reserva',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.reserva, _data_old parque.reserva, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: rol; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.rol (
    id integer NOT NULL,
    nombre text NOT NULL
);


ALTER TABLE parque.rol OWNER TO postgres;

--
-- Name: TABLE rol; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.rol IS 'Tabla dedicada a almacenar los roles correspondientes al sistema ';


--
-- Name: field_audit(parque.rol, parque.rol, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.rol, _data_old parque.rol, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'rol',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.rol, _data_old parque.rol, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: ticket; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.ticket (
    id integer NOT NULL,
    nombre text NOT NULL,
    precio double precision NOT NULL,
    token text,
    last_modification timestamp without time zone
);


ALTER TABLE parque.ticket OWNER TO postgres;

--
-- Name: TABLE ticket; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.ticket IS 'Tabla destinada a almacenar los datos de los tiquetes de los usuarios del sistema,profe si llega a ver esto pasenos';


--
-- Name: field_audit(parque.ticket, parque.ticket, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.ticket, _data_old parque.ticket, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				_datos := _datos || json_build_object('precio_nuevo', _data_new.precio)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('precio_anterior', _data_old.precio)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			IF _data_old.precio <> _data_new.precio
				THEN _datos := _datos || json_build_object('precio_anterior', _data_old.precio, 'precio_nuevo', _data_new.precio)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'ticket',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.ticket, _data_old parque.ticket, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: usuario; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.usuario (
    id integer NOT NULL,
    nombre text NOT NULL,
    apellido text NOT NULL,
    tipo_documento text NOT NULL,
    numero_documento double precision NOT NULL,
    lugar_expedicion text,
    correo_electronico text NOT NULL,
    clave text NOT NULL,
    icono_url text NOT NULL,
    verificacion_cuenta boolean NOT NULL,
    estado_cuenta boolean NOT NULL,
    rol_id integer NOT NULL,
    token text,
    last_modification timestamp without time zone
);


ALTER TABLE parque.usuario OWNER TO postgres;

--
-- Name: TABLE usuario; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.usuario IS 'Tabla dedicada a almacenar los datos correspondientes a los usuarios que se registren en el sistema';


--
-- Name: field_audit(parque.usuario, parque.usuario, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new parque.usuario, _data_old parque.usuario, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('nombre_nuevo', _data_new.nombre)::jsonb;
				_datos := _datos || json_build_object('apellido_nuevo', _data_new.apellido)::jsonb;
				_datos := _datos || json_build_object('tipo_documento_nuevo', _data_new.tipo_documento)::jsonb;
				_datos := _datos || json_build_object('numero_documento_nuevo', _data_new.numero_documento)::jsonb;
				_datos := _datos || json_build_object('lugar_expedicion_nuevo', _data_new.lugar_expedicion)::jsonb;
				_datos := _datos || json_build_object('correo_electronico_nuevo', _data_new.correo_electronico)::jsonb;
				_datos := _datos || json_build_object('clave_nuevo', _data_new.clave)::jsonb;
				_datos := _datos || json_build_object('icono_url_nuevo', _data_new.icono_url)::jsonb;
				_datos := _datos || json_build_object('verificacion_cuenta_nuevo', _data_new.verificacion_cuenta)::jsonb;
				_datos := _datos || json_build_object('estado_cuenta_nuevo', _data_new.estado_cuenta)::jsonb;
				_datos := _datos || json_build_object('rol_id_nuevo', _data_new.rol_id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_nuevo', _data_new.last_modification)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('apellido_anterior', _data_old.apellido)::jsonb;
				_datos := _datos || json_build_object('tipo_documento_anterior', _data_old.tipo_documento)::jsonb;
				_datos := _datos || json_build_object('numero_documento_anterior', _data_old.numero_documento)::jsonb;
				_datos := _datos || json_build_object('lugar_expedicion_anterior', _data_old.lugar_expedicion)::jsonb;
				_datos := _datos || json_build_object('correo_electronico_anterior', _data_old.correo_electronico)::jsonb;
				_datos := _datos || json_build_object('clave_anterior', _data_old.clave)::jsonb;
				_datos := _datos || json_build_object('icono_url_anterior', _data_old.icono_url)::jsonb;
				_datos := _datos || json_build_object('verificacion_cuenta_anterior', _data_old.verificacion_cuenta)::jsonb;
				_datos := _datos || json_build_object('estado_cuenta_anterior', _data_old.estado_cuenta)::jsonb;
				_datos := _datos || json_build_object('rol_id_anterior', _data_old.rol_id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.nombre <> _data_new.nombre
				THEN _datos := _datos || json_build_object('nombre_anterior', _data_old.nombre, 'nombre_nuevo', _data_new.nombre)::jsonb;
			END IF;
			IF _data_old.apellido <> _data_new.apellido
				THEN _datos := _datos || json_build_object('apellido_anterior', _data_old.apellido, 'apellido_nuevo', _data_new.apellido)::jsonb;
			END IF;
			IF _data_old.tipo_documento <> _data_new.tipo_documento
				THEN _datos := _datos || json_build_object('tipo_documento_anterior', _data_old.tipo_documento, 'tipo_documento_nuevo', _data_new.tipo_documento)::jsonb;
			END IF;
			IF _data_old.numero_documento <> _data_new.numero_documento
				THEN _datos := _datos || json_build_object('numero_documento_anterior', _data_old.numero_documento, 'numero_documento_nuevo', _data_new.numero_documento)::jsonb;
			END IF;
			IF _data_old.lugar_expedicion <> _data_new.lugar_expedicion
				THEN _datos := _datos || json_build_object('lugar_expedicion_anterior', _data_old.lugar_expedicion, 'lugar_expedicion_nuevo', _data_new.lugar_expedicion)::jsonb;
			END IF;
			IF _data_old.correo_electronico <> _data_new.correo_electronico
				THEN _datos := _datos || json_build_object('correo_electronico_anterior', _data_old.correo_electronico, 'correo_electronico_nuevo', _data_new.correo_electronico)::jsonb;
			END IF;
			IF _data_old.clave <> _data_new.clave
				THEN _datos := _datos || json_build_object('clave_anterior', _data_old.clave, 'clave_nuevo', _data_new.clave)::jsonb;
			END IF;
			IF _data_old.icono_url <> _data_new.icono_url
				THEN _datos := _datos || json_build_object('icono_url_anterior', _data_old.icono_url, 'icono_url_nuevo', _data_new.icono_url)::jsonb;
			END IF;
			IF _data_old.verificacion_cuenta <> _data_new.verificacion_cuenta
				THEN _datos := _datos || json_build_object('verificacion_cuenta_anterior', _data_old.verificacion_cuenta, 'verificacion_cuenta_nuevo', _data_new.verificacion_cuenta)::jsonb;
			END IF;
			IF _data_old.estado_cuenta <> _data_new.estado_cuenta
				THEN _datos := _datos || json_build_object('estado_cuenta_anterior', _data_old.estado_cuenta, 'estado_cuenta_nuevo', _data_new.estado_cuenta)::jsonb;
			END IF;
			IF _data_old.rol_id <> _data_new.rol_id
				THEN _datos := _datos || json_build_object('rol_id_anterior', _data_old.rol_id, 'rol_id_nuevo', _data_new.rol_id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.last_modification <> _data_new.last_modification
				THEN _datos := _datos || json_build_object('last_modification_anterior', _data_old.last_modification, 'last_modification_nuevo', _data_new.last_modification)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'parque',
			'usuario',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new parque.usuario, _data_old parque.usuario, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: tokens; Type: TABLE; Schema: security; Owner: postgres
--

CREATE TABLE security.tokens (
    id integer NOT NULL,
    token text NOT NULL,
    fecha_generacion timestamp without time zone NOT NULL,
    fecha_vencimiento timestamp without time zone NOT NULL,
    aplicacion_id integer NOT NULL,
    user_id integer NOT NULL
);


ALTER TABLE security.tokens OWNER TO postgres;

--
-- Name: field_audit(security.tokens, security.tokens, character varying, text, character varying, text, text); Type: FUNCTION; Schema: security; Owner: postgres
--

CREATE FUNCTION security.field_audit(_data_new security.tokens, _data_old security.tokens, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
    LANGUAGE plpgsql
    AS $$

	DECLARE
		_column_data TEXT;
	 	_datos jsonb;
	 	
	BEGIN
			_datos = '{}';

		IF _accion = 'INSERT'
			THEN
				_datos := _datos || json_build_object('id_nuevo', _data_new.id)::jsonb;
				_datos := _datos || json_build_object('token_nuevo', _data_new.token)::jsonb;
				_datos := _datos || json_build_object('fecha_generacion_nuevo', _data_new.fecha_generacion)::jsonb;
				_datos := _datos || json_build_object('fecha_vencimiento_nuevo', _data_new.fecha_vencimiento)::jsonb;
				_datos := _datos || json_build_object('aplicacion_id_nuevo', _data_new.aplicacion_id)::jsonb;
				_datos := _datos || json_build_object('user_id_nuevo', _data_new.user_id)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('token_anterior', _data_old.token)::jsonb;
				_datos := _datos || json_build_object('fecha_generacion_anterior', _data_old.fecha_generacion)::jsonb;
				_datos := _datos || json_build_object('fecha_vencimiento_anterior', _data_old.fecha_vencimiento)::jsonb;
				_datos := _datos || json_build_object('aplicacion_id_anterior', _data_old.aplicacion_id)::jsonb;
				_datos := _datos || json_build_object('user_id_anterior', _data_old.user_id)::jsonb;
				
		ELSE
			IF _data_old.id <> _data_new.id
				THEN _datos := _datos || json_build_object('id_anterior', _data_old.id, 'id_nuevo', _data_new.id)::jsonb;
			END IF;
			IF _data_old.token <> _data_new.token
				THEN _datos := _datos || json_build_object('token_anterior', _data_old.token, 'token_nuevo', _data_new.token)::jsonb;
			END IF;
			IF _data_old.fecha_generacion <> _data_new.fecha_generacion
				THEN _datos := _datos || json_build_object('fecha_generacion_anterior', _data_old.fecha_generacion, 'fecha_generacion_nuevo', _data_new.fecha_generacion)::jsonb;
			END IF;
			IF _data_old.fecha_vencimiento <> _data_new.fecha_vencimiento
				THEN _datos := _datos || json_build_object('fecha_vencimiento_anterior', _data_old.fecha_vencimiento, 'fecha_vencimiento_nuevo', _data_new.fecha_vencimiento)::jsonb;
			END IF;
			IF _data_old.aplicacion_id <> _data_new.aplicacion_id
				THEN _datos := _datos || json_build_object('aplicacion_id_anterior', _data_old.aplicacion_id, 'aplicacion_id_nuevo', _data_new.aplicacion_id)::jsonb;
			END IF;
			IF _data_old.user_id <> _data_new.user_id
				THEN _datos := _datos || json_build_object('user_id_anterior', _data_old.user_id, 'user_id_nuevo', _data_new.user_id)::jsonb;
			END IF;
			 
		END IF;

		INSERT INTO security.auditoria
		(
			fecha,
			accion,
			schema,
			tabla,
			pk,
			token,
			user_bd,
			data
		)
		VALUES
		(
			CURRENT_TIMESTAMP,
			_accion,
			'security',
			'tokens',
			_table_pk,
			_token,
			_user_db,
			_datos::jsonb
			);

		RETURN NULL; 
	END;$$;


ALTER FUNCTION security.field_audit(_data_new security.tokens, _data_old security.tokens, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) OWNER TO postgres;

--
-- Name: aplicacion; Type: TABLE; Schema: parametrizacion; Owner: postgres
--

CREATE TABLE parametrizacion.aplicacion (
    id integer NOT NULL,
    nombre text
);


ALTER TABLE parametrizacion.aplicacion OWNER TO postgres;

--
-- Name: cabana_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.cabana_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.cabana_id_seq OWNER TO postgres;

--
-- Name: cabana_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.cabana_id_seq OWNED BY parque.cabana.id;


--
-- Name: comentario_cabana; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.comentario_cabana (
    cabana_id integer NOT NULL
)
INHERITS (parque.comentarios);


ALTER TABLE parque.comentario_cabana OWNER TO postgres;

--
-- Name: comentario_evento; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.comentario_evento (
    evento_id integer NOT NULL
)
INHERITS (parque.comentarios);


ALTER TABLE parque.comentario_evento OWNER TO postgres;

--
-- Name: comentario_noticia; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.comentario_noticia (
    noticia_id integer NOT NULL
)
INHERITS (parque.comentarios);


ALTER TABLE parque.comentario_noticia OWNER TO postgres;

--
-- Name: comentario_pictograma; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.comentario_pictograma (
    pictograma_id integer NOT NULL
)
INHERITS (parque.comentarios);


ALTER TABLE parque.comentario_pictograma OWNER TO postgres;

--
-- Name: comentarios_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.comentarios_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.comentarios_id_seq OWNER TO postgres;

--
-- Name: comentarios_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.comentarios_id_seq OWNED BY parque.comentarios.id;


--
-- Name: estado_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.estado_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.estado_id_seq OWNER TO postgres;

--
-- Name: estado_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.estado_id_seq OWNED BY parque.estado_reserva.id;


--
-- Name: evento_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.evento_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.evento_id_seq OWNER TO postgres;

--
-- Name: evento_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.evento_id_seq OWNED BY parque.evento.id;


--
-- Name: informacion_parque_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.informacion_parque_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.informacion_parque_id_seq OWNER TO postgres;

--
-- Name: informacion_parque_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.informacion_parque_id_seq OWNED BY parque.informacion_parque.id;


--
-- Name: noticia_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.noticia_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.noticia_id_seq OWNER TO postgres;

--
-- Name: noticia_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.noticia_id_seq OWNED BY parque.noticia.id;


--
-- Name: pictograma_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.pictograma_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.pictograma_id_seq OWNER TO postgres;

--
-- Name: pictograma_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.pictograma_id_seq OWNED BY parque.pictograma.id;


--
-- Name: pqr_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.pqr_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.pqr_id_seq OWNER TO postgres;

--
-- Name: pqr_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.pqr_id_seq OWNED BY parque.pqr.id;


--
-- Name: reserva_cabana; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.reserva_cabana (
    ticket_id integer NOT NULL
)
INHERITS (parque.reserva);


ALTER TABLE parque.reserva_cabana OWNER TO postgres;

--
-- Name: reserva_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.reserva_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.reserva_id_seq OWNER TO postgres;

--
-- Name: reserva_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.reserva_id_seq OWNED BY parque.reserva.id;


--
-- Name: reserva_ticket; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.reserva_ticket (
    ticket_id bigint NOT NULL,
    qr text NOT NULL,
    cantidad double precision NOT NULL
)
INHERITS (parque.reserva);


ALTER TABLE parque.reserva_ticket OWNER TO postgres;

--
-- Name: TABLE reserva_ticket; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.reserva_ticket IS 'Tabla dedicada a almacenar los datos de los tiquetes de los usuarios heredada de la tabla reserva ';


--
-- Name: rol_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.rol_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.rol_id_seq OWNER TO postgres;

--
-- Name: rol_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.rol_id_seq OWNED BY parque.rol.id;


--
-- Name: ticket_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.ticket_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.ticket_id_seq OWNER TO postgres;

--
-- Name: ticket_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.ticket_id_seq OWNED BY parque.ticket.id;


--
-- Name: usuario_id_seq; Type: SEQUENCE; Schema: parque; Owner: postgres
--

CREATE SEQUENCE parque.usuario_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE parque.usuario_id_seq OWNER TO postgres;

--
-- Name: usuario_id_seq; Type: SEQUENCE OWNED BY; Schema: parque; Owner: postgres
--

ALTER SEQUENCE parque.usuario_id_seq OWNED BY parque.usuario.id;


--
-- Name: auditoria; Type: TABLE; Schema: security; Owner: postgres
--

CREATE TABLE security.auditoria (
    id bigint NOT NULL,
    fecha timestamp without time zone NOT NULL,
    accion character varying(100),
    schema character varying(200) NOT NULL,
    tabla character varying(200),
    token text NOT NULL,
    user_bd character varying(100) NOT NULL,
    data jsonb NOT NULL,
    pk text NOT NULL
);


ALTER TABLE security.auditoria OWNER TO postgres;

--
-- Name: auditoria_id_seq; Type: SEQUENCE; Schema: security; Owner: postgres
--

CREATE SEQUENCE security.auditoria_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE security.auditoria_id_seq OWNER TO postgres;

--
-- Name: auditoria_id_seq; Type: SEQUENCE OWNED BY; Schema: security; Owner: postgres
--

ALTER SEQUENCE security.auditoria_id_seq OWNED BY security.auditoria.id;


--
-- Name: function_db_view; Type: VIEW; Schema: security; Owner: postgres
--

CREATE VIEW security.function_db_view AS
 SELECT pp.proname AS b_function,
    oidvectortypes(pp.proargtypes) AS b_type_parameters
   FROM (pg_proc pp
     JOIN pg_namespace pn ON ((pn.oid = pp.pronamespace)))
  WHERE ((pn.nspname)::text <> ALL (ARRAY[('pg_catalog'::character varying)::text, ('information_schema'::character varying)::text, ('admin_control'::character varying)::text, ('vial'::character varying)::text]));


ALTER TABLE security.function_db_view OWNER TO postgres;

--
-- Name: token_id_seq; Type: SEQUENCE; Schema: security; Owner: postgres
--

CREATE SEQUENCE security.token_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE security.token_id_seq OWNER TO postgres;

--
-- Name: token_id_seq; Type: SEQUENCE OWNED BY; Schema: security; Owner: postgres
--

ALTER SEQUENCE security.token_id_seq OWNED BY security.tokens.id;


--
-- Name: cabana id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.cabana ALTER COLUMN id SET DEFAULT nextval('parque.cabana_id_seq'::regclass);


--
-- Name: comentario_cabana id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.comentario_cabana ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);


--
-- Name: comentario_evento id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.comentario_evento ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);


--
-- Name: comentario_noticia id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.comentario_noticia ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);


--
-- Name: comentario_pictograma id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.comentario_pictograma ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);


--
-- Name: comentarios id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.comentarios ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);


--
-- Name: estado_reserva id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.estado_reserva ALTER COLUMN id SET DEFAULT nextval('parque.estado_id_seq'::regclass);


--
-- Name: evento id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.evento ALTER COLUMN id SET DEFAULT nextval('parque.evento_id_seq'::regclass);


--
-- Name: informacion_parque id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.informacion_parque ALTER COLUMN id SET DEFAULT nextval('parque.informacion_parque_id_seq'::regclass);


--
-- Name: noticia id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.noticia ALTER COLUMN id SET DEFAULT nextval('parque.noticia_id_seq'::regclass);


--
-- Name: pictograma id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.pictograma ALTER COLUMN id SET DEFAULT nextval('parque.pictograma_id_seq'::regclass);


--
-- Name: pqr id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.pqr ALTER COLUMN id SET DEFAULT nextval('parque.pqr_id_seq'::regclass);


--
-- Name: reserva id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.reserva ALTER COLUMN id SET DEFAULT nextval('parque.reserva_id_seq'::regclass);


--
-- Name: reserva_cabana id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.reserva_cabana ALTER COLUMN id SET DEFAULT nextval('parque.reserva_id_seq'::regclass);


--
-- Name: reserva_ticket id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.reserva_ticket ALTER COLUMN id SET DEFAULT nextval('parque.reserva_id_seq'::regclass);


--
-- Name: rol id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.rol ALTER COLUMN id SET DEFAULT nextval('parque.rol_id_seq'::regclass);


--
-- Name: ticket id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.ticket ALTER COLUMN id SET DEFAULT nextval('parque.ticket_id_seq'::regclass);


--
-- Name: usuario id; Type: DEFAULT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.usuario ALTER COLUMN id SET DEFAULT nextval('parque.usuario_id_seq'::regclass);


--
-- Name: auditoria id; Type: DEFAULT; Schema: security; Owner: postgres
--

ALTER TABLE ONLY security.auditoria ALTER COLUMN id SET DEFAULT nextval('security.auditoria_id_seq'::regclass);


--
-- Name: tokens id; Type: DEFAULT; Schema: security; Owner: postgres
--

ALTER TABLE ONLY security.tokens ALTER COLUMN id SET DEFAULT nextval('security.token_id_seq'::regclass);


--
-- Data for Name: aplicacion; Type: TABLE DATA; Schema: parametrizacion; Owner: postgres
--

COPY parametrizacion.aplicacion (id, nombre) FROM stdin;
\.
COPY parametrizacion.aplicacion (id, nombre) FROM '$$PATH$$/3033.dat';

--
-- Data for Name: cabana; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.cabana (id, nombre, capacidad, precio, imagenes_url, calificacion, comentarios_id, token, last_modification) FROM stdin;
\.
COPY parque.cabana (id, nombre, capacidad, precio, imagenes_url, calificacion, comentarios_id, token, last_modification) FROM '$$PATH$$/3019.dat';

--
-- Data for Name: comentario_cabana; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.comentario_cabana (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, cabana_id) FROM stdin;
\.
COPY parque.comentario_cabana (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, cabana_id) FROM '$$PATH$$/3035.dat';

--
-- Data for Name: comentario_evento; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.comentario_evento (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, evento_id) FROM stdin;
\.
COPY parque.comentario_evento (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, evento_id) FROM '$$PATH$$/3036.dat';

--
-- Data for Name: comentario_noticia; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.comentario_noticia (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, noticia_id) FROM stdin;
\.
COPY parque.comentario_noticia (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, noticia_id) FROM '$$PATH$$/3037.dat';

--
-- Data for Name: comentario_pictograma; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.comentario_pictograma (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, pictograma_id) FROM stdin;
\.
COPY parque.comentario_pictograma (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token, pictograma_id) FROM '$$PATH$$/3038.dat';

--
-- Data for Name: comentarios; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.comentarios (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token) FROM stdin;
\.
COPY parque.comentarios (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, last_modification, token) FROM '$$PATH$$/3020.dat';

--
-- Data for Name: estado_pqr; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.estado_pqr (id, nombre) FROM stdin;
\.
COPY parque.estado_pqr (id, nombre) FROM '$$PATH$$/3021.dat';

--
-- Data for Name: estado_reserva; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.estado_reserva (id, nombre) FROM stdin;
\.
COPY parque.estado_reserva (id, nombre) FROM '$$PATH$$/3022.dat';

--
-- Data for Name: evento; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.evento (id, nombre, fecha_publicacion, descripcion, imagenes_url, comentarios_id, calificacion, token, last_modification, fecha) FROM stdin;
\.
COPY parque.evento (id, nombre, fecha_publicacion, descripcion, imagenes_url, comentarios_id, calificacion, token, last_modification, fecha) FROM '$$PATH$$/3023.dat';

--
-- Data for Name: informacion_parque; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.informacion_parque (id, propety, descripcion, imagenes_url, token, last_modification) FROM stdin;
\.
COPY parque.informacion_parque (id, propety, descripcion, imagenes_url, token, last_modification) FROM '$$PATH$$/3024.dat';

--
-- Data for Name: noticia; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.noticia (id, titulo, descripcion, fecha_publicacion, imagen_url, comentarios_id, calificacion, token, last_modification) FROM stdin;
\.
COPY parque.noticia (id, titulo, descripcion, fecha_publicacion, imagen_url, comentarios_id, calificacion, token, last_modification) FROM '$$PATH$$/3025.dat';

--
-- Data for Name: pictograma; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.pictograma (id, nombre, calificacion, imagenes_url, descripcion, comentarios_id, token, last_modification) FROM stdin;
\.
COPY parque.pictograma (id, nombre, calificacion, imagenes_url, descripcion, comentarios_id, token, last_modification) FROM '$$PATH$$/3026.dat';

--
-- Data for Name: pqr; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.pqr (id, fecha_publicacion, pregunta, respuesta, usuario_id, estado_id, token, last_modification) FROM stdin;
\.
COPY parque.pqr (id, fecha_publicacion, pregunta, respuesta, usuario_id, estado_id, token, last_modification) FROM '$$PATH$$/3027.dat';

--
-- Data for Name: reserva; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.reserva (id, fecha_compra, precio, usuario_id, estado_id, token, last_modification) FROM stdin;
\.
COPY parque.reserva (id, fecha_compra, precio, usuario_id, estado_id, token, last_modification) FROM '$$PATH$$/3028.dat';

--
-- Data for Name: reserva_cabana; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.reserva_cabana (id, fecha_compra, precio, usuario_id, estado_id, token, last_modification, ticket_id) FROM stdin;
\.
COPY parque.reserva_cabana (id, fecha_compra, precio, usuario_id, estado_id, token, last_modification, ticket_id) FROM '$$PATH$$/3046.dat';

--
-- Data for Name: reserva_ticket; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.reserva_ticket (id, fecha_compra, precio, usuario_id, estado_id, token, last_modification, ticket_id, qr, cantidad) FROM stdin;
\.
COPY parque.reserva_ticket (id, fecha_compra, precio, usuario_id, estado_id, token, last_modification, ticket_id, qr, cantidad) FROM '$$PATH$$/3048.dat';

--
-- Data for Name: rol; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.rol (id, nombre) FROM stdin;
\.
COPY parque.rol (id, nombre) FROM '$$PATH$$/3029.dat';

--
-- Data for Name: ticket; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.ticket (id, nombre, precio, token, last_modification) FROM stdin;
\.
COPY parque.ticket (id, nombre, precio, token, last_modification) FROM '$$PATH$$/3030.dat';

--
-- Data for Name: usuario; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.usuario (id, nombre, apellido, tipo_documento, numero_documento, lugar_expedicion, correo_electronico, clave, icono_url, verificacion_cuenta, estado_cuenta, rol_id, token, last_modification) FROM stdin;
\.
COPY parque.usuario (id, nombre, apellido, tipo_documento, numero_documento, lugar_expedicion, correo_electronico, clave, icono_url, verificacion_cuenta, estado_cuenta, rol_id, token, last_modification) FROM '$$PATH$$/3031.dat';

--
-- Data for Name: auditoria; Type: TABLE DATA; Schema: security; Owner: postgres
--

COPY security.auditoria (id, fecha, accion, schema, tabla, token, user_bd, data, pk) FROM stdin;
\.
COPY security.auditoria (id, fecha, accion, schema, tabla, token, user_bd, data, pk) FROM '$$PATH$$/3052.dat';

--
-- Data for Name: tokens; Type: TABLE DATA; Schema: security; Owner: postgres
--

COPY security.tokens (id, token, fecha_generacion, fecha_vencimiento, aplicacion_id, user_id) FROM stdin;
\.
COPY security.tokens (id, token, fecha_generacion, fecha_vencimiento, aplicacion_id, user_id) FROM '$$PATH$$/3032.dat';

--
-- Name: cabana_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.cabana_id_seq', 26, true);


--
-- Name: comentarios_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.comentarios_id_seq', 6, true);


--
-- Name: estado_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.estado_id_seq', 1, true);


--
-- Name: evento_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.evento_id_seq', 8, true);


--
-- Name: informacion_parque_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.informacion_parque_id_seq', 1, true);


--
-- Name: noticia_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.noticia_id_seq', 7, true);


--
-- Name: pictograma_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.pictograma_id_seq', 1, true);


--
-- Name: pqr_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.pqr_id_seq', 1, true);


--
-- Name: reserva_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.reserva_id_seq', 1, true);


--
-- Name: rol_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.rol_id_seq', 2, true);


--
-- Name: ticket_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.ticket_id_seq', 1, true);


--
-- Name: usuario_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.usuario_id_seq', 7, true);


--
-- Name: auditoria_id_seq; Type: SEQUENCE SET; Schema: security; Owner: postgres
--

SELECT pg_catalog.setval('security.auditoria_id_seq', 54, true);


--
-- Name: token_id_seq; Type: SEQUENCE SET; Schema: security; Owner: postgres
--

SELECT pg_catalog.setval('security.token_id_seq', 4, true);


--
-- Name: aplicacion aplicacion_pkey; Type: CONSTRAINT; Schema: parametrizacion; Owner: postgres
--

ALTER TABLE ONLY parametrizacion.aplicacion
    ADD CONSTRAINT aplicacion_pkey PRIMARY KEY (id);


--
-- Name: pictograma pictograma_pkey; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.pictograma
    ADD CONSTRAINT pictograma_pkey PRIMARY KEY (id);


--
-- Name: cabana pk_cabana; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.cabana
    ADD CONSTRAINT pk_cabana PRIMARY KEY (id);


--
-- Name: comentarios pk_comentarios; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.comentarios
    ADD CONSTRAINT pk_comentarios PRIMARY KEY (id);


--
-- Name: estado_reserva pk_estado; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.estado_reserva
    ADD CONSTRAINT pk_estado PRIMARY KEY (id);


--
-- Name: estado_pqr pk_estado_pqr; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.estado_pqr
    ADD CONSTRAINT pk_estado_pqr PRIMARY KEY (id);


--
-- Name: evento pk_evento; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.evento
    ADD CONSTRAINT pk_evento PRIMARY KEY (id);


--
-- Name: informacion_parque pk_informacion_parque; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.informacion_parque
    ADD CONSTRAINT pk_informacion_parque PRIMARY KEY (id);


--
-- Name: noticia pk_noticia; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.noticia
    ADD CONSTRAINT pk_noticia PRIMARY KEY (id);


--
-- Name: pqr pk_pqr; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.pqr
    ADD CONSTRAINT pk_pqr PRIMARY KEY (id);


--
-- Name: reserva pk_reserva; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.reserva
    ADD CONSTRAINT pk_reserva PRIMARY KEY (id);


--
-- Name: rol pk_rol; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.rol
    ADD CONSTRAINT pk_rol PRIMARY KEY (id);


--
-- Name: ticket pk_ticket; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.ticket
    ADD CONSTRAINT pk_ticket PRIMARY KEY (id);


--
-- Name: usuario pk_usuario; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.usuario
    ADD CONSTRAINT pk_usuario PRIMARY KEY (id);


--
-- Name: auditoria auditoria_pkey; Type: CONSTRAINT; Schema: security; Owner: postgres
--

ALTER TABLE ONLY security.auditoria
    ADD CONSTRAINT auditoria_pkey PRIMARY KEY (id);


--
-- Name: tokens pk_tokens; Type: CONSTRAINT; Schema: security; Owner: postgres
--

ALTER TABLE ONLY security.tokens
    ADD CONSTRAINT pk_tokens PRIMARY KEY (id);


--
-- Name: aplicacion tg_parametrizacion_tokens; Type: TRIGGER; Schema: parametrizacion; Owner: postgres
--

CREATE TRIGGER tg_parametrizacion_tokens AFTER INSERT OR DELETE OR UPDATE ON parametrizacion.aplicacion FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: cabana tg_parque_cabana; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_cabana AFTER INSERT OR DELETE OR UPDATE ON parque.cabana FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: comentario_cabana tg_parque_comentario_cabana; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_comentario_cabana AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_cabana FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: comentario_evento tg_parque_comentario_evento; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_comentario_evento AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_evento FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: comentario_noticia tg_parque_comentario_noticia; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_comentario_noticia AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_noticia FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: comentario_pictograma tg_parque_comentario_pictograma; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_comentario_pictograma AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_pictograma FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: comentarios tg_parque_comentarios; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_comentarios AFTER INSERT OR DELETE OR UPDATE ON parque.comentarios FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: estado_pqr tg_parque_estado_pqr; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_estado_pqr AFTER INSERT OR DELETE OR UPDATE ON parque.estado_pqr FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: estado_reserva tg_parque_estado_reserva; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_estado_reserva AFTER INSERT OR DELETE OR UPDATE ON parque.estado_reserva FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: evento tg_parque_evento; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_evento AFTER INSERT OR DELETE OR UPDATE ON parque.evento FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: informacion_parque tg_parque_informacion_parque; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_informacion_parque AFTER INSERT OR DELETE OR UPDATE ON parque.informacion_parque FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: noticia tg_parque_noticia; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_noticia AFTER INSERT OR DELETE OR UPDATE ON parque.noticia FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: pictograma tg_parque_pictograma; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_pictograma AFTER INSERT OR DELETE OR UPDATE ON parque.pictograma FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: pqr tg_parque_pqr; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_pqr AFTER INSERT OR DELETE OR UPDATE ON parque.pqr FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: reserva tg_parque_reserva; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_reserva AFTER INSERT OR DELETE OR UPDATE ON parque.reserva FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: reserva_cabana tg_parque_reserva_cabana; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_reserva_cabana AFTER INSERT OR DELETE OR UPDATE ON parque.reserva_cabana FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: reserva_ticket tg_parque_reserva_ticket; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_reserva_ticket AFTER INSERT OR DELETE OR UPDATE ON parque.reserva_ticket FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: rol tg_parque_rol; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_rol AFTER INSERT OR DELETE OR UPDATE ON parque.rol FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: ticket tg_parque_ticket; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_ticket AFTER INSERT OR DELETE OR UPDATE ON parque.ticket FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: usuario tg_parque_usuario; Type: TRIGGER; Schema: parque; Owner: postgres
--

CREATE TRIGGER tg_parque_usuario AFTER INSERT OR DELETE OR UPDATE ON parque.usuario FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- Name: tokens tg_parametrizacion_tokens; Type: TRIGGER; Schema: security; Owner: postgres
--

CREATE TRIGGER tg_parametrizacion_tokens AFTER INSERT OR DELETE OR UPDATE ON security.tokens FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();


--
-- PostgreSQL database dump complete
--

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                      