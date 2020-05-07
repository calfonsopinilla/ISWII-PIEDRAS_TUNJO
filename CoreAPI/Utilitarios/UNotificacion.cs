﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilitarios
{

    [Table("notificaciones", Schema = "pushed")] // Se específica la tabla con la que se relaciona la clase U Usuario
    public class UNotificacion{

        private long id;
        private string tokenId;
        private string informacion;
        private bool estado;
        private UMensajeNotificacion mensajeNotificacion;

        [Key]
        [Column("id")]
        public long Id { get => id; set => id = value; }
        [Column("token_id")]
        public string TokenId { get => tokenId; set => tokenId = value; }
        [Column("informacion")]
        public string Informacion { get => informacion; set => informacion = value; }
        [Column("estado")]
        public bool Estado { get => estado; set => estado = value; }
        [NotMapped]
        public UMensajeNotificacion MensajeNotificacion { get => mensajeNotificacion; set => mensajeNotificacion = value; }
        
    }
}