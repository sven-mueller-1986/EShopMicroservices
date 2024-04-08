using Discount.Grpc;
using EShopMicroservices.Services.Discount.Grpc.Data;
using EShopMicroservices.Services.Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace EShopMicroservices.Services.Discount.Grpc.Services;

public class DiscountService(DiscountContext dbContext, ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

        if (coupon is null)
            coupon = new Coupon { Id = 0, ProductName = "No Discount", Amount = 0, Description = "No Discount Description" };

        logger.LogInformation("Discount is retrieved for {productName} ({amount})", request.ProductName, coupon.Amount);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request Object"));

        var couponEntity = dbContext.Add(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully created: {productName} ({amount})", couponEntity.Entity.ProductName, couponEntity.Entity.Amount);

        return couponEntity.Entity.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request Object"));

        var couponEntity = dbContext.Update(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully updated: {productName} ({amount})", couponEntity.Entity.ProductName, couponEntity.Entity.Amount);

        return couponEntity.Entity.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await dbContext
            .Coupons
            .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with {request.ProductName} was not found."));

        var couponEntity = dbContext.Coupons.Remove(coupon);
        await dbContext.SaveChangesAsync();

        logger.LogInformation("Discount is successfully deleted: {productName} ({amount})", couponEntity.Entity.ProductName, couponEntity.Entity.Amount);

        return new DeleteDiscountResponse { Success = true };
    }
}

