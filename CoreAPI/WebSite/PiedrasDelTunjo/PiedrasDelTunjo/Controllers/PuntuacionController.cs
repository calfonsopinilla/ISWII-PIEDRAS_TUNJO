using Data;
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;

namespace PiedrasDelTunjo.Controllers {

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("puntuacion")]
    public class PuntuacionController : ApiController {

        // Variables
        private int contador;
        private List<UPuntuacion> listaPuntuaciones;
        private UPictograma pictograma;
        private UEvento evento;
        private UNoticia noticia;
        private UCabana cabana;

        /*
            Autor: Jhonattan Alejandro Pulido Arenas
            Fecha creación: 24/03/2020
            Descripción: Método que sirve para leer las puntuaciones de un objeto insertado en una tabla
            Recibe: UPuntuacion puntuacion - Objeto con los atributos a guardar
            Retorna: Lista de puntuaciones
        */
        [HttpGet]
        [Route("calcular")]
        public HttpResponseMessage EnviarTokenCorreo([FromUri] UPuntuacion puntuacion) {

            this.listaPuntuaciones = new LPuntuacion().LeerPuntuacionesObjeto(puntuacion.Puntero, puntuacion.PunteroId);
            if (this.listaPuntuaciones.Count > 0) {

                this.contador = puntuacion.Puntuacion;

                foreach (UPuntuacion punt in this.listaPuntuaciones)
                    this.contador += punt.Puntuacion;

                this.contador = this.contador / (this.listaPuntuaciones.Count + 1);

                new DAOPuntuacion().AgregarPuntuacion(puntuacion);

                switch (puntuacion.Puntero) {

                    // Tabla Pictograma
                    case 1:

                        break;

                    // Tabla Evento
                    case 2:
                        this.evento = new DaoEvento().Buscar(puntuacion.PunteroId);
                        this.evento.Calificacion = this.contador;
                        new DaoEvento().Actualizar(this.evento.Id, this.evento);
                        break;

                    // Tabla Noticia
                    case 3:

                        break;

                    // Cabana
                    case 4:
                        this.cabana = new DAOCabana().LeerCabana(puntuacion.PunteroId);
                        this.cabana.Calificacion = this.contador;
                        new DAOCabana().ActualizarCabana(this.cabana);
                        break;

                    default:
                        return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "ERROR: Ha ocurrido un problema con el servidor" });
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "" });

            } else if (this.listaPuntuaciones == null || this.listaPuntuaciones.Count == 0) {

                new DAOPuntuacion().AgregarPuntuacion(puntuacion);

                switch (puntuacion.Puntero) {

                    // Tabla Pictograma
                    case 1:

                        break;

                    // Tabla Evento
                    case 2:
                        this.evento = new DaoEvento().Buscar(puntuacion.PunteroId);
                        this.evento.Calificacion = this.contador;
                        new DaoEvento().Actualizar(this.evento.Id, this.evento);
                        break;

                    // Tabla Noticia
                    case 3:

                        break;

                    // Cabana
                    case 4:
                        this.cabana = new DAOCabana().LeerCabana(puntuacion.PunteroId);
                        this.cabana.Calificacion = this.contador;
                        new DAOCabana().ActualizarCabana(this.cabana);
                        break;

                    default:
                        return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "ERROR: Ha ocurrido un problema con el servidor" });
                }

                return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "" });

            } else
                return Request.CreateResponse(HttpStatusCode.NotFound, new { ok = false, message = "ERROR: Ha ocurrido un problema con el servidor" });
        }        
    }
}
