using LegacyRenewalApp.Models;

namespace LegacyRenewalApp.Interfaces;

public interface ISubscriptionPlanRepository
{
    public SubscriptionPlan GetByCode(string code);
}