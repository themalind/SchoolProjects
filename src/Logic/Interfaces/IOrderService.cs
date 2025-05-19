using Logic.Entities;
using Logic.Enums;

namespace Logic.Interfaces;

public interface IOrderService
{
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken token = default);
    Task<Order> AddAsync(Order order, CancellationToken token = default);
    Task<Order> UpdateStatus(Order order, CancellationToken token = default);

}