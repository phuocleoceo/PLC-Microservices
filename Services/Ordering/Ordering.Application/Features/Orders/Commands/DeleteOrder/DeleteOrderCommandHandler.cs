using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;
    public DeleteOrderCommandHandler(IOrderRepository repository, IMapper mapper,
                                                                    ILogger<DeleteOrderCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        Order deleteOrder = await _repository.GetByIdAsync(request.Id);
        if (deleteOrder is null)
        {
            _logger.LogError($"Order {request.Id} not exists");
            throw new NotFoundException(nameof(Order), request.Id);
        }
        await _repository.DeleteAsync(deleteOrder);
        _logger.LogInformation($"Order {deleteOrder.Id} is deleted");
        return Unit.Value;
    }
}
