namespace Utilitarios {
    /*
        Author: Jhonattan Pulido
        Creation Date: 17/04/2020
        Description: Class created for declare the necesary attributes for generate payments with Stripe
    */
    public class UStripe {

        // Vars
        private string secretApiKey;
        private string publicApiKey;
        private string cardNumber;
        private string monthExpiration;
        private string yearExpiration;
        private string cvc;
        private string email;
        private string token;
        private long amount;

        // Get & Set Methods
        public string SecretApiKey { get => secretApiKey; set => secretApiKey = value; }
        public string PublicApiKey { get => publicApiKey; set => publicApiKey = value; }
        public string CardNumber { get => cardNumber; set => cardNumber = value; }
        public string MonthExpiration { get => monthExpiration; set => monthExpiration = value; }
        public string YearExpiration { get => yearExpiration; set => yearExpiration = value; }
        public string Cvc { get => cvc; set => cvc = value; }
        public string Email { get => email; set => email = value; }
        public string Token { get => token; set => token = value; }
        public long Amount { get => amount; set => amount = value; }
    }
}
