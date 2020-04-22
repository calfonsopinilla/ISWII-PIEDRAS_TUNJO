// Librerias
using Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using Utilitarios;
using Stripe;

namespace PiedrasDelTunjo.Controllers {

    /*
        Author: Jhonattan Pulido
        Creation Date: 17/04/2020
        Description: Controller created for generate payments with Stripe
    */
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("stripe")]
    public class StripeController : ApiController {

        // Vars                
        private Charge charge;        
        private Customer customer;
        private string description;
        private UUsuario user = new UUsuario();
        private Token stripeToken = new Token();
        private TokenService tokenService = new TokenService();
        private ChargeService chargeService = new ChargeService();
        private CustomerService customerService = new CustomerService();

        [HttpPost]
        //[Authorize]
        [Route("payment")]
        public HttpResponseMessage GeneratePayment([FromBody] UStripe customerInfo, [FromUri] int userId) {

            try {

                    customerInfo = new LStripe().InitConfig(customerInfo);
                StripeConfiguration.ApiKey = customerInfo.SecretApiKey;

                // Object for validate the credit card
                var options = new TokenCreateOptions {

                    Card = new CreditCardOptions {
                        Number = customerInfo.CardNumber,
                        ExpYear = long.Parse(customerInfo.YearExpiration),
                        ExpMonth = long.Parse(customerInfo.MonthExpiration),
                        Cvc = customerInfo.Cvc
                    }
                };

                this.stripeToken = this.tokenService.Create(options);        
                this.user = new LUsuario().Buscar(userId);  

                if (this.user != null) {
                
                    this.description = "Pago generado para el usuario " + this.user.Nombre + " " + this.user.Apellido + " por un monto de: $" + customerInfo.Amount;                

                    this.customer = this.customerService.Create(new CustomerCreateOptions {
                        Source = this.stripeToken.Id,
                        Email = customerInfo.Email,                    
                    });                

                    this.charge = this.chargeService.Create(new ChargeCreateOptions {
                        Amount = customerInfo.Amount,
                        Description = this.description,
                        Currency = "usd",
                        Customer = this.customer.Id,
                        ReceiptEmail = customerInfo.Email                    
                    });

                    if (this.charge.Status.Equals("succeeded"))
                        return Request.CreateResponse(HttpStatusCode.OK, new { ok = true, message = "Pago generado con éxito", this.charge.BalanceTransactionId });
                    else
                        return Request.CreateResponse(HttpStatusCode.Conflict, new { ok = false, message = "ERROR: No se generó el pago", this.charge.BalanceTransactionId });

                } else
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Los datos del usuario son incorrectos" });

            } catch { return Request.CreateResponse(HttpStatusCode.InternalServerError, new { ok = false, message = "ERROR: Tarjeta de credito incorrecta" }); }
        }
    }
}
