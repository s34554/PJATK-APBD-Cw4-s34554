using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Notifiers;

public class EmailInvoiceNotifier : IInvoiceNotifier
{
    private readonly IBillingGateway _billingGateway;
    
    public EmailInvoiceNotifier(IBillingGateway billingGateway)
    {
        _billingGateway = billingGateway;
    }
    
    public void Notify(Customer customer, RenewalInvoice invoice)
    {
        if (string.IsNullOrWhiteSpace(customer.Email)) return;
        string subject = "Subscription renewal invoice";
        string body =
            $"Hello {customer.FullName}, your renewal for plan {invoice.PlanCode} " +
            $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

        _billingGateway.SendEmail(customer.Email, subject, body);
    }
}