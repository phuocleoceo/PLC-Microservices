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

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        Coupon coupon = _mapper.Map<Coupon>(request.Coupon);
        if (await _repository.CreateDiscount(coupon))
        {
            _logger.LogInformation($"Discount for {coupon.ProductName} created.");
            return _mapper.Map<CouponModel>(coupon);
        }
        else
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Fail when create discount for {coupon.ProductName}"));
        }
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        Coupon coupon = _mapper.Map<Coupon>(request.Coupon);
        if (await _repository.UpdateDiscount(coupon))
        {
            _logger.LogInformation($"Discount for {coupon.ProductName} updated.");
            return _mapper.Map<CouponModel>(coupon);
        }
        else
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, $"Fail when update discount for {coupon.ProductName}"));
        }
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        bool deleted = await _repository.DeleteDiscount(request.ProductName);
        DeleteDiscountResponse response = new DeleteDiscountResponse { Success = deleted };
        return response;
    }
}
