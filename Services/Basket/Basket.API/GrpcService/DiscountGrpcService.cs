using Discount.Grpc.Protos;

namespace Basket.API.GrpcService;

public class DiscountGrpcService
{
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;

    public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
    {
        _discountProtoService = discountProtoService;
    }

    public async Task<CouponModel> GetDiscount(string productName)
    {
        // Gọi sang phương thức GetDiscount (hỗ trợ Async) của bên phía Grpc Server
        GetDiscountRequest discountRequest = new GetDiscountRequest { ProductName = productName };
        return await _discountProtoService.GetDiscountAsync(discountRequest);
    }
}
