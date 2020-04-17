using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilitarios;

namespace Data {

    /*
        Author: Jhonattan Pulido
        Creation Date: 17/04/2020
        Description: Controller created for generate payments with Stripe
    */
    public class DAOStripe {

        /*
            Author: Jhonattan Pulido
            Creation Date: 17/04/2020
            Description: This method initialize the stripe config
            Parameters: UStripe customerInfo: Object with the credential information declared
            Returns: The var "customerInfo" with the secret API key & public API key declared
        */
        public UStripe InitConfig(UStripe customerInfo) {
            
            customerInfo.SecretApiKey = "sk_test_Kj64NJ6w0vZzdSYqj6ISaB5Z00KfR7Q05t";
            customerInfo.PublicApiKey = "pk_test_9ghoZ0r96C1aUefVJ9iv1N9j00okeHjdPx";
            return customerInfo;
        }        
    }
}
