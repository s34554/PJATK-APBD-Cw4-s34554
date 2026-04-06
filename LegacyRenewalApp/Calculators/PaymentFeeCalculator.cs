using System;
using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators;

public class PaymentFeeCalculator : IPaymentFeeCalculator
{
    public CalculatorResult Calculate(decimal amount, string paymentMethod)
    {
        return paymentMethod switch
        {
            "CARD"          => new CalculatorResult { Amount = amount * 0.02m,  Notes = "card payment fee; " },
            "BANK_TRANSFER" => new CalculatorResult { Amount = amount * 0.01m,  Notes = "bank transfer fee; " },
            "PAYPAL"        => new CalculatorResult { Amount = amount * 0.035m, Notes = "paypal fee; " },
            "INVOICE"       => new CalculatorResult { Amount = 0m,              Notes = "invoice payment; " },
            _ => throw new ArgumentException("Unsupported payment method")
        };
    }
}