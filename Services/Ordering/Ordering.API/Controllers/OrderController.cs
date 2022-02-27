using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace Ordering.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly IMediator _mediator;
    public OrderController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{userName}", Name = "GetOrder")]
    public async Task<IEnumerable<OrdersVM>> GetOrdersByUserName(string userName)
    {
        GetOrdersListQuery query = new GetOrdersListQuery() { UserName = userName };
        return await _mediator.Send(query);
    }

    [HttpPost(Name = "CheckoutOrder")]
    public async Task<ActionResult<int>> CheckoutOrder(CheckoutOrderCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut(Name = "UpdateOrder")]
    public async Task<ActionResult> UpdateOrder(UpdateOrderCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}", Name = "DeleteOrder")]
    public async Task<ActionResult> DeleteOrder(int id)
    {
        DeleteOrderCommand command = new DeleteOrderCommand() { Id = id };
        await _mediator.Send(command);
        return NoContent();
    }
}
