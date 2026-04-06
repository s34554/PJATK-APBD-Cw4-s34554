namespace LegacyRenewalApp;

public interface IPaymentFeeCalculator
{
    public CalculatorResult Calculate(
        decimal amount,
        string paymentMethod);
}