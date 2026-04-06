using System;
using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Calculators;

public class PaymentFeeCalculator : IPaymentFeeCalculator
{
    public CalculatorResult Calculate(decimal amount, string paymentMethod)
    {
        decimal paymentFee;
        var notes = string.Empty;
        switch (paymentMethod)
        {
            case "CARD":
                paymentFee = amount * 0.02m;
                notes += "card payment fee; ";
                break;
            case "BANK_TRANSFER":
                paymentFee = amount * 0.01m;
                notes += "bank transfer fee; ";
                break;
            case "PAYPAL":
                paymentFee = amount * 0.035m;
                notes += "paypal fee; ";
                break;
            case "INVOICE":
                paymentFee = 0m;
                notes += "invoice payment; ";
                break;
            default:
                throw new ArgumentException("Unsupported payment method");
        }
        return new CalculatorResult()
        {
            Amount = paymentFee,
            Notes = notes
        };
    }
}