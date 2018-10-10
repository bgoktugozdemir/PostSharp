using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcmeCarRental
{

    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DriversLicense { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class Vehicle
    {
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public Size Size { get; set; }
        public string Vin { get; set; }
    }

    public enum Size
    {
        Compact = 0, Midsize, FullSize, Luxury, Truck, SUV
    }

    public class RentalAgreement
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public Vehicle Vehicle { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public interface ILoyaltyAccrualService
    {
        void Accrue(RentalAgreement agreement);
    }


    public class FakeLoyaltyDataService : ILoyaltyDataService
    {
        public void AddPoints(Guid customerId, int points)
        {
            Console.WriteLine("Adding {0} points for customer '{1}'",
            points, customerId);
        }
        public void SubtractPoints(Guid customerId, int points)
        {
            Console.WriteLine("Subtracting {0} points for customer '{1}'",
            points, customerId);
        }
    }
    public interface ILoyaltyDataService
    {
        void AddPoints(Guid customerId, int points);
        void SubtractPoints(Guid customerId, int points);

    }

    public class LoyaltyAccrualService : ILoyaltyAccrualService
    {
        readonly ILoyaltyDataService _loyaltyDataService;
        public LoyaltyAccrualService(ILoyaltyDataService service)
        {
            _loyaltyDataService = service;
        }
        public void Accrue(RentalAgreement agreement)
        {
            // defensive programming
            if (agreement == null) throw new ArgumentNullException("agreement");

            // logging
            Console.WriteLine("Accrue: {0}", DateTime.Now);
            Console.WriteLine("Customer: {0}", agreement.Customer.Id);
            Console.WriteLine("Vehicle: {0}", agreement.Vehicle.Id);

            var rentalTimeSpan =
            (agreement.EndDate.Subtract(agreement.StartDate));
            var numberOfDays = (int)Math.Floor(rentalTimeSpan.TotalDays);
            var pointsPerDay = 1;
            if (agreement.Vehicle.Size >= Size.Luxury)
                pointsPerDay = 2;
            var points = numberOfDays * pointsPerDay;
            _loyaltyDataService.AddPoints(agreement.Customer.Id, points);

            // logging
            Console.WriteLine("Accrue complete: {0}", DateTime.Now);
        }
    }


    public interface ILoyaltyRedemptionService
    {
        void Redeem(Invoice invoice, int numberOfDays);
    }

    public class Invoice
    {
        public Guid Id { get; set; }
        public Customer Customer { get; set; }
        public Vehicle Vehicle { get; set; }
        public decimal CostPerDay { get; set; }
        public decimal Discount { get; set; }
    }

    public class LoyaltyRedemptionService : ILoyaltyRedemptionService
    {
        readonly ILoyaltyDataService _loyaltyDataService;
        public LoyaltyRedemptionService(ILoyaltyDataService service)
        {
            _loyaltyDataService = service;
        }
        public void Redeem(Invoice invoice, int numberOfDays)
        {
            // defensive programming
            if (invoice == null) throw new ArgumentNullException("invoice");
            if (numberOfDays <= 0)
                throw new ArgumentException("", "numberOfDays");
            // logging
            Console.WriteLine("Redeem: {0}", DateTime.Now);
            Console.WriteLine("Invoice: {0}", invoice.Id);


            var pointsPerDay = 10;
            if (invoice.Vehicle.Size >= Size.Luxury)
                pointsPerDay = 15;
            var points = numberOfDays * pointsPerDay;
            _loyaltyDataService.SubtractPoints(invoice.Customer.Id, points);
            invoice.Discount = numberOfDays * invoice.CostPerDay;

            // logging
            Console.WriteLine("Redeem complete: {0}", DateTime.Now);
        }
    }
}
