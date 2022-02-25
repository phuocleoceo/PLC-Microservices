using Ordering.Application.Contracts.Persistence;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;
    public UpdateOrderCommandHandler(IOrderRepository repository, IMapper mapper,
                                                                    ILogger<UpdateOrderCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    // Unit -> Void
    public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        Order updateOrder = await _repository.GetByIdAsync(request.Id);
        if (updateOrder is null)
        {
            _logger.LogError($"Order {request.Id} not exists");
        }
        // Update by AutoMapper, request thay thế cho updateOrder
        _mapper.Map(request, updateOrder, typeof(UpdateOrderCommand), typeof(Order));
        await _repository.UpdateAsync(updateOrder);
        _logger.LogInformation($"Order {updateOrder.Id} is updated");
        return Unit.Value;
    }
}
