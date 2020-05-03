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
    [Authorize]
    public class PuntuacionController : ApiController {        

        /*
         * Autor: Jhonattan Pulido
         * Fecha creación: 29/04/2020
         * Descripción: Método que sirve para agregar un comentario y añadir una puntuación
         * Recibe: 
         * Retorna:
         * Ruta: .../puntuacion/crear?table=noticia&objectId=1
        */
        [HttpPost]        
        [Route("crear")]
        public HttpResponseMessage CrearComentario([FromUri] string table, [FromBody] UComentario comentario, [FromUri] int objectId) {

            if (table != null) {

                bool created = false;
                bool updated = false;
                double calificacion;

                switch (table) {

                    case "evento":

                        UEvento evento = new UEvento();
                        UComentarioEvento comentarioEvento = new UComentarioEvento();
                        List<UComentarioEvento> listaComentariosEvento = new List<UComentarioEvento>();

                        comentarioEvento.Id = comentario.Id;
                        comentarioEvento.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioEvento.Descripcion = comentario.Descripcion;
                        comentarioEvento.Calificacion = comentario.Calificacion;
                        comentarioEvento.LastModification = DateTime.Now;
                        comentarioEvento.Token = "Token";
                        comentarioEvento.EventoId = objectId;
                        comentarioEvento.UsuarioId = comentario.UsuarioId;
                        comentarioEvento.Reportado = false;

                        created = new LComentarioEvento().CrearComentario(comentarioEvento);

                        if (created) {

                            listaComentariosEvento = new LComentarioEvento().LeerComentariosId(comentarioEvento);
                            calificacion = listaComentariosEvento.Sum(x => x.Calificacion) / listaComentariosEvento.Count;
                            evento = new LEvento().Buscar(comentarioEvento.EventoId);
                            evento.Calificacion = Math.Floor(calificacion);
                            updated = new LEvento().Actualizar(evento.Id, evento);                            
                        }                             

                        break;

                    case "noticia":

                        UNoticia noticia = new UNoticia();
                        UComentarioNoticia comentarioNoticia = new UComentarioNoticia();
                        List<UComentarioNoticia> listaComentariosNoticia = new List<UComentarioNoticia>();

                        comentarioNoticia.Id = comentario.Id;
                        comentarioNoticia.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioNoticia.Descripcion = comentario.Descripcion;
                        comentarioNoticia.Calificacion = comentario.Calificacion;
                        comentarioNoticia.LastModification = DateTime.Now;
                        comentarioNoticia.Token = "Token";
                        comentarioNoticia.Noticia_id = objectId;
                        comentarioNoticia.UsuarioId = comentario.UsuarioId;
                        comentarioNoticia.Reportado = false;

                        created = new LComentarioNoticias().CrearComentario(comentarioNoticia);

                        if (created) {

                            listaComentariosNoticia = new LComentarioNoticias().LeerComentariosId(comentarioNoticia);
                            calificacion = listaComentariosNoticia.Sum(x => x.Calificacion) / listaComentariosNoticia.Count;
                            noticia = new LNoticia().Buscar(comentarioNoticia.Noticia_id);
                            noticia.Calificacion = Math.Floor(calificacion);
                            updated = new LNoticia().Actualizar(noticia.Id, noticia);                            
                        }                             

                        break;

                    case "pictograma":

                        UPictograma pictograma = new UPictograma();
                        UComentarioPictograma comentarioPictograma = new UComentarioPictograma();
                        List<UComentarioPictograma> listaComentariosPictograma = new List<UComentarioPictograma>();

                        comentarioPictograma.Id = comentario.Id;
                        comentarioPictograma.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioPictograma.Descripcion = comentario.Descripcion;
                        comentarioPictograma.Calificacion = comentario.Calificacion;
                        comentarioPictograma.LastModification = DateTime.Now;
                        comentarioPictograma.Token = "Token";
                        comentarioPictograma.PictogramaId = objectId;
                        comentarioPictograma.UsuarioId = comentario.UsuarioId;
                        comentarioPictograma.Reportado = false;

                        created = new LComentarioPictograma().CrearComentario(comentarioPictograma);

                        if (created) {

                            listaComentariosPictograma = new LComentarioPictograma().LeerComentariosId(comentarioPictograma);
                            calificacion = listaComentariosPictograma.Sum(x => x.Calificacion) / listaComentariosPictograma.Count;
                            pictograma = new LPictograma().Buscar(comentarioPictograma.PictogramaId);
                            pictograma.Calificacion = Math.Floor(calificacion);
                            new LPictograma().Actualizar(pictograma, pictograma.Id);
                            updated = true;
                        }                             

                        break;

                    case "cabana":

                        UCabana cabana = new UCabana();
                        UComentarioCabana comentarioCabana = new UComentarioCabana();
                        List<UComentarioCabana> listaComentariosCabana = new List<UComentarioCabana>();

                        comentarioCabana.Id = comentario.Id;
                        comentarioCabana.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioCabana.Descripcion = comentario.Descripcion;
                        comentarioCabana.Calificacion = comentario.Calificacion;
                        comentarioCabana.LastModification = DateTime.Now;
                        comentarioCabana.Token = "Token";
                        comentarioCabana.CabanaId = objectId;
                        comentarioCabana.UsuarioId = comentario.UsuarioId;
                        comentarioCabana.Reportado = false;

                        created = new LComentarioCabana().CrearComentario(comentarioCabana);

                        if (created) {

                            listaComentariosCabana = new LComentarioCabana().LeerComentariosId(comentarioCabana);
                            calificacion = listaComentariosCabana.Sum(x => x.Calificacion) / listaComentariosCabana.Count;
                            cabana = new LCabana().LeerCabana(comentarioCabana.CabanaId);
                            cabana.Calificacion = Math.Floor(calificacion);
                            updated = new LCabana().Actualizar(cabana.Id, cabana);
                        }

                        break;

                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" });
                }

                if (created && updated)
                    return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Puntuación y comentario agregado correctamente" });
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Ha ocurrido un error" });

            } else { return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" }); }
        }

        /*
         * Autor: Jhonattan Pulido
         * Fecha creación: 29/04/2020
         * Descripción: Método que sirve para leer los comentarios de un evento, noticia, pictograma etc.
         * Recibe: String table: nombre de la tabla a referenciar - Int objectId: Identificador del objeto del cual se quiere obtener los comentarios
         * Retorna: Lista de comentarios
         * Ruta: .../puntuacion/leer?table=noticia&objectId=1
        */
        [HttpGet]        
        [Route("leer")]
        public HttpResponseMessage LeerComentarios([FromUri] string table, [FromUri] int objectId) {

            if (table != null) {

                switch (table) {

                    case "evento":

                        UComentarioEvento comentarioEvento = new UComentarioEvento();
                        List<UComentarioEvento> listaComentariosEvento = new List<UComentarioEvento>();
                        comentarioEvento.EventoId = objectId;
                        listaComentariosEvento = new LComentarioEvento().LeerComentariosId(comentarioEvento);

                        return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, lista = listaComentariosEvento });

                    case "noticia":

                        UComentarioNoticia comentarioNoticia = new UComentarioNoticia();
                        List<UComentarioNoticia> listaComentariosNoticia = new List<UComentarioNoticia>();
                        comentarioNoticia.Noticia_id = objectId;
                        listaComentariosNoticia = new LComentarioNoticias().LeerComentariosId(comentarioNoticia);

                        return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, lista = listaComentariosNoticia });

                    case "pictograma":

                        UComentarioPictograma comentarioPictograma = new UComentarioPictograma();
                        List<UComentarioPictograma> listaComentariosPictograma = new List<UComentarioPictograma>();
                        comentarioPictograma.PictogramaId = objectId;
                        listaComentariosPictograma = new LComentarioPictograma().LeerComentariosId(comentarioPictograma);

                        return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, lista = listaComentariosPictograma });

                    case "cabana":

                        UComentarioCabana comentarioCabana = new UComentarioCabana();
                        List<UComentarioCabana> listaComentariosCabana = new List<UComentarioCabana>();
                        comentarioCabana.CabanaId = objectId;
                        listaComentariosCabana = new LComentarioCabana().LeerComentariosId(comentarioCabana);

                        return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, lista = listaComentariosCabana });

                    default:

                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" });
                }

            } else { return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" }); }
        }

        /*
         * Autor: Jhonattan Pulido
         * Fecha creación: 29/04/2020
         * Descripción: Método que sirve para leer los comentarios de un evento, noticia, pictograma etc de un usuario.
         * Recibe: String table: nombre de la tabla a referenciar - Int objectId: Identificador del objeto del cual se quiere obtener los comentarios - Int userId: Identificador del usuario
         * Retorna: Comentario del usuario
         * Ruta: .../puntuacion/leer-usuario?table=noticia&objectId=1&userId=1
        */
        [HttpGet]        
        [Route("leer-usuario")]
        public HttpResponseMessage LeerComentarioUsuario([FromUri] string table, [FromUri] int objectId, [FromUri] int userId) {

            if (table != null) {

                switch (table) {

                    case "evento":

                        UComentarioEvento comentarioEvento = new UComentarioEvento();                        
                        comentarioEvento.EventoId = objectId;
                        comentarioEvento.UsuarioId = userId;
                        comentarioEvento = new LComentarioEvento().LeerComentarioUsuario(comentarioEvento);

                        if (comentarioEvento != null)
                            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, comentario = comentarioEvento });
                        else
                            return Request.CreateResponse(HttpStatusCode.OK, new { ok = false });

                    case "noticia":

                        UComentarioNoticia comentarioNoticia = new UComentarioNoticia();                        
                        comentarioNoticia.Noticia_id = objectId;
                        comentarioNoticia.UsuarioId = userId;
                        comentarioNoticia = new LComentarioNoticias().LeerComentarioUsuario(comentarioNoticia);

                        if (comentarioNoticia != null)
                            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, comentario = comentarioNoticia });
                        else
                            return Request.CreateResponse(HttpStatusCode.OK, new { ok = false });

                    case "pictograma":

                        UComentarioPictograma comentarioPictograma = new UComentarioPictograma();                        
                        comentarioPictograma.PictogramaId = objectId;
                        comentarioPictograma.UsuarioId = userId;
                        comentarioPictograma = new LComentarioPictograma().LeerComentarioUsuario(comentarioPictograma);

                        if (comentarioPictograma != null)
                            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, comentario = comentarioPictograma });
                        else
                            return Request.CreateResponse(HttpStatusCode.OK, new { ok = false });

                    case "cabana":

                        UComentarioCabana comentarioCabana = new UComentarioCabana();                        
                        comentarioCabana.CabanaId = objectId;
                        comentarioCabana.UsuarioId = userId;
                        comentarioCabana = new LComentarioCabana().LeerComentarioUsuario(comentarioCabana);

                        if (comentarioCabana != null)
                            return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, comentario = comentarioCabana });
                        else
                            return Request.CreateResponse(HttpStatusCode.OK, new { ok = false });

                    default:

                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" });
                }

            } else { return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" }); }
        }

        /*
         * Autor: Jhonattan Pulido
         * Fecha creación: 29/04/2020
         * Descripción: Método que sirve para actualizar un comentario y/o una puntuación
         * Recibe: 
         * Retorna:
         * Ruta: .../puntuacion/crear?table=noticia&objectId=1
        */
        [HttpPost]
        [Route("actualizar")]
        public HttpResponseMessage ActualizarComentario([FromUri] string table, [FromBody] UComentario comentario, [FromUri] int objectId) {
            
            if (table != null) {

                bool created = false;
                bool updated = false;
                double calificacion;

                switch (table) {

                    case "evento":

                        UEvento evento = new UEvento();
                        UComentarioEvento comentarioEvento = new UComentarioEvento();
                        List<UComentarioEvento> listaComentariosEvento = new List<UComentarioEvento>();

                        comentarioEvento.Id = comentario.Id;
                        comentarioEvento.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioEvento.Descripcion = comentario.Descripcion;
                        comentarioEvento.Calificacion = comentario.Calificacion;
                        comentarioEvento.LastModification = DateTime.Now;
                        comentarioEvento.Token = "Token";
                        comentarioEvento.EventoId = objectId;
                        comentarioEvento.UsuarioId = comentario.UsuarioId;
                        comentarioEvento.Reportado = false;

                        created = new LComentarioEvento().ActualizarComentario(comentarioEvento);

                        if (created) {

                            listaComentariosEvento = new LComentarioEvento().LeerComentariosId(comentarioEvento);
                            calificacion = listaComentariosEvento.Sum(x => x.Calificacion) / listaComentariosEvento.Count;
                            evento = new LEvento().Buscar(comentarioEvento.EventoId);
                            evento.Calificacion = Math.Floor(calificacion);
                            updated = new LEvento().Actualizar(evento.Id, evento);
                        }                             

                        break;

                    case "noticia":

                        UNoticia noticia = new UNoticia();
                        UComentarioNoticia comentarioNoticia = new UComentarioNoticia();
                        List<UComentarioNoticia> listaComentariosNoticia = new List<UComentarioNoticia>();

                        comentarioNoticia.Id = comentario.Id;
                        comentarioNoticia.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioNoticia.Descripcion = comentario.Descripcion;
                        comentarioNoticia.Calificacion = comentario.Calificacion;
                        comentarioNoticia.LastModification = DateTime.Now;
                        comentarioNoticia.Token = "Token";
                        comentarioNoticia.Noticia_id = objectId;
                        comentarioNoticia.UsuarioId = comentario.UsuarioId;
                        comentarioNoticia.Reportado = false;

                        created = new LComentarioNoticias().ActualizarComentario(comentarioNoticia);

                        if (created) {

                            listaComentariosNoticia = new LComentarioNoticias().LeerComentariosId(comentarioNoticia);
                            calificacion = listaComentariosNoticia.Sum(x => x.Calificacion) / listaComentariosNoticia.Count;
                            noticia = new LNoticia().Buscar(comentarioNoticia.Noticia_id);
                            noticia.Calificacion = Math.Floor(calificacion);
                            updated = new LNoticia().Actualizar(noticia.Id, noticia);                            
                        }                             

                        break;

                    case "pictograma":

                        UPictograma pictograma = new UPictograma();
                        UComentarioPictograma comentarioPictograma = new UComentarioPictograma();
                        List<UComentarioPictograma> listaComentariosPictograma = new List<UComentarioPictograma>();

                        comentarioPictograma.Id = comentario.Id;
                        comentarioPictograma.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioPictograma.Descripcion = comentario.Descripcion;
                        comentarioPictograma.Calificacion = comentario.Calificacion;
                        comentarioPictograma.LastModification = DateTime.Now;
                        comentarioPictograma.Token = "Token";
                        comentarioPictograma.PictogramaId = objectId;
                        comentarioPictograma.UsuarioId = comentario.UsuarioId;
                        comentarioPictograma.Reportado = false;

                        created = new LComentarioPictograma().ActualizarComentario(comentarioPictograma);

                        if (created) {

                            listaComentariosPictograma = new LComentarioPictograma().LeerComentariosId(comentarioPictograma);
                            calificacion = listaComentariosPictograma.Sum(x => x.Calificacion) / listaComentariosPictograma.Count;
                            pictograma = new LPictograma().Buscar(comentarioPictograma.PictogramaId);
                            pictograma.Calificacion = Math.Floor(calificacion);
                            new LPictograma().Actualizar(pictograma, pictograma.Id);
                            updated = true;
                        }                             

                        break;

                    case "cabana":

                        UCabana cabana = new UCabana();
                        UComentarioCabana comentarioCabana = new UComentarioCabana();
                        List<UComentarioCabana> listaComentariosCabana = new List<UComentarioCabana>();

                        comentarioCabana.Id = comentario.Id;
                        comentarioCabana.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioCabana.Descripcion = comentario.Descripcion;
                        comentarioCabana.Calificacion = comentario.Calificacion;
                        comentarioCabana.LastModification = DateTime.Now;
                        comentarioCabana.Token = "Token";
                        comentarioCabana.CabanaId = objectId;
                        comentarioCabana.UsuarioId = comentario.UsuarioId;
                        comentarioCabana.Reportado = false;

                        created = new LComentarioCabana().ActualizarComentario(comentarioCabana);

                        if (created) {

                            listaComentariosCabana = new LComentarioCabana().LeerComentariosId(comentarioCabana);
                            calificacion = listaComentariosCabana.Sum(x => x.Calificacion) / listaComentariosCabana.Count;
                            cabana = new LCabana().LeerCabana(comentarioCabana.CabanaId);
                            cabana.Calificacion = Math.Floor(calificacion);
                            updated = new LCabana().Actualizar(cabana.Id, cabana);
                        }

                        break;

                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" });
                }

                if (created && updated)
                    return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Puntuación y comentario actualizado correctamente" });
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Ha ocurrido un error" });

            } else { return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" }); }
        }

        /*
         * Autor: Jhonattan Pulido
         * Fecha creación: 29/04/2020
         * Descripción: Método que sirve para actualizar un comentario y/o una puntuación
         * Recibe: 
         * Retorna:
         * Ruta: .../puntuacion/crear?table=noticia&objectId=1
        */
        [HttpGet]
        [Route("borrar")]
        public HttpResponseMessage BorrarComentario([FromUri] string table, [FromUri] int objectId, [FromUri] long id) {

            if (table != null) {
                
                bool updated = false;
                bool deleted = false;
                double calificacion;

                switch (table) {

                    case "evento":

                        UEvento evento = new UEvento();
                        UComentarioEvento comentarioEvento = new UComentarioEvento();
                        List<UComentarioEvento> listaComentariosEvento = new List<UComentarioEvento>();
                        comentarioEvento.Id = id;
                        comentarioEvento.EventoId = objectId;

                        deleted = new LComentarioEvento().BorrarComentario(id);

                        if (deleted) {

                            listaComentariosEvento = new LComentarioEvento().LeerComentariosId(comentarioEvento);
                            calificacion = listaComentariosEvento.Sum(x => x.Calificacion) / listaComentariosEvento.Count;
                            evento = new LEvento().Buscar(comentarioEvento.EventoId);
                            evento.Calificacion = Math.Floor(calificacion);
                            updated = new LEvento().Actualizar(evento.Id, evento);
                        }                             

                        break;

                    case "noticia":

                        UNoticia noticia = new UNoticia();
                        UComentarioNoticia comentarioNoticia = new UComentarioNoticia();
                        List<UComentarioNoticia> listaComentariosNoticia = new List<UComentarioNoticia>();
                        comentarioNoticia.Id = id;
                        comentarioNoticia.Noticia_id = objectId;

                        deleted = new LComentarioNoticias().BorrarComentario(id);

                        if (deleted) {

                            listaComentariosNoticia = new LComentarioNoticias().LeerComentariosId(comentarioNoticia);
                            calificacion = listaComentariosNoticia.Sum(x => x.Calificacion) / listaComentariosNoticia.Count;
                            noticia = new LNoticia().Buscar(comentarioNoticia.Noticia_id);
                            noticia.Calificacion = Math.Floor(calificacion);
                            updated = new LNoticia().Actualizar(noticia.Id, noticia);                            
                        }                             

                        break;

                    case "pictograma":

                        UPictograma pictograma = new UPictograma();
                        UComentarioPictograma comentarioPictograma = new UComentarioPictograma();
                        List<UComentarioPictograma> listaComentariosPictograma = new List<UComentarioPictograma>();
                        comentarioPictograma.Id = id;
                        comentarioPictograma.PictogramaId = objectId;

                        deleted = new LComentarioPictograma().BorrarComentario(id);

                        if (deleted) {

                            listaComentariosPictograma = new LComentarioPictograma().LeerComentariosId(comentarioPictograma);
                            calificacion = listaComentariosPictograma.Sum(x => x.Calificacion) / listaComentariosPictograma.Count;
                            pictograma = new LPictograma().Buscar(comentarioPictograma.PictogramaId);
                            pictograma.Calificacion = Math.Floor(calificacion);
                            new LPictograma().Actualizar(pictograma, pictograma.Id);
                            updated = true;
                        }                             

                        break;

                    case "cabana":

                        UCabana cabana = new UCabana();
                        UComentarioCabana comentarioCabana = new UComentarioCabana();
                        List<UComentarioCabana> listaComentariosCabana = new List<UComentarioCabana>();
                        comentarioCabana.Id = id;
                        comentarioCabana.CabanaId = objectId;

                        deleted = new LComentarioCabana().BorrarComentario(id);

                        if (deleted) {

                            listaComentariosCabana = new LComentarioCabana().LeerComentariosId(comentarioCabana);
                            calificacion = listaComentariosCabana.Sum(x => x.Calificacion) / listaComentariosCabana.Count;
                            cabana = new LCabana().LeerCabana(comentarioCabana.CabanaId);
                            cabana.Calificacion = Math.Floor(calificacion);
                            updated = new LCabana().Actualizar(cabana.Id, cabana);
                        }

                        break;

                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" });
                }

                if (deleted && updated)
                    return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Puntuación y comentario eliminado correctamente" });
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Ha ocurrido un error" });

            } else { return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" }); }            
        }

        /*
         * Autor: Jhonattan Pulido
         * Fecha creación: 29/04/2020
         * Descripción: Método que sirve para agregar un comentario y añadir una puntuación
         * Recibe: 
         * Retorna:
         * Ruta: .../puntuacion/crear?table=noticia&objectId=1
        */
        [HttpPost]
        [Route("reportar")]
        public HttpResponseMessage ReportarComentario([FromUri] string table, [FromBody] UComentario comentario, [FromUri] int objectId) {

            if (table != null) {

                bool created = false;                              

                switch (table) {

                    case "evento":

                        UEvento evento = new UEvento();
                        UComentarioEvento comentarioEvento = new UComentarioEvento();
                        List<UComentarioEvento> listaComentariosEvento = new List<UComentarioEvento>();

                        comentarioEvento.Id = comentario.Id;
                        comentarioEvento.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioEvento.Descripcion = comentario.Descripcion;
                        comentarioEvento.Calificacion = comentario.Calificacion;
                        comentarioEvento.LastModification = DateTime.Now;
                        comentarioEvento.Token = "Token";
                        comentarioEvento.EventoId = objectId;
                        comentarioEvento.UsuarioId = comentario.UsuarioId;
                        comentarioEvento.Reportado = true;

                        created = new LComentarioEvento().ActualizarComentario(comentarioEvento);                                                  

                        break;

                    case "noticia":

                        UNoticia noticia = new UNoticia();
                        UComentarioNoticia comentarioNoticia = new UComentarioNoticia();
                        List<UComentarioNoticia> listaComentariosNoticia = new List<UComentarioNoticia>();

                        comentarioNoticia.Id = comentario.Id;
                        comentarioNoticia.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioNoticia.Descripcion = comentario.Descripcion;
                        comentarioNoticia.Calificacion = comentario.Calificacion;
                        comentarioNoticia.LastModification = DateTime.Now;
                        comentarioNoticia.Token = "Token";
                        comentarioNoticia.Noticia_id = objectId;
                        comentarioNoticia.UsuarioId = comentario.UsuarioId;
                        comentarioNoticia.Reportado = true;

                        created = new LComentarioNoticias().ActualizarComentario(comentarioNoticia);                        

                        break;

                    case "pictograma":

                        UPictograma pictograma = new UPictograma();
                        UComentarioPictograma comentarioPictograma = new UComentarioPictograma();
                        List<UComentarioPictograma> listaComentariosPictograma = new List<UComentarioPictograma>();

                        comentarioPictograma.Id = comentario.Id;
                        comentarioPictograma.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioPictograma.Descripcion = comentario.Descripcion;
                        comentarioPictograma.Calificacion = comentario.Calificacion;
                        comentarioPictograma.LastModification = DateTime.Now;
                        comentarioPictograma.Token = "Token";
                        comentarioPictograma.PictogramaId = objectId;
                        comentarioPictograma.UsuarioId = comentario.UsuarioId;
                        comentarioPictograma.Reportado = true;

                        created = new LComentarioPictograma().ActualizarComentario(comentarioPictograma);                        

                        break;

                    case "cabana":

                        UCabana cabana = new UCabana();
                        UComentarioCabana comentarioCabana = new UComentarioCabana();
                        List<UComentarioCabana> listaComentariosCabana = new List<UComentarioCabana>();

                        comentarioCabana.Id = comentario.Id;
                        comentarioCabana.FechaPublicacion = comentario.FechaPublicacion;
                        comentarioCabana.Descripcion = comentario.Descripcion;
                        comentarioCabana.Calificacion = comentario.Calificacion;
                        comentarioCabana.LastModification = DateTime.Now;
                        comentarioCabana.Token = "Token";
                        comentarioCabana.CabanaId = objectId;
                        comentarioCabana.UsuarioId = comentario.UsuarioId;
                        comentarioCabana.Reportado = true;

                        created = new LComentarioCabana().ActualizarComentario(comentarioCabana);                        

                        break;

                    default:
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" });
                }

                if (created)
                    return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Comentario reportado" });
                else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Ha ocurrido un error" });

            } else { return Request.CreateResponse(HttpStatusCode.BadRequest, new { ok = false, message = "ERROR: Referencia a tabla desconocida" }); }
        }
    }
}
