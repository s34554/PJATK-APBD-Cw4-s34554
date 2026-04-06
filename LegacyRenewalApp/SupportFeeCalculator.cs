namespace LegacyRenewalApp;

public class SupportFeeCalculator : ISupportFeeCalculator
{
    public CalculatorResult Calculate(string planCode, bool includePremiumSupport)
    {
        var supportFee = 0m;
        var notes = string.Empty;
        if (includePremiumSupport)
        {
            if (planCode == "START")
            {
                supportFee = 250m;
            }
            else if (planCode == "PRO")
            {
                supportFee = 400m;
            }
            else if (planCode == "ENTERPRISE")
            {
                supportFee = 700m;
            }

            notes += "premium support included; ";
        }

        return new CalculatorResult
        {
            Amount = supportFee,
            Notes = notes
        };
    }
}