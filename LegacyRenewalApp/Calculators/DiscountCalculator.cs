using System;
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
        string notes = "";
        var segment = ApplySegmentDiscount(baseAmount, customer.Segment, isEducationEligible);
        var tenure  = ApplyTenureDiscount(baseAmount, customer.YearsWithCompany);
        var team    = ApplyTeamDiscount(baseAmount, seatCount);

        discountAmount = segment.Amount + tenure.Amount + team.Amount;
        notes    = segment.Notes + tenure.Notes + team.Notes;

        if (useLoyaltyPoints && customer.LoyaltyPoints > 0)
        {
            int pointsToUse = Math.Min(customer.LoyaltyPoints, 200);
            discountAmount += pointsToUse;
            notes += $"loyalty points used: {pointsToUse}; ";
        }

        return new CalculatorResult { Amount = discountAmount, Notes = notes };
    }
    private static CalculatorResult ApplySegmentDiscount(decimal baseAmount, string segment, bool isEducationEligible)
    {
        return segment switch
        {
            "Silver"    => new CalculatorResult { Amount = baseAmount * 0.05m, Notes = "silver discount; " },
            "Gold"      => new CalculatorResult { Amount = baseAmount * 0.10m, Notes = "gold discount; " },
            "Platinum"  => new CalculatorResult { Amount = baseAmount * 0.15m, Notes = "platinum discount; " },
            "Education" when isEducationEligible
                => new CalculatorResult { Amount = baseAmount * 0.20m, Notes = "education discount; " },
            _ => new CalculatorResult()
        };
    }
    private static CalculatorResult ApplyTenureDiscount(decimal baseAmount, int yearsWithCompany)
    {
        return yearsWithCompany switch
        {
            >= 5 => new CalculatorResult { Amount = baseAmount * 0.07m, Notes = "long-term loyalty discount; " },
            >= 2 => new CalculatorResult { Amount = baseAmount * 0.03m, Notes = "basic loyalty discount; " },
            _    => new CalculatorResult()
        };
    }

    private static CalculatorResult ApplyTeamDiscount(decimal baseAmount, int seatCount)
    {
        return seatCount switch
        {
            >= 50 => new CalculatorResult { Amount = baseAmount * 0.12m, Notes = "large team discount; " },
            >= 20 => new CalculatorResult { Amount = baseAmount * 0.08m, Notes = "medium team discount; " },
            >= 10 => new CalculatorResult { Amount = baseAmount * 0.04m, Notes = "small team discount; " },
            _ => new CalculatorResult()
        };
    }
}