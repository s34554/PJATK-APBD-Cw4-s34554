using System;
using LegacyRenewalApp.Calculators;
using LegacyRenewalApp.Interfaces;
using LegacyRenewalApp.Models;
using LegacyRenewalApp.Repositories;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly IInvoiceCalculator _invoiceCalculator;

        public SubscriptionRenewalService()
            : this(
                new CustomerRepository(),
                new SubscriptionPlanRepository(),
                new InvoiceCalculator(
                    new DiscountCalculator(),
                    new SupportFeeCalculator(),
                    new PaymentFeeCalculator(),
                    new TaxCalculator()))
        { }

        public SubscriptionRenewalService(
            ICustomerRepository customerRepository,
            ISubscriptionPlanRepository planRepository,
            IInvoiceCalculator invoiceCalculator)
        {
            _customerRepository = customerRepository;
            _subscriptionPlanRepository = planRepository;
            _invoiceCalculator = invoiceCalculator;
        }
        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            ValidateInputs(customerId, planCode, seatCount, paymentMethod);

            var normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            var normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

            var customer = _customerRepository.GetById(customerId);
            var plan = _subscriptionPlanRepository.GetByCode(normalizedPlanCode);

            if (!customer.IsActive)
            {
                throw new InvalidOperationException("Inactive customers cannot renew subscriptions");
            }
            
            var invoiceResult = _invoiceCalculator.Calculate(customer, plan, seatCount, paymentMethod,
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

            LegacyBillingGateway.SaveInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                string subject = "Subscription renewal invoice";
                string body =
                    $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode} " +
                    $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

                LegacyBillingGateway.SendEmail(customer.Email, subject, body);
            }

            return invoice;
        }

        private static void ValidateInputs(int customerId, string planCode, int seatCount, string paymentMethod)
        {
            if (customerId <= 0)
            {
                throw new ArgumentException("Customer id must be positive");
            }

            if (string.IsNullOrWhiteSpace(planCode))
            {
                throw new ArgumentException("Plan code is required");
            }

            if (seatCount <= 0)
            {
                throw new ArgumentException("Seat count must be positive");
            }

            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                throw new ArgumentException("Payment method is required");
            }
        }
    }
}
