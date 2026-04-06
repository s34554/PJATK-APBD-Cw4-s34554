using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Interfaces;

public interface IPaymentFeeCalculator
{
    public CalculatorResult Calculate(
        decimal amount,
        string paymentMethod);
}