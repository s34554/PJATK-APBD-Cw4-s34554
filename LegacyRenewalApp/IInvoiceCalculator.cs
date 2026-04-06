namespace LegacyRenewalApp;

public interface IInvoiceCalculator
{
    InvoiceResult Calculate(
        Customer customer,
        SubscriptionPlan plan,
        int seatCount,
        string paymentMethod,
        bool includePremiumSupport,
        bool useLoyaltyPoints);
}