using Application.Shared.Enum;

namespace Application.Shared.Models.Request;

public class PaymentRequest
{
    public List<EnumTypePayments> TypePayments { get; set; } = null!;
}