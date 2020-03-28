using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Logica
{
    public class LPuntoInteres
    {
        public IEnumerable<UPuntoInteres> ObtenerTodos()
        {
            return new DaoPuntoInteres().ObtenerTodos();
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: UPuntoInteres puntoInteres - Objeto con los atributos a guardar
            Retorna: Booleano
        */
        public bool CrearPuntoInteres(UPuntoInteres puntoInteres) {

            return new DaoPuntoInteres().CrearPuntoInteres(puntoInteres);
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: Integer id - Id del punto de interes que se desea buscar
            Retorna: Objeto de tipo UPuntoInteres con los datos filtrados
        */
        public UPuntoInteres LeerPuntoInteres(int id) {

            return new DaoPuntoInteres().LeerPuntoInteres(id);
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: Integer id - Id del punto de interes que se desea buscar
            Retorna: Booleano definiendo si la elmininacion se hizo satisfactoriamente
        */
        public bool BorrarPuntoInteres(int id) {

            return new DaoPuntoInteres().BorrarPuntoInteres(id);
        }

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 26/03/2020
            Descripción: Método que sirve para guardar un punto de interes en la tabla puntos interes
            Recibe: UPuntoInteres puntoInteres - Objeto con los atributos a actualizar
            Retorna: Booleano
        */
        public bool ActualizarPuntoInteres(UPuntoInteres puntoInteres) {

            return new DaoPuntoInteres().ActualizarPuntoInteres(puntoInteres);
        }
        public bool Actualizar(int id, UPuntoInteres puntoInteres)
        {
            return new DaoPuntoInteres().Actualizar(id, puntoInteres);
        }
    }
}
