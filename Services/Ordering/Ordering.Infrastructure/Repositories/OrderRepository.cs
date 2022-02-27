using Ordering.Application.Contracts.Persistence;
using Ordering.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Repositories;

public class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OrderContext db) : base(db) { }

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
    {
        return await _db.Orders.Where(c => c.UserName == userName).ToListAsync();
    }
}
