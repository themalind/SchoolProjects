using Logic.Entities;
using Logic.Enums;
using Logic.Interfaces;

namespace Logic.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;

    public OrderService(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<Order> AddAsync(Order order, CancellationToken token = default)
    {
        return await _repository.AddAsync(order, token);
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken token = default)
    {
        return await _repository.GetByOrderNumberAsync(orderNumber, token);
    }

    public async Task<Order> UpdateStatus(Order order, CancellationToken token = default)
    {
        return await _repository.UpdateStatus(order, token);
    }
}