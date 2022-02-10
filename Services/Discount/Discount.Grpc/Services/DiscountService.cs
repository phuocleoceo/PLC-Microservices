using Discount.Grpc.Repositories;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using AutoMapper;
using Grpc.Core;

namespace Discount.Grpc.Services;

public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly IDiscountRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DiscountService> _logger;
    public DiscountService(IDiscountRepository repository, IMapper mapper, ILogger<DiscountService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        Coupon coupon = await _repository.GetDiscount(request.ProductName);
        if (coupon is null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, $"Not found discount for {request.ProductName}"));
        }
        return _mapper.Map<CouponModel>(coupon);
    }
}
