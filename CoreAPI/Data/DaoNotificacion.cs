using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data
{
    public class DaoNotificacion
    {

        public void GenerarNotificaciones(string titulo,string tipo,string descripcion) {
            DataTable agregar = new DataTable();
            NpgsqlConnection conection = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgresConnection"].ConnectionString);
            try {
                NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter("pushed.f_crear_notificaciones", conection);
                dataAdapter.SelectCommand.Parameters.Add("_titulo", NpgsqlDbType.Text).Value = titulo;
                dataAdapter.SelectCommand.Parameters.Add("_descripcion", NpgsqlDbType.Text).Value = descripcion;
                dataAdapter.SelectCommand.Parameters.Add("_tipo", NpgsqlDbType.Text).Value = tipo;
                dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                conection.Open();
                dataAdapter.Fill(agregar);
                conection.Close();
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                if (conection != null)
                {
                    conection.Close();
                }
            }
            
        }

        private readonly Mapeo db = new Mapeo();

        public List<UNotificacion> obtenerNotificaciones() {
            try{
                return db.Notificacion.Where(x => x.Estado == true).Take(50).ToList();
                
            }
            catch (Exception ex) {
                throw ex;
            }
            
        }

        public void cambiarEstadoNotifiacion(long id) {

            try {
                var notificacion = db.Notificacion.Find(id);
                notificacion.Estado = false;
                db.SaveChanges();
            } catch (Exception ex) { throw ex; }
        }
        public void eliminarNotificacion(long id) {
            try  {
                var notificacion = db.Notificacion.Find(id);
                db.Notificacion.Remove(notificacion);
                db.SaveChanges();
            }
            catch (Exception ex) { throw ex;}
        }


    }
}
