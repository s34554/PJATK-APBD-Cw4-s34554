using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators;

public class SupportFeeCalculator : ISupportFeeCalculator
{
    public CalculatorResult Calculate(string planCode, bool includePremiumSupport)
    {
        if (!includePremiumSupport)
            return new CalculatorResult();

        return planCode switch
        {
            "START"      => new CalculatorResult { Amount = 250m, Notes = "premium support included; " },
            "PRO"        => new CalculatorResult { Amount = 400m, Notes = "premium support included; " },
            "ENTERPRISE" => new CalculatorResult { Amount = 700m, Notes = "premium support included; " },
            _            => new CalculatorResult { Notes = "premium support included; " }
        };
    }
}