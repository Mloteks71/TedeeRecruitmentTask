using Microsoft.EntityFrameworkCore;
using Tedee.Models;
using Tedee.Models.Enums;

namespace Tedee.Repositories.Interfaces
{
    public interface ITripRepository
    {
        IQueryable<Trip> GetTrips();
        IQueryable<Trip> GetTripsWithCountryId(int countryId);
        IQueryable<Trip?> GetTrip(Guid id);
        IQueryable<Trip?> GetTripWithEmails(Guid id);
        bool TripExists(Guid id);
        bool TripByNameExists(string name);
        bool TripByCountryExists(int countryId);
    }
}