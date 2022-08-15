using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tedee.Models;
using Tedee.Models.Enums;
using Tedee.Repositories.Interfaces;

namespace Tedee.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly TedeeContext _context;

        public TripRepository(TedeeContext context)
        {
            _context = context;
        }

        public IQueryable<Trip> GetTrips()
        {
            return _context.Trips;
        }

        public IQueryable<Trip> GetTripsWithCountryId(int countryId)
        {
            return _context.Trips.WithCountryId(countryId);
        }

        public IQueryable<Trip?> GetTrip(Guid id)
        {
            return GetTrips().WithId(id);
        }
        public IQueryable<Trip?> GetTripWithEmails(Guid id)
        {
            return GetTrips().Include(x=>x.Emails).WithId(id);
        }

        public bool TripExists(Guid id)
        {
            return (GetTrips()?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public bool TripByNameExists(string name)
        {
            return (GetTrips()?.Any(e => e.Name == name)).GetValueOrDefault();
        }


        public bool TripByCountryExists(int countryId)
        {
            return (GetTrips()?.Any(x => x.Country == (Country)countryId)).GetValueOrDefault();
        }
    }
    public static class TripExtensions
    {
        public static IQueryable<Trip> WithId(this IQueryable<Trip> query, Guid id)
        {
            return query.Where(x => x.Id == id);
        }

        public static IQueryable<Trip> WithCountryId(this IQueryable<Trip> query, int countryId)
        {
            return query.Where(x => (int)x.Country == countryId);
        }

    }
}
