using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;
using Newtonsoft.Json;

namespace Logica
{
    /**
         * Autor: Mary Zapata
         * fecha: 19/03/2019
         * Parametro de recepcion: json tipo UPictograma
         * Return: string estado registro del pictograma
      **/
    public class LPictograma
    {
        public object Agregar(UPictograma pictograma)
        {
            string messageError = string.Empty;
            bool created = false;
            try
            {
                var existe = new DAOPictograma().ExistePorNombre(pictograma.Nombre);
                if (existe)
                {
                    messageError = "El nombre de pictograma ya existe.";
                } else
                {
                    created = new DAOPictograma().Agregar(pictograma);
                }
            }catch(Exception ex)
            {
                messageError = ex.Message;
            }
            return new { ok = created, message = messageError };
        }

        public IEnumerable<UPictograma> ObtenerTodos()
        {
            return new DAOPictograma().ObtenerTodos();
        }

        public object Actualizar(UPictograma pic, int id)
        {
            string messageError = string.Empty;
            bool updated = false;
            try
            {
                var existe = new DAOPictograma().ExistePorNombre(pic.Nombre, true);
                if (existe)
                {
                    messageError = "El nombre de pictograma ya existe.";
                }
                else
                {
                    updated = new DAOPictograma().Actualizar(pic, id);
                }
            }
            catch (Exception ex)
            {
                messageError = ex.Message;
            }
            return new { ok = updated, message = messageError };
        }

        public UPictograma Buscar(int id)
        {
            return new DAOPictograma().Buscar(id);
        }

        public bool Eliminar(int id)
        {
            return new DAOPictograma().Eliminar(id);
        }
    }
}
