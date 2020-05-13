using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;
using Data;

namespace Logica
{
    public class Lpush{


        public bool insertarPush(UPush push) {
            return new DaoPush().insertarPush(push);
        }


        public List<UPush> obtenerTokens()
        {
            return new DaoPush().Tokensnotificaciones().ToList();
        }


        public string tokenUser(int id) {

            return new  DaoPush().obtenerTokenUsuario(id);
        }


        public void SendNotificationPrueba()
        {
            try
            {
                string keyService = "AAAAOwkpcno:APA91bFBQ-bWf461MVHDS8bpMDA0SFVUaQmkaqJMu_WtkXMJu3crJ68bmyGrLtYMKEed1RKaNtJWhDLfPDk9FbmyZ0WjDUbxiTVTAWL5gsiOX6OpoCW4MlrEn0LSCkxe6yEbKKJ-6pqG";
                string SENDER_ID = "253556781690";
                string idDevice = "e2pAY_ok9T8:APA91bEjU-DovVgumiQ54Dp5j1_i5ZAlNi2KsXKASytvMxUANPl5fLHWLFr0s1iYJxD0mYfRwQBAkNiL1gRWGWu6Sm7RmcpBVQ13TMIyeinWIwXuTpPtcDDMpBc1NpsijkcjalumKXYx";
                string menssage = "El parque vuelve a cobrar por que el COVID SE ACABO";
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                //serverKey - Key from Firebase cloud messaging server  
                tRequest.Headers.Add(string.Format("Authorization: key={0}", keyService));
                //Sender Id - From firebase project setting  
                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                tRequest.ContentType = "application/json";
                var payload = new
                {
                    to = idDevice,
                    priority = "high",
                    content_available = true,
                    time_to_live = 5000,
                    notification = new
                    {
                        body = menssage,
                        title = "Test",
                        badge = 1,
                        sound = "default",
                        //click_action="FCM_PLUGIN_ACTIVITY",
                        icon = "favicon"
                    },
                    data = new
                    {
                        tipo = "Noticia"
                    }
                };
                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void SendNotification(UNotificacion notificacion)
        {
            try{
                string keyService = "AAAAOwkpcno:APA91bFBQ-bWf461MVHDS8bpMDA0SFVUaQmkaqJMu_WtkXMJu3crJ68bmyGrLtYMKEed1RKaNtJWhDLfPDk9FbmyZ0WjDUbxiTVTAWL5gsiOX6OpoCW4MlrEn0LSCkxe6yEbKKJ-6pqG";
                string SENDER_ID = "253556781690";
                string idDevice = notificacion.TokenId;
                UMensajeNotificacion mensajeNotificacion = JsonConvert.DeserializeObject<UMensajeNotificacion>(notificacion.Informacion);
                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                //serverKey - Key from Firebase cloud messaging server  
                tRequest.Headers.Add(string.Format("Authorization: key={0}", keyService));
                //Sender Id - From firebase project setting  
                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                tRequest.ContentType = "application/json";
                var payload = new
                {
                    to = idDevice,
                    priority = "high",
                    content_available = true,
                    time_to_live= 5000,
                    notification = new
                    {
                        body = mensajeNotificacion.Descripcion,
                        title = mensajeNotificacion.Titulo,
                        badge = 1,
                        sound = "default"
                        //click-action="FMC_PLUGIN_ACTIVITY"
                    },
                    data = new
                    {
                        tipo = mensajeNotificacion.Tipo
                    }
                };
                string postbody = JsonConvert.SerializeObject(payload).ToString();
                Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
                tRequest.ContentLength = byteArray.Length;
                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    using (WebResponse tResponse = tRequest.GetResponse())
                    {
                        using (Stream dataStreamResponse = tResponse.GetResponseStream())
                        {
                            if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
    
}

