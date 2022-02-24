using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Microsoft.Extensions.Logging;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder;

public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
{
    private readonly IOrderRepository _repository;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    private readonly ILogger<CheckoutOrderCommandHandler> _logger;
    public CheckoutOrderCommandHandler(IOrderRepository repository, IMapper mapper,
                                        IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
    {
        Order orderEntity = _mapper.Map<Order>(request);
        Order newOrder = await _repository.AddAsync(orderEntity);

        _logger.LogInformation($"Order {newOrder.Id} is created.");

        await SendMail(newOrder);
        return newOrder.Id;
    }

    private async Task SendMail(Order order)
    {
        Email email = new Email
        {
            To = "ht10082001@gmail.com",
            Body = "Order was created",
            Subject = "Order was created"
        };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception e)
        {
            _logger.LogError($"Order {order.Id} failed to send mail {e.Message}");
        }
    }
}
