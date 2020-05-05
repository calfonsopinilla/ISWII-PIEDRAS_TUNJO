using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Data;
using Utilitarios;



namespace Logica
{
    public class LEnviarNotificaciones {
        public void enviarNotificaciones() {

            DaoNotificacion notificacion = new DaoNotificacion();
            List<UNotificacion> listaNoficaciones = notificacion.obtenerNotificaciones();
            Lpush push = new Lpush();

            if (listaNoficaciones.Count != 0) {
                foreach (var item in listaNoficaciones) {
                    push.SendNotification(item);
                    notificacion.cambiarEstadoNotifiacion(item.Id);
                }
            }
        }
        //llamar hilo con el relog 
        public void activarEnvioNotificaciones(){
            Thread hilo = new Thread(enviarNotificaciones);
            hilo.Start();

        }






    }
}
