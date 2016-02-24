using Microsoft.AspNet.Mvc;
using System.Linq;
using WebApi.Hal.Web.Api.Resources;
using WebApi.Hal.Web.Data;

namespace WebApi.Hal.Web.Api
{
    public class BreweriesController : Controller
    {
        readonly BeerDbContext beerDbContext;

        public BreweriesController(BeerDbContext beerDbContext)
        {
            this.beerDbContext = beerDbContext;
        }

        public BreweryListRepresentation Get()
        {
            var breweries = beerDbContext.Styles
                .ToList()
                .Select(s => new BreweryRepresentation
                {
                    Id = s.Id,
                    Name = s.Name
                })
                .ToList();

            return new BreweryListRepresentation(breweries);
        }

        public BreweryRepresentation Get(int id)
        {
            var brewery = beerDbContext.Breweries.FirstOrDefault(b => b.Id == id);
            return new BreweryRepresentation
            {
                Id = brewery.Id,
                Name = brewery.Name
            };
        }
    }
}