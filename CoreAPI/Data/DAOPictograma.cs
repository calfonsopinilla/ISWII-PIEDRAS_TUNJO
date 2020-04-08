using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Data
{
    public class DAOPictograma
    {
        /**
       * Autor: Mary Zapata
       * Fecha: 19/03/2020
       * Parametro de recepcion: int id a buscar y string nombre a buscar
       * return string busqueda
       **/
        //
        
        private readonly Mapeo db = new Mapeo();
        private Mapeo conexionBD;
        private UPictograma pictograma;
        private List<UPictograma> listapictogramas;
        public string Valida_ExistenciaPictograma(int id_parque, string nombre)
        {
            string busqueda = "";

            using (var db = new Mapeo())
            {
                //
                if (db.Pictograma.Where(x => x.Id_parque == id_parque).FirstOrDefault() != null && (db.Pictograma.Where(x => x.Nombre == nombre).FirstOrDefault() != null))
                {
                    busqueda = "Ya exite un pictograma con el mismo nombre y el mismo id, eliminelo o actualicelo";

                }
                else if (db.Pictograma.Where(x => x.Id_parque == id_parque).FirstOrDefault() != null)
                {
                    busqueda = "Ya exite un pictograma con el mismo id, eliminelo o actualicelo";

                }
                else if (db.Pictograma.Where(x => x.Nombre == nombre).FirstOrDefault() != null)
                {
                    busqueda = "Ya exite un pictograma con el mismo nombre, eliminelo o actualicelo";
                }
                else
                {
                    busqueda = "El pictograma no ha sido creado";
                }

            }

            return busqueda;
        }

        /**
         * Autor: Mary Zapata
         * Fecha: 19/03/2020
         * Metodo para insertar un nuevo pictograma en base de datos en la tabla pictograma
         * Parametro de recepcion: objeto UPictograma para insertar en la bd
         * return: void
         **/
        public void RegistroPictograma(UPictograma datosInsertar)
        {

            using (var db = new Mapeo())
            {

                try
                {
                    db.Pictograma.Add(datosInsertar);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }
        }

        /**
         * Autor: Mary Zapata
         * Fecha: 19/03/2020
         * Metodo para consultar los pictogramas en base de datos en la tabla pictograma         
         * return: List<UPictograma> listaPic
         **/
        public List<UPictograma> Mostrar_Pictograma(int estadoFiltro)
        {

            using (var db = new Mapeo())
            {
                try
                {

                    return db.Pictograma.Where(x => x.Estado == estadoFiltro).ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public List<UComentarioPic> BuscarCalificaciones()
        {

            using (var db = new Mapeo())
            {
                try
                {

                    return db.ComentarioPic.ToList();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /**
       * Autor: Mary Zapata
       * Fecha: 20/03/2020
       * Metodo para editar los pictogramas en base de datos en la tabla pictograma        
       * return: void
       **/
        public void EditarPictograma(UPictograma infoNueva)
        {
            using (var db = new Mapeo())
            {
                UPictograma edit = db.Pictograma.Where(x => x.Id == infoNueva.Id).First();

                edit.Nombre = infoNueva.Nombre;
                edit.Descripcion = infoNueva.Descripcion;
                edit.Imagenes_url = infoNueva.Imagenes_url;
                edit.Calificacion = infoNueva.Calificacion;
                db.Pictograma.Attach(edit);
                var entry = db.Entry(edit);
                entry.State = EntityState.Modified;
                db.SaveChanges();
            }

        }
        public UPictograma LeerPictograma(int pictogramaId)
        {

            try
            {

                this.pictograma = new UPictograma();

                using (this.conexionBD = new Mapeo())
                {

                    this.pictograma = this.conexionBD.Pictograma.Find(pictogramaId);
                    return this.pictograma;
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /**
         * Autor: Mary Zapata
         * Fecha: 20/03/2020
         * Metodo para editar el estado del pictograma en base de datos en la tabla pictograma        
         * return: void
         **/
        public void CambiarEstado_Pictograma(int id_pictograma)
        {
            using (var db = new Mapeo())
            {
                var subs = db.Pictograma.Where(x => x.Id == id_pictograma).FirstOrDefault();

                if (subs.Estado == 2)
                {
                    subs.Estado = 1;
                }
                else
                {
                    subs.Estado = 2;
                }
                db.SaveChanges();
            }
        }

        public bool Actualizar(int id, UPictograma pictograma)
        {
            try
            {

                db.Entry(pictograma).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Existe(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }
        public bool Existe(int id)
        {
            return db.Pictograma.Any(x => x.Id == id);
        }
    }
}
