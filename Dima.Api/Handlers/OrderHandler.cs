using Dima.Api.Data;
using Dima.Core.Enums;
using Dima.Core.Handlers;
using Dima.Core.Models;
using Dima.Core.Requests.Orders;
using Dima.Core.Responses;
using Microsoft.EntityFrameworkCore;

namespace Dima.Api.Handlers
{
    public class OrderHandler(AppDbContext context) : IOrderHandler
    {
        public async Task<Response<Order?>> CancelAsync(CancelOrderRequest request)
        {
            Order? order;
            try
            {
                order = await context.Orders.Include(x => x.Product).Include(x => x.Voucher)
                     .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == request.UserId);

                if (order is null)
                {
                    return new Response<Order?>(null, 404, "Pedido não encontrado");
                }

            }
            catch
            {
                return new Response<Order?>(null, 500, "Falha ao obter pedido");
            }

            switch (order.Status)
            {
                case EOrderStatus.Canceled:
                    return new Response<Order?>(order, 400, "Esse pedido já foi cancelado");

                case EOrderStatus.WaitingPayment:
                    break;

                case EOrderStatus.Paid:
                    return new Response<Order?>(order, 400, "Esse pedido já foi pago e não pode ser cancelado");

                case EOrderStatus.Refunded:
                    return new Response<Order?>(order, 400, "Esse pedido já foi reembolsado e não pode mais ser cancelado");

                default:
                    return new Response<Order?>(order, 400, "Esse pedido não pode ser cancelado");
            }

            order.Status = EOrderStatus.Canceled;
            order.UpdateAt = DateTime.Now;

            try
            {
                context.Orders.Update(order);
                await context.SaveChangesAsync();
            }
            catch
            {
                return new Response<Order?>(order, 500, "Não foi possível cancelar seu pedido");
            }

            return new Response<Order?>(order, 200, $"Pedido {order.Number} cancelado com sucesso");
        }

        public async Task<Response<Order?>> CreateAsync(CreateOrderRequest request)
        {
            Product? product;
            try
            {
                product = await context.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id ==
                request.ProductId && x.IsActive == true);

                if (product is null)
                    return new Response<Order?>(null, 400, "Produto não encontrado");

                context.Attach(product);
            }
            catch
            {
                return new Response<Order?>(null, 500, "Não foi possível obter o produto") /
            }

            Voucher? voucher = null;

            try
            {
                if (request.VoucherId is not null)
                {
                    voucher = await context.Vouchers.AsNoTracking().FirstOrDefaultAsync(x => x.Id
                    == request.VoucherId && x.IsActive == true);

                    if (voucher is null)
                        return new Response<Order?>(null, 400, "Voucher inválido ou não encontrado");

                    if (voucher.IsActive == false)
                        return new Response<Order?>(null, 400, "Este voucher já foi utilizado");

                    voucher.IsActive = false;
                    context.Vouchers.Update(voucher);
                }
            }
            catch
            {
                return new Response<Order?>(null, 500, "Falha ao obter o voucher informado");
            }

            var order = new Order()
            {
                UserId = request.UserId,
                Product = product,
                ProductId = request.ProductId,
                Voucher = voucher,
                VoucherId = request.VoucherId

            };

            try
            {
                await context.Orders.AddAsync(order);
                await context.SaveChangesAsync();
            }
            catch
            {
                return new Response<Order?>(null, 500, "Não foi possível realizar seu pedido");
            }

            return new Response<Order?>(order, 201, $"Pedido {order.Number} cadastrado com sucesso");

        }

        public Task<PagedResponse<List<Order>?>> GetAllAsync(GetAllOrdersRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Order?>> GetByNumberAsync(GetOrderByNumberRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Order?>> PayAsync(PayOrderRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<Response<Order?>> RefundAsync(RefundOrderRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
