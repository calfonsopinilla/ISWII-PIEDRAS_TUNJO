using Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Logica {

    /*
        Author: Jhonattan Pulido
        Creation Date: 17/04/2020
        Description: Controller created for generate payments with Stripe
    */
    public class LStripe {

        /*
            Author: Jhonattan Pulido
            Creation Date: 17/04/2020
            Description: This method initialize the stripe config
            Parameters: UStripe customerInfo: Object with the credential information declared
            Returns: The var "customerInfo" with the secret API key & public API key declared
        */
        public UStripe InitConfig(UStripe customerInfo) { return new DAOStripe().InitConfig(customerInfo); }        
    }
}
