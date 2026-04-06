using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.BillingGateways;

public class BillingGatewayWrapper : IBillingGateway
{
    public void SaveInvoice(RenewalInvoice invoice)
    {
        LegacyBillingGateway.SaveInvoice(invoice);
    }

    public void SendEmail(string to, string subject, string body)
    {
        LegacyBillingGateway.SendEmail(to, subject, body);
    }
}