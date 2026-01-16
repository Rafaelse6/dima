using Dima.Core;
using Dima.Core.Handlers;
using Dima.Core.Requests.Stripe;
using Dima.Core.Responses;
using Dima.Core.Responses.Stripe;
using Stripe;
using Stripe.Checkout;
using System.Security.Cryptography.Xml;
using System.Text;

namespace Dima.Api.Handlers;

public class StripeHandler : IStripeHandler
{
    public async Task<Response<string?>> CreateSessionAsync(CreateSessionRequest request)
    {
        var options = new SessionCreateOptions
        {
            PaymentMethodTypes = ["card"],
            Mode = "payment",

            PaymentIntentData = new SessionPaymentIntentDataOptions
            {
                Metadata = new Dictionary<string, string>
            {
                { "order", request.OrderNumber }
            }
            },
            LineItems =
                [
                    new SessionLineItemOptions
                    {
                        Quantity = 1,
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "brl",
                            UnitAmount = 79990,
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Premium Anual",
                                Description = "Plano para um ano de acesso"
                            }
                        }
                    }
                ],
            SuccessUrl = $"{Configuration.FrontendUrl}/pedidos/{request.OrderNumber}/confirmar",
            CancelUrl = $"{Configuration.FrontendUrl}/pedidos/{request.OrderNumber}/cancelar",
        };

        var service = new SessionService();
        var session = await service.CreateAsync(options);

        return new Response<string?>(session.Id);
    }

    public async Task<Response<List<StripeTransactionResponse>>> GetTransactionsByOrderNumberAsync(GetTransactionsByOrderNumberRequest request)
    {
        var options = new ChargeSearchOptions
        {
            Query = $"metadata['order']: '{request.Number}'"
        };

        var service = new ChargeService();

        var result = await service.SearchAsync(options);

        if (result.Data.Count == 0)
            return new Response<List<StripeTransactionResponse>>(null, 404, "Nenhuma transação encontrada");

        var data = new List<StripeTransactionResponse>();
        foreach (var item in result.Data)
        {
            data.Add(new StripeTransactionResponse
            {
                Id = item.Id,
                Email = item.BillingDetails.Email,
                Amount = item.Amount,
                AmountCaptured = item.AmountCaptured,
                Status = item.Status,
                Paid = item.Paid,
                Refund = item.Refunded
            });
        }

        return new Response<List<StripeTransactionResponse>>(data);

    }
}
