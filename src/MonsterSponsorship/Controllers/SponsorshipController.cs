using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MonsterSponsorship.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonsterSponsorship.Controllers
{
    public class SponsorshipController : Controller
    {
        private readonly StripeOptions _stripeOptions;
        private readonly BusinessOptions _businessOptions;

        public SponsorshipController(IOptions<StripeOptions> stripeOptions, IOptions<BusinessOptions> businessOptions)
        {
            _stripeOptions = stripeOptions.Value;
            _businessOptions = businessOptions.Value;
        }

        public IActionResult Index(int numberOfEpisodes = 5, string sponsorName = "")
        {
            var model = new SponsorshipModel
            {
                Email = string.Empty,
                Name = sponsorName,
                NumberOfEpisodes = Math.Max(1, numberOfEpisodes),
                Notes = string.Empty,
                Tagline = string.Empty,
                Website = string.Empty
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Review(SponsorshipModel model)
        {
            if (ModelState.IsValid)
            {
                //Can't trust the totals that come in so reset those
                var subTotal = (_businessOptions.PricePerEpisode * model.NumberOfEpisodes);
                var gst = (subTotal * 0.05m);
                var total = Math.Round((subTotal + gst), 2);
                var correctedAmountModel = new SponsorshipModel
                {
                    Email = model.Email,
                    Name = model.Name,
                    NumberOfEpisodes = model.NumberOfEpisodes,
                    Tagline = model.Tagline,
                    Website = model.Website,
                    Notes = model.Notes,
                    Gst = gst,
                    SubTotal = subTotal,
                    Total = (subTotal + gst),
                    StripeAmount = (int)(total * 100),
                    StripeKey = _stripeOptions.Key
                };
                return View(correctedAmountModel);
            }
            return View("Index", model);
        }


        public IActionResult Complete(CompleteModel model)
        {
            StripeConfiguration.SetApiKey(_stripeOptions.SecretKey);

            var myCharge = new StripeChargeCreateOptions();

            // find the customer with stripe
            var customerService = new StripeCustomerService();
            var customer = customerService.List().FirstOrDefault(c => c.Email == model.Email);

            if (customer == null)
            {
                var newCustomer = new StripeCustomerCreateOptions
                {
                    Email = model.Email,
                    Description = model.Name,
                    Metadata = new Dictionary<string, string>() {
                        { "Website", model.Website},
                        { "TagLine", model.Tagline },
                        { "Notes", model.Notes }
                    }
                };
                customer = customerService.Create(newCustomer);
            }

            // always set these properties
            var numberOfEpisodes = model.NumberOfEpisodes;
            var subTotal = (_businessOptions.PricePerEpisode * model.NumberOfEpisodes);
            var gst = (subTotal * 0.05m);
            var total = Math.Round((subTotal + gst), 2);

            model.SubTotal = subTotal;
            model.Gst = gst;
            model.Total = total;


            myCharge.Amount = (int) (total * 100);
            myCharge.Currency = "cad";
            myCharge.Metadata = new Dictionary<string, string>()
            {
                {"CustomerId", customer.Id },
                {"CustomerEmail", model.Email }
            };

            // set this if you want to
            myCharge.Description = "ASP.NET Monsters Sponsorship";
            myCharge.Source = new StripeSourceOptions { TokenId = model.StripeToken };

            // (not required) set this to false if you don't want to capture the charge yet - requires you call capture later
            myCharge.Capture = true;

            var chargeService = new StripeChargeService();
            StripeCharge stripeCharge = chargeService.Create(myCharge);

            return View(model);
        }


    }
}
