namespace LegacyRenewalApp;

public interface IDiscountCalculator
{
    public CalculatorResult Calculate(
        decimal baseAmount,
        Customer customer,
        int seatCount,
        bool useLoyaltyPoints,
        bool isEducationEligible);
}