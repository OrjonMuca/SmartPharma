using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smartPharmaAPI.Data;
using smartPharmaAPI.Dtos;
using smartPharmaAPI.Models;
using smartPharmaAPI.Services;

namespace smartPharmaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public sealed class OrdersController : ControllerBase
{
    private readonly SmartPharmaDbContext _db;
    private readonly OrderService _orderService;

    public OrdersController(SmartPharmaDbContext db, OrderService orderService)
    {
        _db = db;
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetOrders(CancellationToken cancellationToken)
    {
        var query = OrderQuery();
        if (!IsStaff())
        {
            var currentUserId = GetCurrentUserId();
            query = query.Where(order => order.CustomerUserId == currentUserId);
        }

        var orders = await query
            .OrderByDescending(order => order.CreatedAtUtc)
            .ToListAsync(cancellationToken);

        return Ok(orders.Select(ToResponse).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponse>> GetOrder(Guid id, CancellationToken cancellationToken)
    {
        var order = await OrderQuery()
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);

        if (order is null)
        {
            return NotFound();
        }

        if (!IsStaff() && order.CustomerUserId != GetCurrentUserId())
        {
            return Forbid();
        }

        return Ok(ToResponse(order));
    }

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> CreateOrder(OrderRequest request, CancellationToken cancellationToken)
    {
        var normalizedRequest = await NormalizeOrderRequestAsync(request, cancellationToken);
        if (normalizedRequest is null)
        {
            return Unauthorized();
        }

        if (normalizedRequest.CustomerUserId.HasValue &&
            !await _db.Users.AnyAsync(user => user.Id == normalizedRequest.CustomerUserId.Value, cancellationToken))
        {
            return BadRequest("Customer user does not exist.");
        }

        var result = await _orderService.PlaceOrderAsync(normalizedRequest, cancellationToken);
        if (!result.Succeeded || result.Value is null)
        {
            return BadRequest(result.Error);
        }

        var order = await OrderQuery().SingleAsync(order => order.Id == result.Value.Id, cancellationToken);
        return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, ToResponse(order));
    }

    [Authorize(Roles = "Admin,Pharmacist")]
    [HttpPatch("{id:guid}/status")]
    public async Task<ActionResult<OrderResponse>> UpdateStatus(
        Guid id,
        UpdateOrderStatusRequest request,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var status))
        {
            return BadRequest("Status must be Pending, Confirmed, Dispensed, or Cancelled.");
        }

        var order = await _db.Orders.FindAsync([id], cancellationToken);
        if (order is null)
        {
            return NotFound();
        }

        order.Status = status;
        await _db.SaveChangesAsync(cancellationToken);

        order = await OrderQuery().SingleAsync(item => item.Id == id, cancellationToken);
        return Ok(ToResponse(order));
    }

    private IQueryable<Order> OrderQuery()
    {
        return _db.Orders
            .Include(order => order.Items)
            .ThenInclude(item => item.Medicine)
            .AsNoTracking();
    }

    private async Task<OrderRequest?> NormalizeOrderRequestAsync(OrderRequest request, CancellationToken cancellationToken)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId is null)
        {
            return null;
        }

        if (IsStaff())
        {
            return request;
        }

        var currentUser = await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(user => user.Id == currentUserId.Value, cancellationToken);

        if (currentUser is null)
        {
            return null;
        }

        var customerName = string.IsNullOrWhiteSpace(request.CustomerName)
            ? currentUser.FullName
            : request.CustomerName.Trim();

        return request with
        {
            CustomerUserId = currentUser.Id,
            CustomerName = customerName
        };
    }

    private bool IsStaff()
    {
        return User.IsInRole(UserRole.Admin.ToString()) || User.IsInRole(UserRole.Pharmacist.ToString());
    }

    private Guid? GetCurrentUserId()
    {
        var rawUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(rawUserId, out var userId) ? userId : null;
    }

    private static OrderResponse ToResponse(Order order)
    {
        return new OrderResponse(
            order.Id,
            order.CustomerUserId,
            order.CustomerName,
            order.PrescriptionId,
            order.Status.ToString(),
            order.TotalAmount,
            order.CreatedAtUtc,
            order.Items
                .OrderBy(item => item.Medicine!.Name)
                .Select(item => new OrderItemResponse(
                    item.Id,
                    item.MedicineId,
                    item.Medicine?.Name ?? "Unknown medicine",
                    item.Quantity,
                    item.UnitPrice,
                    item.UnitPrice * item.Quantity))
                .ToList());
    }
}
