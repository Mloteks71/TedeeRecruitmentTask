using Tedee.Models.Enums;
using Tedee.Models;

namespace Tedee.Controllers.Trips.Dto
{
    public class TripNoDescription
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public DateTime StartDate { get; set; }
        public uint SeatsCount { get; set; }
        public TripNoDescription()
        {

        }

        public TripNoDescription(Trip baseTrip)
        {
            Id = baseTrip.Id;
            Name = baseTrip.Name;
            Country = baseTrip.Country;
            StartDate = baseTrip.StartDate;
            SeatsCount = baseTrip.SeatsCount;
        }

        public TripNoDescription(Guid id, string name, Country country, DateTime startDate, uint seatsCount)
        {
            Id = id;
            Name = name;
            Country = country;
            StartDate = startDate;
            SeatsCount = seatsCount;
        }
    }
}
