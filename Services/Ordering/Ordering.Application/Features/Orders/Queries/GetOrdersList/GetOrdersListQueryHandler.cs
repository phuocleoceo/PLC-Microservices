using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList;

public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVM>>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    public GetOrdersListQueryHandler(IOrderRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<List<OrdersVM>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Order> orderList = await _repository.GetOrdersByUserName(request.UserName);
        return _mapper.Map<List<OrdersVM>>(orderList);
    }
}
