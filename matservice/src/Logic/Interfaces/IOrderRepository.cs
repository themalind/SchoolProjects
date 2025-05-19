using System.Linq.Expressions;

using Logic.Entities;
using Logic.Enums;

namespace Logic.Interfaces;

public interface IOrderRepository
{
    Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken token = default);
    Task<Courier?> GetCourierAsync(int courierId, CancellationToken token = default);
    Task<IReadOnlyCollection<Order>> ListAsync(CancellationToken token = default);
    Task<IReadOnlyCollection<Order>> ListAsync(Expression<Func<Order, bool>> predicate, CancellationToken token = default);
    Task<Order> AddAsync(Order order, CancellationToken token = default);
    Task ChangeOrderStatusAsync(Order order, OrderStatus orderStatus, CancellationToken token = default);
    Task<Order> AssignOrderToCourierAsync(Order order, Courier courier, CancellationToken token = default);
    Task<Order> UpdateStatus(Order order, CancellationToken token = default);
}