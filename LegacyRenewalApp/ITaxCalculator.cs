namespace LegacyRenewalApp;

public interface ITaxCalculator
{
    public CalculatorResult Calculate(
        decimal taxBase,
        string country);
}