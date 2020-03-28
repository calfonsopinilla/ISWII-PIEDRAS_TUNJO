using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    [Serializable] // Se declara que la clase U Usuario se puede expresar en formato JSON
    [Table("reserva_cabana", Schema = "parque")] // Se específica la tabla con la que se relaciona la clase U Usuario
    class UReservaCabana : UReservaTicket {

    }
}
