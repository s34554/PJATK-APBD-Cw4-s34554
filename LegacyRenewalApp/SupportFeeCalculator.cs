namespace LegacyRenewalApp;

public class SupportFeeCalculator : ISupportFeeCalculator
{
    public CalculatorResult Calculate(string planCode, bool includePremiumSupport)
    {
        var supportFee = 0m;
        var notes = string.Empty;
        if (includePremiumSupport)
        {
            supportFee = planCode switch
            {
                "START" => 250m,
                "PRO" => 400m,
                "ENTERPRISE" => 700m,
                _ => supportFee
            };

            notes += "premium support included; ";
        }

        return new CalculatorResult
        {
            Amount = supportFee,
            Notes = notes
        };
    }
}