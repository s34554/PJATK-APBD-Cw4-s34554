using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Interfaces;

public interface IBillingGateway
{
    void SaveInvoice(RenewalInvoice invoice);
    void SendEmail(string to, string subject, string body);
}