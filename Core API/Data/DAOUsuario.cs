﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data {

    public class DAOUsuario {

        private DAOMapeo conexionBD;

        /*
            Autor: Jhonattan Pulido
            Fecha modificacion: 8/03/2020
        */
        public bool CrearUsuario(UUsuario usuario) {

            using (this.conexionBD = new DAOMapeo()) {

                this.conexionBD.Usuarios.Add(usuario);
                this.conexionBD.SaveChanges();
            }

            return true;
        }        
    }
}
