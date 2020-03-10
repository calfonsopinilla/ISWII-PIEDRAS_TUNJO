toc.dat                                                                                             0000600 0004000 0002000 00000162040 13632002770 0014442 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        PGDMP                   
        x            piedras    11.5    11.5 �    �           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                       false         �           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                       false         �           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                       false         �           1262    87756    piedras    DATABASE     �   CREATE DATABASE piedras WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'Spanish_Spain.1252' LC_CTYPE = 'Spanish_Spain.1252';
    DROP DATABASE piedras;
             postgres    false                     2615    87757    parque    SCHEMA        CREATE SCHEMA parque;
    DROP SCHEMA parque;
             postgres    false         �           0    0    SCHEMA parque    COMMENT     �   COMMENT ON SCHEMA parque IS 'Esquema dedicado a contener todas las tablas,triggets, procedimientos almacenados, jobs correspondientes a todas las tablas dedicadas a lo directamente relacionado con el parque';
                  postgres    false    6                     2615    87758    security    SCHEMA        CREATE SCHEMA security;
    DROP SCHEMA security;
             postgres    false         �           0    0    SCHEMA security    COMMENT     W   COMMENT ON SCHEMA security IS 'Esquema dedicado a almacenar la seguridad del sistema';
                  postgres    false    8         �            1255    87759    f_log_auditoria()    FUNCTION     }  CREATE FUNCTION security.f_log_auditoria() RETURNS trigger
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
			_token := _new_data.session;
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
       security       postgres    false    8         �            1259    87761    cabana    TABLE       CREATE TABLE parque.cabana (
    id integer NOT NULL,
    nombre text NOT NULL,
    capacidad integer NOT NULL,
    precio double precision NOT NULL,
    imagenes_url text NOT NULL,
    calificacion double precision NOT NULL,
    comentarios_id text NOT NULL
);
    DROP TABLE parque.cabana;
       parque         postgres    false    6         �           0    0    TABLE cabana    COMMENT     �   COMMENT ON TABLE parque.cabana IS 'Tabla dedicada a almacenar los datos de las cabañas del parque arqueológico de facatativa.';
            parque       postgres    false    198         �            1255    87767 a   field_audit(parque.cabana, parque.cabana, character varying, text, character varying, text, text)    FUNCTION       CREATE FUNCTION security.field_audit(_data_new parque.cabana, _data_old parque.cabana, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
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
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('capacidad_anterior', _data_old.capacidad)::jsonb;
				_datos := _datos || json_build_object('precio_anterior', _data_old.precio)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				
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
       security       postgres    false    198    198    8         �            1259    87816    evento    TABLE     0  CREATE TABLE parque.evento (
    id integer NOT NULL,
    nombre text NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    descripcion text NOT NULL,
    comentarios_id text NOT NULL,
    calificacion double precision,
    fecha timestamp without time zone,
    imagenes_url text
);
    DROP TABLE parque.evento;
       parque         postgres    false    6         �           0    0    TABLE evento    COMMENT        COMMENT ON TABLE parque.evento IS 'Tabla dedicada a almacenar los datos de los eventos del parque arqueologico de facatativa';
            parque       postgres    false    209         �            1255    87971 a   field_audit(parque.evento, parque.evento, character varying, text, character varying, text, text)    FUNCTION     �  CREATE FUNCTION security.field_audit(_data_new parque.evento, _data_old parque.evento, _accion character varying, _token text, _user_db character varying, _table_pk text, _init text) RETURNS text
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
				_datos := _datos || json_build_object('comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('fecha_nuevo', _data_new.fecha)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('fecha_anterior', _data_old.fecha)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				
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
			IF _data_old.comentarios_id <> _data_new.comentarios_id
				THEN _datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id, 'comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.fecha <> _data_new.fecha
				THEN _datos := _datos || json_build_object('fecha_anterior', _data_old.fecha, 'fecha_nuevo', _data_new.fecha)::jsonb;
			END IF;
			IF _data_old.imagenes_url <> _data_new.imagenes_url
				THEN _datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url, 'imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
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
       security       postgres    false    209    8    209         �            1259    87768    cabana_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.cabana_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE parque.cabana_id_seq;
       parque       postgres    false    6    198         �           0    0    cabana_id_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE parque.cabana_id_seq OWNED BY parque.cabana.id;
            parque       postgres    false    199         �            1259    87770    comentarios    TABLE       CREATE TABLE parque.comentarios (
    id bigint NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    descripcion text NOT NULL,
    calificacion double precision NOT NULL,
    reportado boolean NOT NULL,
    usuario_id integer NOT NULL
);
    DROP TABLE parque.comentarios;
       parque         postgres    false    6         �           0    0    TABLE comentarios    COMMENT     �   COMMENT ON TABLE parque.comentarios IS 'tabla dedicada a almacenar los datos de los comentarios del sistema,tanto como de las noticias,eventos y pictogramas del parque arqueologico';
            parque       postgres    false    200         �            1259    87776    comentario_cabana    TABLE     h   CREATE TABLE parque.comentario_cabana (
    cabana_id integer NOT NULL
)
INHERITS (parque.comentarios);
 %   DROP TABLE parque.comentario_cabana;
       parque         postgres    false    200    6         �            1259    87782    comentario_evento    TABLE     h   CREATE TABLE parque.comentario_evento (
    evento_id integer NOT NULL
)
INHERITS (parque.comentarios);
 %   DROP TABLE parque.comentario_evento;
       parque         postgres    false    200    6         �            1259    87788    comentario_noticia    TABLE     j   CREATE TABLE parque.comentario_noticia (
    noticia_id integer NOT NULL
)
INHERITS (parque.comentarios);
 &   DROP TABLE parque.comentario_noticia;
       parque         postgres    false    200    6         �            1259    87794    comentario_pictograma    TABLE     p   CREATE TABLE parque.comentario_pictograma (
    pictograma_id integer NOT NULL
)
INHERITS (parque.comentarios);
 )   DROP TABLE parque.comentario_pictograma;
       parque         postgres    false    200    6         �            1259    87800    comentarios_id_seq    SEQUENCE     {   CREATE SEQUENCE parque.comentarios_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE parque.comentarios_id_seq;
       parque       postgres    false    200    6         �           0    0    comentarios_id_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE parque.comentarios_id_seq OWNED BY parque.comentarios.id;
            parque       postgres    false    205         �            1259    87802    estado_reserva    TABLE     Z   CREATE TABLE parque.estado_reserva (
    id integer NOT NULL,
    nombre text NOT NULL
);
 "   DROP TABLE parque.estado_reserva;
       parque         postgres    false    6         �           0    0    TABLE estado_reserva    COMMENT     p   COMMENT ON TABLE parque.estado_reserva IS 'Tabla dedicada a almacenar los estados de los procesos del sistema';
            parque       postgres    false    206         �            1259    87808    estado_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.estado_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE parque.estado_id_seq;
       parque       postgres    false    206    6         �           0    0    estado_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE parque.estado_id_seq OWNED BY parque.estado_reserva.id;
            parque       postgres    false    207         �            1259    87810 
   estado_pqr    TABLE     Y   CREATE TABLE parque.estado_pqr (
    "id " integer NOT NULL,
    nombre text NOT NULL
);
    DROP TABLE parque.estado_pqr;
       parque         postgres    false    6         �            1259    87822    evento_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.evento_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE parque.evento_id_seq;
       parque       postgres    false    6    209         �           0    0    evento_id_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE parque.evento_id_seq OWNED BY parque.evento.id;
            parque       postgres    false    210         �            1259    87824    informacion_parque    TABLE     �   CREATE TABLE parque.informacion_parque (
    id integer NOT NULL,
    propety text NOT NULL,
    descripcion text NOT NULL,
    imagenes_url text NOT NULL
);
 &   DROP TABLE parque.informacion_parque;
       parque         postgres    false    6         �           0    0    TABLE informacion_parque    COMMENT     �   COMMENT ON TABLE parque.informacion_parque IS 'Tabla dedicada a almacenar la informacion del parque, tal como descripcion,ubicacion, reseña historica';
            parque       postgres    false    211         �            1259    87830    informacion_parque_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.informacion_parque_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 0   DROP SEQUENCE parque.informacion_parque_id_seq;
       parque       postgres    false    6    211         �           0    0    informacion_parque_id_seq    SEQUENCE OWNED BY     W   ALTER SEQUENCE parque.informacion_parque_id_seq OWNED BY parque.informacion_parque.id;
            parque       postgres    false    212         �            1259    87832    noticia    TABLE     (  CREATE TABLE parque.noticia (
    id integer NOT NULL,
    titulo double precision NOT NULL,
    descripcion text NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    imagenes_url text NOT NULL,
    comentarios_id text NOT NULL,
    calificacion double precision NOT NULL
);
    DROP TABLE parque.noticia;
       parque         postgres    false    6         �           0    0    TABLE noticia    COMMENT     g   COMMENT ON TABLE parque.noticia IS 'Tabla dedicada a almacenar los datos de las noticias del sistema';
            parque       postgres    false    213         �            1259    87838 
   pictograma    TABLE     �   CREATE TABLE parque.pictograma (
    id integer NOT NULL,
    nombre text NOT NULL,
    calificacion double precision NOT NULL,
    imagenes_url text NOT NULL,
    descripcion text NOT NULL,
    comentarios_id text
);
    DROP TABLE parque.pictograma;
       parque         postgres    false    6         �           0    0    TABLE pictograma    COMMENT     z   COMMENT ON TABLE parque.pictograma IS 'Tabla dedicada a almacenar los datos de los pictogramas del parque arqueologico ';
            parque       postgres    false    214         �            1259    87844    pictograma_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.pictograma_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE parque.pictograma_id_seq;
       parque       postgres    false    214    6         �           0    0    pictograma_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE parque.pictograma_id_seq OWNED BY parque.pictograma.id;
            parque       postgres    false    215         �            1259    87846    pqr    TABLE     �   CREATE TABLE parque.pqr (
    id bigint NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    pregunta text NOT NULL,
    respuesta text,
    usuario_id integer NOT NULL,
    estado_id integer NOT NULL
);
    DROP TABLE parque.pqr;
       parque         postgres    false    6         �           0    0 	   TABLE pqr    COMMENT     X   COMMENT ON TABLE parque.pqr IS 'Tabla dedicada a almacenar las preguntas del sistema ';
            parque       postgres    false    216         �            1259    87852 
   pqr_id_seq    SEQUENCE     s   CREATE SEQUENCE parque.pqr_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 !   DROP SEQUENCE parque.pqr_id_seq;
       parque       postgres    false    216    6         �           0    0 
   pqr_id_seq    SEQUENCE OWNED BY     9   ALTER SEQUENCE parque.pqr_id_seq OWNED BY parque.pqr.id;
            parque       postgres    false    217         �            1259    87854    reserva    TABLE     �   CREATE TABLE parque.reserva (
    id bigint NOT NULL,
    fecha_compra timestamp without time zone NOT NULL,
    precio double precision NOT NULL,
    usuario_id integer NOT NULL,
    estado_id integer NOT NULL
);
    DROP TABLE parque.reserva;
       parque         postgres    false    6         �           0    0    TABLE reserva    COMMENT     �   COMMENT ON TABLE parque.reserva IS 'Tabla dedicada a almacenar los datos de las reservas del sistema, esta sera heredada por otras tablas para independizar la información correspondiente ';
            parque       postgres    false    218         �            1259    87857    reserva_cabana    TABLE     a   CREATE TABLE parque.reserva_cabana (
    ticket_id integer NOT NULL
)
INHERITS (parque.reserva);
 "   DROP TABLE parque.reserva_cabana;
       parque         postgres    false    6    218         �            1259    87860    reserva_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.reserva_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE parque.reserva_id_seq;
       parque       postgres    false    218    6         �           0    0    reserva_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE parque.reserva_id_seq OWNED BY parque.reserva.id;
            parque       postgres    false    220         �            1259    87862    reserva_ticket    TABLE     �   CREATE TABLE parque.reserva_ticket (
    ticket_id bigint NOT NULL,
    qr text NOT NULL,
    cantidad double precision NOT NULL
)
INHERITS (parque.reserva);
 "   DROP TABLE parque.reserva_ticket;
       parque         postgres    false    6    218         �           0    0    TABLE reserva_ticket    COMMENT     �   COMMENT ON TABLE parque.reserva_ticket IS 'Tabla dedicada a almacenar los datos de los tiquetes de los usuarios heredada de la tabla reserva ';
            parque       postgres    false    221         �            1259    87868    rol    TABLE     O   CREATE TABLE parque.rol (
    id integer NOT NULL,
    nombre text NOT NULL
);
    DROP TABLE parque.rol;
       parque         postgres    false    6         �           0    0 	   TABLE rol    COMMENT     d   COMMENT ON TABLE parque.rol IS 'Tabla dedicada a almacenar los roles correspondientes al sistema ';
            parque       postgres    false    222         �            1259    87874 
   rol_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.rol_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 !   DROP SEQUENCE parque.rol_id_seq;
       parque       postgres    false    6    222         �           0    0 
   rol_id_seq    SEQUENCE OWNED BY     9   ALTER SEQUENCE parque.rol_id_seq OWNED BY parque.rol.id;
            parque       postgres    false    223         �            1259    87876    ticket    TABLE     x   CREATE TABLE parque.ticket (
    id integer NOT NULL,
    nombre text NOT NULL,
    precio double precision NOT NULL
);
    DROP TABLE parque.ticket;
       parque         postgres    false    6         �           0    0    TABLE ticket    COMMENT     �   COMMENT ON TABLE parque.ticket IS 'Tabla destinada a almacenar los datos de los tiquetes de los usuarios del sistema,profe si llega a ver esto pasenos';
            parque       postgres    false    224         �            1259    87882    ticket_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.ticket_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE parque.ticket_id_seq;
       parque       postgres    false    6    224         �           0    0    ticket_id_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE parque.ticket_id_seq OWNED BY parque.ticket.id;
            parque       postgres    false    225         �            1259    87884    usuario    TABLE     �  CREATE TABLE parque.usuario (
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
    rol_id integer NOT NULL
);
    DROP TABLE parque.usuario;
       parque         postgres    false    6         �           0    0    TABLE usuario    COMMENT     �   COMMENT ON TABLE parque.usuario IS 'Tabla dedicada a almacenar los datos correspondientes a los usuarios que se registren en el sistema';
            parque       postgres    false    226         �            1259    87890    usuario_id_seq    SEQUENCE     �   CREATE SEQUENCE parque.usuario_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 %   DROP SEQUENCE parque.usuario_id_seq;
       parque       postgres    false    226    6         �           0    0    usuario_id_seq    SEQUENCE OWNED BY     A   ALTER SEQUENCE parque.usuario_id_seq OWNED BY parque.usuario.id;
            parque       postgres    false    227         �            1259    87892 	   auditoria    TABLE     R  CREATE TABLE security.auditoria (
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
       security         postgres    false    8         �            1259    87898    auditoria_id_seq    SEQUENCE     {   CREATE SEQUENCE security.auditoria_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 )   DROP SEQUENCE security.auditoria_id_seq;
       security       postgres    false    8    228         �           0    0    auditoria_id_seq    SEQUENCE OWNED BY     I   ALTER SEQUENCE security.auditoria_id_seq OWNED BY security.auditoria.id;
            security       postgres    false    229         �            1259    87900    function_db_view    VIEW     �  CREATE VIEW security.function_db_view AS
 SELECT pp.proname AS b_function,
    oidvectortypes(pp.proargtypes) AS b_type_parameters
   FROM (pg_proc pp
     JOIN pg_namespace pn ON ((pn.oid = pp.pronamespace)))
  WHERE ((pn.nspname)::text <> ALL (ARRAY[('pg_catalog'::character varying)::text, ('information_schema'::character varying)::text, ('admin_control'::character varying)::text, ('vial'::character varying)::text]));
 %   DROP VIEW security.function_db_view;
       security       postgres    false    8         �
           2604    87905 	   cabana id    DEFAULT     f   ALTER TABLE ONLY parque.cabana ALTER COLUMN id SET DEFAULT nextval('parque.cabana_id_seq'::regclass);
 8   ALTER TABLE parque.cabana ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    199    198         �
           2604    87906    comentario_cabana id    DEFAULT     v   ALTER TABLE ONLY parque.comentario_cabana ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);
 C   ALTER TABLE parque.comentario_cabana ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    201    205         �
           2604    87907    comentario_evento id    DEFAULT     v   ALTER TABLE ONLY parque.comentario_evento ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);
 C   ALTER TABLE parque.comentario_evento ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    202    205         �
           2604    87908    comentario_noticia id    DEFAULT     w   ALTER TABLE ONLY parque.comentario_noticia ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);
 D   ALTER TABLE parque.comentario_noticia ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    205    203         �
           2604    87909    comentario_pictograma id    DEFAULT     z   ALTER TABLE ONLY parque.comentario_pictograma ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);
 G   ALTER TABLE parque.comentario_pictograma ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    205    204         �
           2604    87910    comentarios id    DEFAULT     p   ALTER TABLE ONLY parque.comentarios ALTER COLUMN id SET DEFAULT nextval('parque.comentarios_id_seq'::regclass);
 =   ALTER TABLE parque.comentarios ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    205    200                     2604    87911    estado_reserva id    DEFAULT     n   ALTER TABLE ONLY parque.estado_reserva ALTER COLUMN id SET DEFAULT nextval('parque.estado_id_seq'::regclass);
 @   ALTER TABLE parque.estado_reserva ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    207    206                    2604    87912 	   evento id    DEFAULT     f   ALTER TABLE ONLY parque.evento ALTER COLUMN id SET DEFAULT nextval('parque.evento_id_seq'::regclass);
 8   ALTER TABLE parque.evento ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    210    209                    2604    87913    informacion_parque id    DEFAULT     ~   ALTER TABLE ONLY parque.informacion_parque ALTER COLUMN id SET DEFAULT nextval('parque.informacion_parque_id_seq'::regclass);
 D   ALTER TABLE parque.informacion_parque ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    212    211                    2604    87914    pictograma id    DEFAULT     n   ALTER TABLE ONLY parque.pictograma ALTER COLUMN id SET DEFAULT nextval('parque.pictograma_id_seq'::regclass);
 <   ALTER TABLE parque.pictograma ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    215    214                    2604    87915    pqr id    DEFAULT     `   ALTER TABLE ONLY parque.pqr ALTER COLUMN id SET DEFAULT nextval('parque.pqr_id_seq'::regclass);
 5   ALTER TABLE parque.pqr ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    217    216                    2604    87916 
   reserva id    DEFAULT     h   ALTER TABLE ONLY parque.reserva ALTER COLUMN id SET DEFAULT nextval('parque.reserva_id_seq'::regclass);
 9   ALTER TABLE parque.reserva ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    220    218                    2604    87917    reserva_cabana id    DEFAULT     o   ALTER TABLE ONLY parque.reserva_cabana ALTER COLUMN id SET DEFAULT nextval('parque.reserva_id_seq'::regclass);
 @   ALTER TABLE parque.reserva_cabana ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    220    219                    2604    87918    reserva_ticket id    DEFAULT     o   ALTER TABLE ONLY parque.reserva_ticket ALTER COLUMN id SET DEFAULT nextval('parque.reserva_id_seq'::regclass);
 @   ALTER TABLE parque.reserva_ticket ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    221    220                    2604    87919    rol id    DEFAULT     `   ALTER TABLE ONLY parque.rol ALTER COLUMN id SET DEFAULT nextval('parque.rol_id_seq'::regclass);
 5   ALTER TABLE parque.rol ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    223    222         	           2604    87920 	   ticket id    DEFAULT     f   ALTER TABLE ONLY parque.ticket ALTER COLUMN id SET DEFAULT nextval('parque.ticket_id_seq'::regclass);
 8   ALTER TABLE parque.ticket ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    225    224         
           2604    87921 
   usuario id    DEFAULT     h   ALTER TABLE ONLY parque.usuario ALTER COLUMN id SET DEFAULT nextval('parque.usuario_id_seq'::regclass);
 9   ALTER TABLE parque.usuario ALTER COLUMN id DROP DEFAULT;
       parque       postgres    false    227    226                    2604    87922    auditoria id    DEFAULT     p   ALTER TABLE ONLY security.auditoria ALTER COLUMN id SET DEFAULT nextval('security.auditoria_id_seq'::regclass);
 =   ALTER TABLE security.auditoria ALTER COLUMN id DROP DEFAULT;
       security       postgres    false    229    228         �          0    87761    cabana 
   TABLE DATA               k   COPY parque.cabana (id, nombre, capacidad, precio, imagenes_url, calificacion, comentarios_id) FROM stdin;
    parque       postgres    false    198       2996.dat �          0    87776    comentario_cabana 
   TABLE DATA                  COPY parque.comentario_cabana (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, cabana_id) FROM stdin;
    parque       postgres    false    201       2999.dat �          0    87782    comentario_evento 
   TABLE DATA                  COPY parque.comentario_evento (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, evento_id) FROM stdin;
    parque       postgres    false    202       3000.dat �          0    87788    comentario_noticia 
   TABLE DATA               �   COPY parque.comentario_noticia (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, noticia_id) FROM stdin;
    parque       postgres    false    203       3001.dat �          0    87794    comentario_pictograma 
   TABLE DATA               �   COPY parque.comentario_pictograma (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, pictograma_id) FROM stdin;
    parque       postgres    false    204       3002.dat �          0    87770    comentarios 
   TABLE DATA               n   COPY parque.comentarios (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id) FROM stdin;
    parque       postgres    false    200       2998.dat �          0    87810 
   estado_pqr 
   TABLE DATA               3   COPY parque.estado_pqr ("id ", nombre) FROM stdin;
    parque       postgres    false    208       3006.dat �          0    87802    estado_reserva 
   TABLE DATA               4   COPY parque.estado_reserva (id, nombre) FROM stdin;
    parque       postgres    false    206       3004.dat �          0    87816    evento 
   TABLE DATA                  COPY parque.evento (id, nombre, fecha_publicacion, descripcion, comentarios_id, calificacion, fecha, imagenes_url) FROM stdin;
    parque       postgres    false    209       3007.dat �          0    87824    informacion_parque 
   TABLE DATA               T   COPY parque.informacion_parque (id, propety, descripcion, imagenes_url) FROM stdin;
    parque       postgres    false    211       3009.dat �          0    87832    noticia 
   TABLE DATA               y   COPY parque.noticia (id, titulo, descripcion, fecha_publicacion, imagenes_url, comentarios_id, calificacion) FROM stdin;
    parque       postgres    false    213       3011.dat �          0    87838 
   pictograma 
   TABLE DATA               i   COPY parque.pictograma (id, nombre, calificacion, imagenes_url, descripcion, comentarios_id) FROM stdin;
    parque       postgres    false    214       3012.dat �          0    87846    pqr 
   TABLE DATA               `   COPY parque.pqr (id, fecha_publicacion, pregunta, respuesta, usuario_id, estado_id) FROM stdin;
    parque       postgres    false    216       3014.dat �          0    87854    reserva 
   TABLE DATA               R   COPY parque.reserva (id, fecha_compra, precio, usuario_id, estado_id) FROM stdin;
    parque       postgres    false    218       3016.dat �          0    87857    reserva_cabana 
   TABLE DATA               d   COPY parque.reserva_cabana (id, fecha_compra, precio, usuario_id, estado_id, ticket_id) FROM stdin;
    parque       postgres    false    219       3017.dat �          0    87862    reserva_ticket 
   TABLE DATA               r   COPY parque.reserva_ticket (id, fecha_compra, precio, usuario_id, estado_id, ticket_id, qr, cantidad) FROM stdin;
    parque       postgres    false    221       3019.dat �          0    87868    rol 
   TABLE DATA               )   COPY parque.rol (id, nombre) FROM stdin;
    parque       postgres    false    222       3020.dat �          0    87876    ticket 
   TABLE DATA               4   COPY parque.ticket (id, nombre, precio) FROM stdin;
    parque       postgres    false    224       3022.dat �          0    87884    usuario 
   TABLE DATA               �   COPY parque.usuario (id, nombre, apellido, tipo_documento, numero_documento, lugar_expedicion, correo_electronico, clave, icono_url, verificacion_cuenta, estado_cuenta, rol_id) FROM stdin;
    parque       postgres    false    226       3024.dat �          0    87892 	   auditoria 
   TABLE DATA               a   COPY security.auditoria (id, fecha, accion, schema, tabla, token, user_bd, data, pk) FROM stdin;
    security       postgres    false    228       3026.dat �           0    0    cabana_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('parque.cabana_id_seq', 25, true);
            parque       postgres    false    199         �           0    0    comentarios_id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('parque.comentarios_id_seq', 2, true);
            parque       postgres    false    205         �           0    0    estado_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('parque.estado_id_seq', 1, false);
            parque       postgres    false    207         �           0    0    evento_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('parque.evento_id_seq', 5, true);
            parque       postgres    false    210         �           0    0    informacion_parque_id_seq    SEQUENCE SET     H   SELECT pg_catalog.setval('parque.informacion_parque_id_seq', 1, false);
            parque       postgres    false    212         �           0    0    pictograma_id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('parque.pictograma_id_seq', 1, false);
            parque       postgres    false    215         �           0    0 
   pqr_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('parque.pqr_id_seq', 1, false);
            parque       postgres    false    217         �           0    0    reserva_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('parque.reserva_id_seq', 1, false);
            parque       postgres    false    220         �           0    0 
   rol_id_seq    SEQUENCE SET     9   SELECT pg_catalog.setval('parque.rol_id_seq', 1, false);
            parque       postgres    false    223         �           0    0    ticket_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('parque.ticket_id_seq', 1, false);
            parque       postgres    false    225         �           0    0    usuario_id_seq    SEQUENCE SET     =   SELECT pg_catalog.setval('parque.usuario_id_seq', 1, false);
            parque       postgres    false    227                     0    0    auditoria_id_seq    SEQUENCE SET     A   SELECT pg_catalog.setval('security.auditoria_id_seq', 32, true);
            security       postgres    false    229                    2606    87924    estado_pqr ok_estado_pqr 
   CONSTRAINT     Y   ALTER TABLE ONLY parque.estado_pqr
    ADD CONSTRAINT ok_estado_pqr PRIMARY KEY ("id ");
 B   ALTER TABLE ONLY parque.estado_pqr DROP CONSTRAINT ok_estado_pqr;
       parque         postgres    false    208                    2606    87926    pictograma pictograma_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY parque.pictograma
    ADD CONSTRAINT pictograma_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY parque.pictograma DROP CONSTRAINT pictograma_pkey;
       parque         postgres    false    214                    2606    87928    cabana pk_cabana 
   CONSTRAINT     N   ALTER TABLE ONLY parque.cabana
    ADD CONSTRAINT pk_cabana PRIMARY KEY (id);
 :   ALTER TABLE ONLY parque.cabana DROP CONSTRAINT pk_cabana;
       parque         postgres    false    198                    2606    87930    comentarios pk_comentarios 
   CONSTRAINT     X   ALTER TABLE ONLY parque.comentarios
    ADD CONSTRAINT pk_comentarios PRIMARY KEY (id);
 D   ALTER TABLE ONLY parque.comentarios DROP CONSTRAINT pk_comentarios;
       parque         postgres    false    200                    2606    87932    estado_reserva pk_estado 
   CONSTRAINT     V   ALTER TABLE ONLY parque.estado_reserva
    ADD CONSTRAINT pk_estado PRIMARY KEY (id);
 B   ALTER TABLE ONLY parque.estado_reserva DROP CONSTRAINT pk_estado;
       parque         postgres    false    206                    2606    87934    evento pk_evento 
   CONSTRAINT     N   ALTER TABLE ONLY parque.evento
    ADD CONSTRAINT pk_evento PRIMARY KEY (id);
 :   ALTER TABLE ONLY parque.evento DROP CONSTRAINT pk_evento;
       parque         postgres    false    209                    2606    87936 (   informacion_parque pk_informacion_parque 
   CONSTRAINT     f   ALTER TABLE ONLY parque.informacion_parque
    ADD CONSTRAINT pk_informacion_parque PRIMARY KEY (id);
 R   ALTER TABLE ONLY parque.informacion_parque DROP CONSTRAINT pk_informacion_parque;
       parque         postgres    false    211                    2606    87938    noticia pk_noticia 
   CONSTRAINT     P   ALTER TABLE ONLY parque.noticia
    ADD CONSTRAINT pk_noticia PRIMARY KEY (id);
 <   ALTER TABLE ONLY parque.noticia DROP CONSTRAINT pk_noticia;
       parque         postgres    false    213                    2606    87940 
   pqr pk_pqr 
   CONSTRAINT     H   ALTER TABLE ONLY parque.pqr
    ADD CONSTRAINT pk_pqr PRIMARY KEY (id);
 4   ALTER TABLE ONLY parque.pqr DROP CONSTRAINT pk_pqr;
       parque         postgres    false    216                    2606    87942    reserva pk_reserva 
   CONSTRAINT     P   ALTER TABLE ONLY parque.reserva
    ADD CONSTRAINT pk_reserva PRIMARY KEY (id);
 <   ALTER TABLE ONLY parque.reserva DROP CONSTRAINT pk_reserva;
       parque         postgres    false    218         !           2606    87944 
   rol pk_rol 
   CONSTRAINT     H   ALTER TABLE ONLY parque.rol
    ADD CONSTRAINT pk_rol PRIMARY KEY (id);
 4   ALTER TABLE ONLY parque.rol DROP CONSTRAINT pk_rol;
       parque         postgres    false    222         #           2606    87946    ticket pk_ticket 
   CONSTRAINT     N   ALTER TABLE ONLY parque.ticket
    ADD CONSTRAINT pk_ticket PRIMARY KEY (id);
 :   ALTER TABLE ONLY parque.ticket DROP CONSTRAINT pk_ticket;
       parque         postgres    false    224         %           2606    87948    usuario pk_usuario 
   CONSTRAINT     P   ALTER TABLE ONLY parque.usuario
    ADD CONSTRAINT pk_usuario PRIMARY KEY (id);
 <   ALTER TABLE ONLY parque.usuario DROP CONSTRAINT pk_usuario;
       parque         postgres    false    226         '           2606    87950    auditoria auditoria_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY security.auditoria
    ADD CONSTRAINT auditoria_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY security.auditoria DROP CONSTRAINT auditoria_pkey;
       security         postgres    false    228         (           2620    87951    cabana tg_parque_cabana    TRIGGER     �   CREATE TRIGGER tg_parque_cabana AFTER INSERT OR DELETE OR UPDATE ON parque.cabana FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 0   DROP TRIGGER tg_parque_cabana ON parque.cabana;
       parque       postgres    false    198    231         *           2620    87952 -   comentario_cabana tg_parque_comentario_cabana    TRIGGER     �   CREATE TRIGGER tg_parque_comentario_cabana AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_cabana FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 F   DROP TRIGGER tg_parque_comentario_cabana ON parque.comentario_cabana;
       parque       postgres    false    201    231         +           2620    87953 -   comentario_evento tg_parque_comentario_evento    TRIGGER     �   CREATE TRIGGER tg_parque_comentario_evento AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_evento FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 F   DROP TRIGGER tg_parque_comentario_evento ON parque.comentario_evento;
       parque       postgres    false    231    202         ,           2620    87954 /   comentario_noticia tg_parque_comentario_noticia    TRIGGER     �   CREATE TRIGGER tg_parque_comentario_noticia AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_noticia FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 H   DROP TRIGGER tg_parque_comentario_noticia ON parque.comentario_noticia;
       parque       postgres    false    203    231         -           2620    87955 5   comentario_pictograma tg_parque_comentario_pictograma    TRIGGER     �   CREATE TRIGGER tg_parque_comentario_pictograma AFTER INSERT OR DELETE OR UPDATE ON parque.comentario_pictograma FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 N   DROP TRIGGER tg_parque_comentario_pictograma ON parque.comentario_pictograma;
       parque       postgres    false    204    231         )           2620    87956 !   comentarios tg_parque_comentarios    TRIGGER     �   CREATE TRIGGER tg_parque_comentarios AFTER INSERT OR DELETE OR UPDATE ON parque.comentarios FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 :   DROP TRIGGER tg_parque_comentarios ON parque.comentarios;
       parque       postgres    false    231    200         /           2620    87957    estado_pqr tg_parque_estado_pqr    TRIGGER     �   CREATE TRIGGER tg_parque_estado_pqr AFTER INSERT OR DELETE OR UPDATE ON parque.estado_pqr FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 8   DROP TRIGGER tg_parque_estado_pqr ON parque.estado_pqr;
       parque       postgres    false    208    231         .           2620    87958 '   estado_reserva tg_parque_estado_reserva    TRIGGER     �   CREATE TRIGGER tg_parque_estado_reserva AFTER INSERT OR DELETE OR UPDATE ON parque.estado_reserva FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 @   DROP TRIGGER tg_parque_estado_reserva ON parque.estado_reserva;
       parque       postgres    false    231    206         0           2620    87959    evento tg_parque_evento    TRIGGER     �   CREATE TRIGGER tg_parque_evento AFTER INSERT OR DELETE OR UPDATE ON parque.evento FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 0   DROP TRIGGER tg_parque_evento ON parque.evento;
       parque       postgres    false    231    209         1           2620    87960    noticia tg_parque_noticia    TRIGGER     �   CREATE TRIGGER tg_parque_noticia AFTER INSERT OR DELETE OR UPDATE ON parque.noticia FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 2   DROP TRIGGER tg_parque_noticia ON parque.noticia;
       parque       postgres    false    213    231         2           2620    87961    pictograma tg_parque_pictograma    TRIGGER     �   CREATE TRIGGER tg_parque_pictograma AFTER INSERT OR DELETE OR UPDATE ON parque.pictograma FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 8   DROP TRIGGER tg_parque_pictograma ON parque.pictograma;
       parque       postgres    false    231    214         3           2620    87962    pqr tg_parque_pqr    TRIGGER     �   CREATE TRIGGER tg_parque_pqr AFTER INSERT OR DELETE OR UPDATE ON parque.pqr FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 *   DROP TRIGGER tg_parque_pqr ON parque.pqr;
       parque       postgres    false    231    216         4           2620    87963    reserva tg_parque_reserva    TRIGGER     �   CREATE TRIGGER tg_parque_reserva AFTER INSERT OR DELETE OR UPDATE ON parque.reserva FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 2   DROP TRIGGER tg_parque_reserva ON parque.reserva;
       parque       postgres    false    231    218         5           2620    87964 '   reserva_cabana tg_parque_reserva_cabana    TRIGGER     �   CREATE TRIGGER tg_parque_reserva_cabana AFTER INSERT OR DELETE OR UPDATE ON parque.reserva_cabana FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 @   DROP TRIGGER tg_parque_reserva_cabana ON parque.reserva_cabana;
       parque       postgres    false    219    231         6           2620    87965 '   reserva_ticket tg_parque_reserva_ticket    TRIGGER     �   CREATE TRIGGER tg_parque_reserva_ticket AFTER INSERT OR DELETE OR UPDATE ON parque.reserva_ticket FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 @   DROP TRIGGER tg_parque_reserva_ticket ON parque.reserva_ticket;
       parque       postgres    false    231    221         7           2620    87966    rol tg_parque_rol    TRIGGER     �   CREATE TRIGGER tg_parque_rol AFTER INSERT OR DELETE OR UPDATE ON parque.rol FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 *   DROP TRIGGER tg_parque_rol ON parque.rol;
       parque       postgres    false    231    222         8           2620    87967    ticket tg_parque_ticket    TRIGGER     �   CREATE TRIGGER tg_parque_ticket AFTER INSERT OR DELETE OR UPDATE ON parque.ticket FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 0   DROP TRIGGER tg_parque_ticket ON parque.ticket;
       parque       postgres    false    231    224         9           2620    87968    usuario tg_parque_usuario    TRIGGER     �   CREATE TRIGGER tg_parque_usuario AFTER INSERT OR DELETE OR UPDATE ON parque.usuario FOR EACH ROW EXECUTE PROCEDURE security.f_log_auditoria();
 2   DROP TRIGGER tg_parque_usuario ON parque.usuario;
       parque       postgres    false    226    231                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        2996.dat                                                                                            0000600 0004000 0002000 00000000040 13632002770 0014255 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        25	asdw	2	1	asdsad	123	asd
\.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                2999.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014261 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3000.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014227 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3001.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014230 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3002.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014231 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           2998.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014260 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3006.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014235 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3004.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014233 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3007.dat                                                                                            0000600 0004000 0002000 00000001320 13632002770 0014237 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        1	Evento 1	2020-03-10 15:26:52.575583	Descripcion 1		12	2020-04-15 15:26:52.575583	evento1.jpg@evento2.jpg@evento3.jpg@evento4.jpg@evento5.jpg
3	Evento 2	2020-03-10 15:26:52.575583	Descripcion 2		12	2020-04-15 15:26:52.575583	evento1.jpg@evento2.jpg@evento3.jpg@evento4.jpg@evento5.jpg
5	Evento 4	2020-03-10 15:26:52.575583	Descripcion 2		12	2020-04-15 15:26:52.575583	evento1.jpg@evento2.jpg@evento3.jpg@evento4.jpg@evento5.jpg
2	Evento 2	2020-03-10 15:28:03.904044	Descripcion 2		12	2020-03-15 15:28:03.904044	evento1.jpg@evento2.jpg@evento3.jpg@evento4.jpg@evento5.jpg
4	Evento 3	2020-03-10 15:28:03.904044	Descripcion 2		12	2020-03-15 15:28:03.904044	evento1.jpg@evento2.jpg@evento3.jpg@evento4.jpg@evento5.jpg
\.


                                                                                                                                                                                                                                                                                                                3009.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014240 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3011.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014231 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3012.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014232 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3014.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014234 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3016.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014236 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3017.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014237 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3019.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014241 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3020.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014231 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3022.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014233 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3024.dat                                                                                            0000600 0004000 0002000 00000000005 13632002770 0014235 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        \.


                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           3026.dat                                                                                            0000600 0004000 0002000 00000014274 13632002770 0014254 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        1	2020-03-09 19:57:34.438211	DELETE	parque	cabana		postgres	{"id_anterior": 24, "nombre_anterior": "Casi", "precio_anterior": 23, "capacidad_anterior": 2, "calificacion_anterior": 12, "imagenes_url_anterior": "asdd", "comentarios_id_anterior": "asd"}	24
2	2020-03-09 20:00:39.946159	DELETE	parque	comentarios		postgres	{"id_anterior": 1, "reportado_anterior": true, "usuario_id_anterior": 7, "descripcion_anterior": "hgjhjk", "calificacion_anterior": 987, "fecha_publicacion_anterior": "1999-01-01T00:00:00"}	1
3	2020-03-09 20:08:55.864435	INSERT	parque	cabana		postgres	{"id_nuevo": 25, "nombre_nuevo": "asdw", "precio_nuevo": 1, "capacidad_nuevo": 2, "calificacion_nuevo": 123, "imagenes_url_nuevo": "asdsad", "comentarios_id_nuevo": "asd"}	25
4	2020-03-10 15:09:35.83298	INSERT	parque	evento		postgres	{"id_nuevo": 1, "nombre_nuevo": "Evento 1", "descripcion_nuevo": "Descripcion 1", "calificacion_nuevo": 12, "imagenes_url_nuevo": "image1.jpg@image2.jpg@image3.jpg", "comentarios_id_nuevo": "", "fecha_publicacion_nuevo": "2020-03-10T15:09:35.83298"}	1
5	2020-03-10 15:10:18.276098	INSERT	parque	evento		postgres	{"id_nuevo": 2, "nombre_nuevo": "Evento 2", "descripcion_nuevo": "Descripcion 2", "calificacion_nuevo": 12, "imagenes_url_nuevo": "image1.jpg@image2.jpg@image3.jpg", "comentarios_id_nuevo": "", "fecha_publicacion_nuevo": "2020-04-10T15:10:18.276098"}	2
6	2020-03-10 15:10:43.665397	INSERT	parque	evento		postgres	{"id_nuevo": 3, "nombre_nuevo": "Evento 2", "descripcion_nuevo": "Descripcion 2", "calificacion_nuevo": 12, "imagenes_url_nuevo": "image1.jpg@image2.jpg@image3.jpg", "comentarios_id_nuevo": "", "fecha_publicacion_nuevo": "2020-04-13T15:10:43.665397"}	3
7	2020-03-10 15:11:14.8688	INSERT	parque	evento		postgres	{"id_nuevo": 4, "nombre_nuevo": "Evento 3", "descripcion_nuevo": "Descripcion 2", "calificacion_nuevo": 12, "imagenes_url_nuevo": "image1.jpg@image2.jpg@image3.jpg", "comentarios_id_nuevo": "", "fecha_publicacion_nuevo": "2020-04-20T15:11:14.8688"}	4
8	2020-03-10 15:11:28.933445	INSERT	parque	evento		postgres	{"id_nuevo": 5, "nombre_nuevo": "Evento 4", "descripcion_nuevo": "Descripcion 2", "calificacion_nuevo": 12, "imagenes_url_nuevo": "image1.jpg@image2.jpg@image3.jpg", "comentarios_id_nuevo": "", "fecha_publicacion_nuevo": "2020-03-10T15:11:28.933445"}	5
9	2020-03-10 15:26:31.544097	UPDATE	parque	evento		postgres	{"fecha_publicacion_nuevo": "2020-03-10T15:26:31.544097", "fecha_publicacion_anterior": "2020-03-10T15:09:35.83298"}	1
10	2020-03-10 15:26:31.544097	UPDATE	parque	evento		postgres	{"fecha_publicacion_nuevo": "2020-03-10T15:26:31.544097", "fecha_publicacion_anterior": "2020-04-10T15:10:18.276098"}	2
11	2020-03-10 15:26:31.544097	UPDATE	parque	evento		postgres	{"fecha_publicacion_nuevo": "2020-03-10T15:26:31.544097", "fecha_publicacion_anterior": "2020-04-13T15:10:43.665397"}	3
12	2020-03-10 15:26:31.544097	UPDATE	parque	evento		postgres	{"fecha_publicacion_nuevo": "2020-03-10T15:26:31.544097", "fecha_publicacion_anterior": "2020-04-20T15:11:14.8688"}	4
13	2020-03-10 15:26:31.544097	UPDATE	parque	evento		postgres	{"fecha_publicacion_nuevo": "2020-03-10T15:26:31.544097", "fecha_publicacion_anterior": "2020-03-10T15:11:28.933445"}	5
14	2020-03-10 15:26:52.575583	UPDATE	parque	evento		postgres	{"fecha_nuevo": "2020-04-15T15:26:52.575583", "fecha_anterior": "2020-04-15T15:26:31.544097", "fecha_publicacion_nuevo": "2020-03-10T15:26:52.575583", "fecha_publicacion_anterior": "2020-03-10T15:26:31.544097"}	1
15	2020-03-10 15:26:52.575583	UPDATE	parque	evento		postgres	{"fecha_nuevo": "2020-04-15T15:26:52.575583", "fecha_anterior": "2020-04-15T15:26:31.544097", "fecha_publicacion_nuevo": "2020-03-10T15:26:52.575583", "fecha_publicacion_anterior": "2020-03-10T15:26:31.544097"}	2
16	2020-03-10 15:26:52.575583	UPDATE	parque	evento		postgres	{"fecha_nuevo": "2020-04-15T15:26:52.575583", "fecha_anterior": "2020-04-15T15:26:31.544097", "fecha_publicacion_nuevo": "2020-03-10T15:26:52.575583", "fecha_publicacion_anterior": "2020-03-10T15:26:31.544097"}	3
17	2020-03-10 15:26:52.575583	UPDATE	parque	evento		postgres	{"fecha_nuevo": "2020-04-15T15:26:52.575583", "fecha_anterior": "2020-04-15T15:26:31.544097", "fecha_publicacion_nuevo": "2020-03-10T15:26:52.575583", "fecha_publicacion_anterior": "2020-03-10T15:26:31.544097"}	4
18	2020-03-10 15:26:52.575583	UPDATE	parque	evento		postgres	{"fecha_nuevo": "2020-04-15T15:26:52.575583", "fecha_anterior": "2020-04-15T15:26:31.544097", "fecha_publicacion_nuevo": "2020-03-10T15:26:52.575583", "fecha_publicacion_anterior": "2020-03-10T15:26:31.544097"}	5
19	2020-03-10 15:27:32.306237	UPDATE	parque	evento		postgres	{"fecha_nuevo": "2020-03-10T15:27:32.306237", "fecha_anterior": "2020-04-15T15:26:52.575583", "fecha_publicacion_nuevo": "2020-03-10T15:27:32.306237", "fecha_publicacion_anterior": "2020-03-10T15:26:52.575583"}	2
20	2020-03-10 15:27:32.306237	UPDATE	parque	evento		postgres	{"fecha_nuevo": "2020-03-10T15:27:32.306237", "fecha_anterior": "2020-04-15T15:26:52.575583", "fecha_publicacion_nuevo": "2020-03-10T15:27:32.306237", "fecha_publicacion_anterior": "2020-03-10T15:26:52.575583"}	4
21	2020-03-10 15:28:03.904044	UPDATE	parque	evento		postgres	{"fecha_nuevo": "2020-03-15T15:28:03.904044", "fecha_anterior": "2020-03-10T15:27:32.306237", "fecha_publicacion_nuevo": "2020-03-10T15:28:03.904044", "fecha_publicacion_anterior": "2020-03-10T15:27:32.306237"}	2
22	2020-03-10 15:28:03.904044	UPDATE	parque	evento		postgres	{"fecha_nuevo": "2020-03-15T15:28:03.904044", "fecha_anterior": "2020-03-10T15:27:32.306237", "fecha_publicacion_nuevo": "2020-03-10T15:28:03.904044", "fecha_publicacion_anterior": "2020-03-10T15:27:32.306237"}	4
23	2020-03-10 16:10:22.59545	UPDATE	parque	evento		postgres	{}	1
24	2020-03-10 16:10:22.59545	UPDATE	parque	evento		postgres	{}	3
25	2020-03-10 16:10:22.59545	UPDATE	parque	evento		postgres	{}	5
26	2020-03-10 16:10:22.59545	UPDATE	parque	evento		postgres	{}	2
27	2020-03-10 16:10:22.59545	UPDATE	parque	evento		postgres	{}	4
28	2020-03-10 16:23:05.139923	UPDATE	parque	evento		postgres	{}	1
29	2020-03-10 16:23:05.139923	UPDATE	parque	evento		postgres	{}	3
30	2020-03-10 16:23:05.139923	UPDATE	parque	evento		postgres	{}	5
31	2020-03-10 16:23:05.139923	UPDATE	parque	evento		postgres	{}	2
32	2020-03-10 16:23:05.139923	UPDATE	parque	evento		postgres	{}	4
\.


                                                                                                                                                                                                                                                                                                                                    restore.sql                                                                                         0000600 0004000 0002000 00000143121 13632002770 0015366 0                                                                                                    ustar 00postgres                        postgres                        0000000 0000000                                                                                                                                                                        --
-- NOTE:
--
-- File paths need to be edited. Search for $$PATH$$ and
-- replace it with the path to the directory containing
-- the extracted data files.
--
--
-- PostgreSQL database dump
--

-- Dumped from database version 11.5
-- Dumped by pg_dump version 11.5

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

DROP DATABASE piedras;
--
-- Name: piedras; Type: DATABASE; Schema: -; Owner: postgres
--

CREATE DATABASE piedras WITH TEMPLATE = template0 ENCODING = 'UTF8' LC_COLLATE = 'Spanish_Spain.1252' LC_CTYPE = 'Spanish_Spain.1252';


ALTER DATABASE piedras OWNER TO postgres;

\connect piedras

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

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
			_token := _new_data.session;
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
    comentarios_id text NOT NULL
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
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('capacidad_anterior', _data_old.capacidad)::jsonb;
				_datos := _datos || json_build_object('precio_anterior', _data_old.precio)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				
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
-- Name: evento; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.evento (
    id integer NOT NULL,
    nombre text NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    descripcion text NOT NULL,
    comentarios_id text NOT NULL,
    calificacion double precision,
    fecha timestamp without time zone,
    imagenes_url text
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
				_datos := _datos || json_build_object('comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_nuevo', _data_new.calificacion)::jsonb;
				_datos := _datos || json_build_object('fecha_nuevo', _data_new.fecha)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
				
		ELSEIF _accion = 'DELETE'
			THEN
				_datos := _datos || json_build_object('id_anterior', _data_old.id)::jsonb;
				_datos := _datos || json_build_object('nombre_anterior', _data_old.nombre)::jsonb;
				_datos := _datos || json_build_object('fecha_publicacion_anterior', _data_old.fecha_publicacion)::jsonb;
				_datos := _datos || json_build_object('descripcion_anterior', _data_old.descripcion)::jsonb;
				_datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id)::jsonb;
				_datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion)::jsonb;
				_datos := _datos || json_build_object('fecha_anterior', _data_old.fecha)::jsonb;
				_datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url)::jsonb;
				
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
			IF _data_old.comentarios_id <> _data_new.comentarios_id
				THEN _datos := _datos || json_build_object('comentarios_id_anterior', _data_old.comentarios_id, 'comentarios_id_nuevo', _data_new.comentarios_id)::jsonb;
			END IF;
			IF _data_old.calificacion <> _data_new.calificacion
				THEN _datos := _datos || json_build_object('calificacion_anterior', _data_old.calificacion, 'calificacion_nuevo', _data_new.calificacion)::jsonb;
			END IF;
			IF _data_old.fecha <> _data_new.fecha
				THEN _datos := _datos || json_build_object('fecha_anterior', _data_old.fecha, 'fecha_nuevo', _data_new.fecha)::jsonb;
			END IF;
			IF _data_old.imagenes_url <> _data_new.imagenes_url
				THEN _datos := _datos || json_build_object('imagenes_url_anterior', _data_old.imagenes_url, 'imagenes_url_nuevo', _data_new.imagenes_url)::jsonb;
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
-- Name: comentarios; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.comentarios (
    id bigint NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    descripcion text NOT NULL,
    calificacion double precision NOT NULL,
    reportado boolean NOT NULL,
    usuario_id integer NOT NULL
);


ALTER TABLE parque.comentarios OWNER TO postgres;

--
-- Name: TABLE comentarios; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.comentarios IS 'tabla dedicada a almacenar los datos de los comentarios del sistema,tanto como de las noticias,eventos y pictogramas del parque arqueologico';


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
-- Name: estado_pqr; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.estado_pqr (
    "id " integer NOT NULL,
    nombre text NOT NULL
);


ALTER TABLE parque.estado_pqr OWNER TO postgres;

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
-- Name: informacion_parque; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.informacion_parque (
    id integer NOT NULL,
    propety text NOT NULL,
    descripcion text NOT NULL,
    imagenes_url text NOT NULL
);


ALTER TABLE parque.informacion_parque OWNER TO postgres;

--
-- Name: TABLE informacion_parque; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.informacion_parque IS 'Tabla dedicada a almacenar la informacion del parque, tal como descripcion,ubicacion, reseña historica';


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
-- Name: noticia; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.noticia (
    id integer NOT NULL,
    titulo double precision NOT NULL,
    descripcion text NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    imagenes_url text NOT NULL,
    comentarios_id text NOT NULL,
    calificacion double precision NOT NULL
);


ALTER TABLE parque.noticia OWNER TO postgres;

--
-- Name: TABLE noticia; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.noticia IS 'Tabla dedicada a almacenar los datos de las noticias del sistema';


--
-- Name: pictograma; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.pictograma (
    id integer NOT NULL,
    nombre text NOT NULL,
    calificacion double precision NOT NULL,
    imagenes_url text NOT NULL,
    descripcion text NOT NULL,
    comentarios_id text
);


ALTER TABLE parque.pictograma OWNER TO postgres;

--
-- Name: TABLE pictograma; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.pictograma IS 'Tabla dedicada a almacenar los datos de los pictogramas del parque arqueologico ';


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
-- Name: pqr; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.pqr (
    id bigint NOT NULL,
    fecha_publicacion timestamp without time zone NOT NULL,
    pregunta text NOT NULL,
    respuesta text,
    usuario_id integer NOT NULL,
    estado_id integer NOT NULL
);


ALTER TABLE parque.pqr OWNER TO postgres;

--
-- Name: TABLE pqr; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.pqr IS 'Tabla dedicada a almacenar las preguntas del sistema ';


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
-- Name: reserva; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.reserva (
    id bigint NOT NULL,
    fecha_compra timestamp without time zone NOT NULL,
    precio double precision NOT NULL,
    usuario_id integer NOT NULL,
    estado_id integer NOT NULL
);


ALTER TABLE parque.reserva OWNER TO postgres;

--
-- Name: TABLE reserva; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.reserva IS 'Tabla dedicada a almacenar los datos de las reservas del sistema, esta sera heredada por otras tablas para independizar la información correspondiente ';


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
-- Name: ticket; Type: TABLE; Schema: parque; Owner: postgres
--

CREATE TABLE parque.ticket (
    id integer NOT NULL,
    nombre text NOT NULL,
    precio double precision NOT NULL
);


ALTER TABLE parque.ticket OWNER TO postgres;

--
-- Name: TABLE ticket; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.ticket IS 'Tabla destinada a almacenar los datos de los tiquetes de los usuarios del sistema,profe si llega a ver esto pasenos';


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
    rol_id integer NOT NULL
);


ALTER TABLE parque.usuario OWNER TO postgres;

--
-- Name: TABLE usuario; Type: COMMENT; Schema: parque; Owner: postgres
--

COMMENT ON TABLE parque.usuario IS 'Tabla dedicada a almacenar los datos correspondientes a los usuarios que se registren en el sistema';


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
-- Data for Name: cabana; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.cabana (id, nombre, capacidad, precio, imagenes_url, calificacion, comentarios_id) FROM stdin;
\.
COPY parque.cabana (id, nombre, capacidad, precio, imagenes_url, calificacion, comentarios_id) FROM '$$PATH$$/2996.dat';

--
-- Data for Name: comentario_cabana; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.comentario_cabana (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, cabana_id) FROM stdin;
\.
COPY parque.comentario_cabana (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, cabana_id) FROM '$$PATH$$/2999.dat';

--
-- Data for Name: comentario_evento; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.comentario_evento (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, evento_id) FROM stdin;
\.
COPY parque.comentario_evento (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, evento_id) FROM '$$PATH$$/3000.dat';

--
-- Data for Name: comentario_noticia; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.comentario_noticia (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, noticia_id) FROM stdin;
\.
COPY parque.comentario_noticia (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, noticia_id) FROM '$$PATH$$/3001.dat';

--
-- Data for Name: comentario_pictograma; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.comentario_pictograma (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, pictograma_id) FROM stdin;
\.
COPY parque.comentario_pictograma (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id, pictograma_id) FROM '$$PATH$$/3002.dat';

--
-- Data for Name: comentarios; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.comentarios (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id) FROM stdin;
\.
COPY parque.comentarios (id, fecha_publicacion, descripcion, calificacion, reportado, usuario_id) FROM '$$PATH$$/2998.dat';

--
-- Data for Name: estado_pqr; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.estado_pqr ("id ", nombre) FROM stdin;
\.
COPY parque.estado_pqr ("id ", nombre) FROM '$$PATH$$/3006.dat';

--
-- Data for Name: estado_reserva; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.estado_reserva (id, nombre) FROM stdin;
\.
COPY parque.estado_reserva (id, nombre) FROM '$$PATH$$/3004.dat';

--
-- Data for Name: evento; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.evento (id, nombre, fecha_publicacion, descripcion, comentarios_id, calificacion, fecha, imagenes_url) FROM stdin;
\.
COPY parque.evento (id, nombre, fecha_publicacion, descripcion, comentarios_id, calificacion, fecha, imagenes_url) FROM '$$PATH$$/3007.dat';

--
-- Data for Name: informacion_parque; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.informacion_parque (id, propety, descripcion, imagenes_url) FROM stdin;
\.
COPY parque.informacion_parque (id, propety, descripcion, imagenes_url) FROM '$$PATH$$/3009.dat';

--
-- Data for Name: noticia; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.noticia (id, titulo, descripcion, fecha_publicacion, imagenes_url, comentarios_id, calificacion) FROM stdin;
\.
COPY parque.noticia (id, titulo, descripcion, fecha_publicacion, imagenes_url, comentarios_id, calificacion) FROM '$$PATH$$/3011.dat';

--
-- Data for Name: pictograma; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.pictograma (id, nombre, calificacion, imagenes_url, descripcion, comentarios_id) FROM stdin;
\.
COPY parque.pictograma (id, nombre, calificacion, imagenes_url, descripcion, comentarios_id) FROM '$$PATH$$/3012.dat';

--
-- Data for Name: pqr; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.pqr (id, fecha_publicacion, pregunta, respuesta, usuario_id, estado_id) FROM stdin;
\.
COPY parque.pqr (id, fecha_publicacion, pregunta, respuesta, usuario_id, estado_id) FROM '$$PATH$$/3014.dat';

--
-- Data for Name: reserva; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.reserva (id, fecha_compra, precio, usuario_id, estado_id) FROM stdin;
\.
COPY parque.reserva (id, fecha_compra, precio, usuario_id, estado_id) FROM '$$PATH$$/3016.dat';

--
-- Data for Name: reserva_cabana; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.reserva_cabana (id, fecha_compra, precio, usuario_id, estado_id, ticket_id) FROM stdin;
\.
COPY parque.reserva_cabana (id, fecha_compra, precio, usuario_id, estado_id, ticket_id) FROM '$$PATH$$/3017.dat';

--
-- Data for Name: reserva_ticket; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.reserva_ticket (id, fecha_compra, precio, usuario_id, estado_id, ticket_id, qr, cantidad) FROM stdin;
\.
COPY parque.reserva_ticket (id, fecha_compra, precio, usuario_id, estado_id, ticket_id, qr, cantidad) FROM '$$PATH$$/3019.dat';

--
-- Data for Name: rol; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.rol (id, nombre) FROM stdin;
\.
COPY parque.rol (id, nombre) FROM '$$PATH$$/3020.dat';

--
-- Data for Name: ticket; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.ticket (id, nombre, precio) FROM stdin;
\.
COPY parque.ticket (id, nombre, precio) FROM '$$PATH$$/3022.dat';

--
-- Data for Name: usuario; Type: TABLE DATA; Schema: parque; Owner: postgres
--

COPY parque.usuario (id, nombre, apellido, tipo_documento, numero_documento, lugar_expedicion, correo_electronico, clave, icono_url, verificacion_cuenta, estado_cuenta, rol_id) FROM stdin;
\.
COPY parque.usuario (id, nombre, apellido, tipo_documento, numero_documento, lugar_expedicion, correo_electronico, clave, icono_url, verificacion_cuenta, estado_cuenta, rol_id) FROM '$$PATH$$/3024.dat';

--
-- Data for Name: auditoria; Type: TABLE DATA; Schema: security; Owner: postgres
--

COPY security.auditoria (id, fecha, accion, schema, tabla, token, user_bd, data, pk) FROM stdin;
\.
COPY security.auditoria (id, fecha, accion, schema, tabla, token, user_bd, data, pk) FROM '$$PATH$$/3026.dat';

--
-- Name: cabana_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.cabana_id_seq', 25, true);


--
-- Name: comentarios_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.comentarios_id_seq', 2, true);


--
-- Name: estado_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.estado_id_seq', 1, false);


--
-- Name: evento_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.evento_id_seq', 5, true);


--
-- Name: informacion_parque_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.informacion_parque_id_seq', 1, false);


--
-- Name: pictograma_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.pictograma_id_seq', 1, false);


--
-- Name: pqr_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.pqr_id_seq', 1, false);


--
-- Name: reserva_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.reserva_id_seq', 1, false);


--
-- Name: rol_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.rol_id_seq', 1, false);


--
-- Name: ticket_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.ticket_id_seq', 1, false);


--
-- Name: usuario_id_seq; Type: SEQUENCE SET; Schema: parque; Owner: postgres
--

SELECT pg_catalog.setval('parque.usuario_id_seq', 1, false);


--
-- Name: auditoria_id_seq; Type: SEQUENCE SET; Schema: security; Owner: postgres
--

SELECT pg_catalog.setval('security.auditoria_id_seq', 32, true);


--
-- Name: estado_pqr ok_estado_pqr; Type: CONSTRAINT; Schema: parque; Owner: postgres
--

ALTER TABLE ONLY parque.estado_pqr
    ADD CONSTRAINT ok_estado_pqr PRIMARY KEY ("id ");


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
-- PostgreSQL database dump complete
--

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               