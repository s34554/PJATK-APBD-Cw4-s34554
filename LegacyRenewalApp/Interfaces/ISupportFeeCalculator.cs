using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Interfaces;

public interface ISupportFeeCalculator
{
    public CalculatorResult Calculate(
        string planCode,
        bool includePremiumSupport);
}