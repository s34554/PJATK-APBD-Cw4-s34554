using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators;

public class DiscountCalculator : IDiscountCalculator
{
    public CalculatorResult Calculate(
        decimal baseAmount,
        Customer customer,
        int seatCount,
        bool useLoyaltyPoints,
        bool isEducationEligible)
    {
        decimal discountAmount = 0;
        var notes = "";
        switch (customer.Segment)
        {
            case "Silver": 
                discountAmount += baseAmount * 0.05m;
                notes += "silver discount; ";
                break;
            case "Gold":
                discountAmount += baseAmount * 0.10m;
                notes += "gold discount; ";
                break;
            case "Platinum":
                discountAmount += baseAmount * 0.15m;
                notes += "platinum discount; ";
                break;
            case "Education":
                if (isEducationEligible)
                {
                    discountAmount += baseAmount * 0.20m;
                    notes += "education discount; ";
                }
                break;
        }

        switch (customer.YearsWithCompany)
        {
            case >= 5:
                discountAmount += baseAmount * 0.07m;
                notes += "long-term loyalty discount; ";
                break;
            case >=2:
                discountAmount += baseAmount * 0.03m;
                notes += "basic loyalty discount; ";
                break;
        }

        switch (seatCount)
        {
            case >= 50:
                discountAmount += baseAmount * 0.12m;
                notes += "large team discount; ";
                break;
            case >= 20:
                discountAmount += baseAmount * 0.08m;
                notes += "medium team discount; ";
                break;
            case >= 10:
                discountAmount += baseAmount * 0.04m;
                notes += "small team discount; ";
                break;
        }
        if (useLoyaltyPoints && customer.LoyaltyPoints > 0)
        {
            int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;
            discountAmount += pointsToUse;
            notes += $"loyalty points used: {pointsToUse}; ";
        }
        return new CalculatorResult
        {
            Amount = discountAmount,
            Notes = notes
        };
    }
}