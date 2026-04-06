namespace LegacyRenewalApp;

public interface ISupportFeeCalculator
{
    public CalculatorResult Calculate(
        string planCode,
        bool includePremiumSupport);
}