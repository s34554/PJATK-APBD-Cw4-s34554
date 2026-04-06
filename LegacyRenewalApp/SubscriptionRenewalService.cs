using System;
using LegacyRenewalApp.BillingGateways;
using LegacyRenewalApp.Calculators;
using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Models;
using LegacyRenewalApp.Notifiers;
using LegacyRenewalApp.Repositories;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IInvoiceCalculator _invoiceCalculator;
        private readonly IBillingGateway _billingGateway;
        private readonly IInvoiceNotifier _invoiceNotifier;

        public SubscriptionRenewalService()
        { 
            var gateway = new BillingGatewayWrapper();
            _customerRepository = new CustomerRepository();
            _subscriptionPlanRepository = new SubscriptionPlanRepository();
            _invoiceCalculator = new InvoiceCalculator(
                new DiscountCalculator(),
                new SupportFeeCalculator(),
                new PaymentFeeCalculator(),
                new TaxCalculator());
            _billingGateway = gateway;
            _invoiceNotifier = new EmailInvoiceNotifier(gateway);
        }

        public SubscriptionRenewalService(
            ICustomerRepository customerRepository,
            ISubscriptionPlanRepository planRepository,
            IInvoiceCalculator invoiceCalculator,
            IBillingGateway billingGateway,
            IInvoiceNotifier invoiceNotifier)
        {
            _customerRepository = customerRepository;
            _subscriptionPlanRepository = planRepository;
            _invoiceCalculator = invoiceCalculator;
            _billingGateway = billingGateway;
            _invoiceNotifier = invoiceNotifier;
        }
        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            InputValidator.ValidateRenewalInputs(customerId, planCode, seatCount, paymentMethod);

            var normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            var normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

            var customer = _customerRepository.GetById(customerId);
            var plan = _subscriptionPlanRepository.GetByCode(normalizedPlanCode);

            if (!customer.IsActive)
            {
                throw new InvalidOperationException("Inactive customers cannot renew subscriptions");
            }
            
            var invoiceResult = _invoiceCalculator.Calculate(customer, plan, seatCount, normalizedPaymentMethod,
                includePremiumSupport, useLoyaltyPoints);
            
            var invoice = new RenewalInvoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customerId}-{normalizedPlanCode}",
                CustomerName = customer.FullName,
                PlanCode = normalizedPlanCode,
                PaymentMethod = normalizedPaymentMethod,
                SeatCount = seatCount,
                BaseAmount = Math.Round(invoiceResult.BaseAmount, 2, MidpointRounding.AwayFromZero),
                DiscountAmount = Math.Round(invoiceResult.DiscountAmount, 2, MidpointRounding.AwayFromZero),
                SupportFee = Math.Round(invoiceResult.SupportFee, 2, MidpointRounding.AwayFromZero),
                PaymentFee = Math.Round(invoiceResult.PaymentFee, 2, MidpointRounding.AwayFromZero),
                TaxAmount = Math.Round(invoiceResult.TaxAmount, 2, MidpointRounding.AwayFromZero),
                FinalAmount = Math.Round(invoiceResult.FinalAmount, 2, MidpointRounding.AwayFromZero),
                Notes = invoiceResult.Notes.Trim(),
                GeneratedAt = DateTime.UtcNow
            };

            _billingGateway.SaveInvoice(invoice);
            _invoiceNotifier.Notify(customer, invoice);

            return invoice;
        }
    }
}
