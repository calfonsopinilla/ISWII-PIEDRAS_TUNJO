using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    }
}
