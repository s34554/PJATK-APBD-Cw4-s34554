using System;

namespace LegacyRenewalApp;

public class PaymentFeeCalculator : IPaymentFeeCalculator
{
    public CalculatorResult Calculate(decimal amount, string paymentMethod)
    {
        var paymentFee = 0m;
        var notes = string.Empty;
        
        if (paymentMethod == "CARD")
        {
            paymentFee = amount * 0.02m;
            notes += "card payment fee; ";
        }
        else if (paymentMethod == "BANK_TRANSFER")
        {
            paymentFee = amount * 0.01m;
            notes += "bank transfer fee; ";
        }
        else if (paymentMethod == "PAYPAL")
        {
            paymentFee = amount * 0.035m;
            notes += "paypal fee; ";
        }
        else if (paymentMethod == "INVOICE")
        {
            paymentFee = 0m;
            notes += "invoice payment; ";
        }
        else
        {
            throw new ArgumentException("Unsupported payment method");
        }

        return new CalculatorResult()
        {
            Amount = paymentFee,
            Notes = notes
        };
    }
}