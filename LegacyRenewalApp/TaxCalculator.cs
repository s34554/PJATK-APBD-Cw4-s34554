namespace LegacyRenewalApp;

public class TaxCalculator : ITaxCalculator
{
    public CalculatorResult Calculate(decimal taxBase, string country)
    {
        var taxRate = 0.20m;
        
        if (country == "Poland")
        {
            taxRate = 0.23m;
        }
        else if (country == "Germany")
        {
            taxRate = 0.19m;
        }
        else if (country == "Czech Republic")
        {
            taxRate = 0.21m;
        }
        else if (country == "Norway")
        {
            taxRate = 0.25m;
        }

        var taxAmount = taxBase * taxRate;

        return new CalculatorResult()
        {
            Amount = taxAmount
        };
    }
}