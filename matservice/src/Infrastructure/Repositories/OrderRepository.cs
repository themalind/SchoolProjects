using System.Linq.Expressions;

using Infrastructure.Data;

using Logic.Entities;
using Logic.Enums;
using Logic.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class OrderRepository(IDbContextFactory<FoodDbContext> context) : IOrderRepository
{
    private readonly IDbContextFactory<FoodDbContext> _context = context;

    public async Task<IReadOnlyCollection<Order>> ListAsync(Expression<Func<Order, bool>> predicate, CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        var list = await context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Address)
            .Include(c => c.Courier)
            .Include(o => o.Customer)
            .Where(predicate)
            .AsNoTracking()
            .ToListAsync(token);

        return list;
    }

    public async Task<IReadOnlyCollection<Order>> ListAsync(CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        var list = await context.Orders
            .Include(o => o.OrderItems)
            .Include(o => o.Address)
            .Include(c => c.Courier)
            .Include(o => o.Customer)
            .AsNoTracking()
            .ToListAsync(token);

        return list;
    }

    public async Task<Order> AddAsync(Order order, CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        context.Orders.Add(order);

        await context.SaveChangesAsync(token);

        var createdOrder = await context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == order.OrderNumber, token);

        return createdOrder is null ? throw new ArgumentNullException("Error") : createdOrder;
    }

    public async Task<Order> AssignOrderToCourierAsync(Order order, Courier courier, CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        if (order is null || courier is null)
        {
            throw new ArgumentNullException($"{order} or {courier} can't be null!");
        }

        order.Courier = courier;

        context.Orders.Update(order);

        await context.SaveChangesAsync(token);

        return order;
    }

    public async Task ChangeOrderStatusAsync(Order order, OrderStatus orderStatus, CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        if (order is null)
        {
            throw new ArgumentNullException($"{order} can't be null!");
        }

        order.Status = orderStatus;

        context.Orders.Update(order);

        await context.SaveChangesAsync(token);
    }

    public async Task<Order?> GetByOrderNumberAsync(string orderNumber, CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        var order = await context.Orders
            .Include(c => c.Customer)
            .Include(a => a.Address)
            .Include(c => c.Courier)
            .Include(oi => oi.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber, token);

        return order;
    }

    public async Task<Courier?> GetCourierAsync(int courierId, CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync(token);

        var courier = await context.Couriers.FindAsync(courierId, token);

        return courier;
    }

    public async Task<Order> UpdateStatus(Order order, CancellationToken token = default)
    {
        using var context = await _context.CreateDbContextAsync();

        context.Orders.Update(order);

        await context.SaveChangesAsync();

        return order;
    }


}