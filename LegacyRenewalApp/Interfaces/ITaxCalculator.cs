using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Interfaces;

public interface ITaxCalculator
{
    public CalculatorResult Calculate(
        decimal taxBase,
        string country);
}