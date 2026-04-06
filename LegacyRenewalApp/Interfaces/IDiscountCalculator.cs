using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Interfaces;

public interface IDiscountCalculator
{
    public CalculatorResult Calculate(
        decimal baseAmount,
        Customer customer,
        int seatCount,
        bool useLoyaltyPoints,
        bool isEducationEligible);
}