using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
namespace Logica
{
   public class LPreguntas_frecuentes
    {
        public IEnumerable<UPreguntas_frecuentes> ObtenerPreguntasFrecuentes()
        {
            return new DaoPreguntas_frecuentes().ObtenerPreguntasFrecuentes();
        }

        public UPreguntas_frecuentes Buscar(int id)
        {
            return new DaoPreguntas_frecuentes().Buscar(id);
        }

        public bool Agregar(UPreguntas_frecuentes preguntas_frecuentes)
        {
            return new DaoPreguntas_frecuentes().Agregar(preguntas_frecuentes);
        }

        public bool Actualizar(int id, UPreguntas_frecuentes preguntas_frecuentes)
        {
            return new DaoPreguntas_frecuentes().Actualizar(id, preguntas_frecuentes);
        }

        public bool Eliminar(int id)
        {
            return new DaoPreguntas_frecuentes().Eliminar(id);
        }
    }
}
