using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Utilitarios {

    /*
        Autor: Jhonattan Alejandro Pulido Arenas
        Fecha creación: 24/03/2020
        Descripción: Clase que sirve para definir el cascaron de la tabla puntuación
    */
    [Serializable]
    [Table("puntuacion", Schema = "parque")]
    public class UPuntuacion {

        // Variables
        private long id; // Id
        private int puntuacion; // Puntuación que el usuario ha seleccionado
        private int puntero; // Id de la tabla a la que apunta la puntuación
        private int punteroId; // Id del objeto al que se esta apuntando

        [Key]
        [Column("id")]
        public long Id { get => id; set => id = value; }
        [Column("puntuacion")]
        public int Puntuacion { get => puntuacion; set => puntuacion = value; }
        [Column("puntero")]
        public int Puntero { get => puntero; set => puntero = value; }
        [Column("puntero_id")]
        public int PunteroId { get => punteroId; set => punteroId = value; }
    }
}
