using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Interfaces;

public interface IInvoiceNotifier
{
    void Notify(Customer customer, RenewalInvoice invoice);
}