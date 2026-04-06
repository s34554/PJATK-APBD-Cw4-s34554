using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Interfaces;

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