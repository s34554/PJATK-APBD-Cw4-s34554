namespace LegacyRenewalApp;

public class InvoiceCalculator : IInvoiceCalculator
{
    private readonly IDiscountCalculator _discountCalculator;
    private readonly ISupportFeeCalculator _supportFeeCalculator;
    private readonly IPaymentFeeCalculator _paymentFeeCalculator;
    private readonly ITaxCalculator _taxCalculator;
    public InvoiceCalculator(
        IDiscountCalculator discountCalculator,
        ISupportFeeCalculator supportFeeCalculator,
        IPaymentFeeCalculator paymentFeeCalculator,
        ITaxCalculator taxCalculator)
    {
        _discountCalculator = discountCalculator;
        _supportFeeCalculator = supportFeeCalculator;
        _paymentFeeCalculator = paymentFeeCalculator;
        _taxCalculator = taxCalculator;
    }
    public InvoiceResult Calculate(
        Customer customer,
        SubscriptionPlan plan,
        int seatCount,
        string paymentMethod,
        bool includePremiumSupport,
        bool useLoyaltyPoints)
    {
        var notes = string.Empty;
        
        decimal baseAmount = plan.MonthlyPricePerSeat * seatCount * 12m + plan.SetupFee;

        var discountResult = _discountCalculator.Calculate(baseAmount, customer, seatCount, useLoyaltyPoints, plan.IsEducationEligible);
        var discountAmount = discountResult.Amount;
        notes += discountResult.Notes;
        var subtotalAfterDiscount = baseAmount - discountAmount;
        if (subtotalAfterDiscount < 300m)
        {
            subtotalAfterDiscount = 300m;
            notes += "minimum discounted subtotal applied; ";
        }
        
        var supportFeeResult = _supportFeeCalculator.Calculate(plan.Code, includePremiumSupport);
        var supportFee = supportFeeResult.Amount;
        notes += supportFeeResult.Notes;

        var paymentFeeResult = _paymentFeeCalculator.Calculate(subtotalAfterDiscount + supportFee, paymentMethod);
        var paymentFee = paymentFeeResult.Amount;
        notes += paymentFeeResult.Notes;

        var taxResult = _taxCalculator.Calculate(subtotalAfterDiscount + supportFee + paymentFee, customer.Country);
        var taxAmount = taxResult.Amount;
        notes += taxResult.Notes;

        var finalAmount = subtotalAfterDiscount + supportFee + paymentFee + taxAmount;
        if (finalAmount < 500m)
        {
            finalAmount = 500m;
            notes += "minimum invoice amount applied; ";
        }

        return new InvoiceResult()
        {
            BaseAmount = baseAmount,
            DiscountAmount = discountAmount,
            FinalAmount = finalAmount,
            Notes = notes,
            PaymentFee = paymentFee,
            SupportFee = supportFee,
            TaxAmount = taxAmount
        };
    }
}