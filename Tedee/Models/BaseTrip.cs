using System.ComponentModel;
using Tedee.Models.Enums;

namespace Tedee.Models
{
    public class BaseTrip
    {
        public string Name { get; set; }
        public Country Country { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        [DefaultValue(1)]
        public uint SeatsCount { get; set; }
        public BaseTrip()
        {

        }

        public BaseTrip(BaseTrip baseTrip)
        {
            Name = baseTrip.Name;
            Country = baseTrip.Country;
            Description = baseTrip.Description;
            StartDate = baseTrip.StartDate;
            SeatsCount = baseTrip.SeatsCount;
        }

        public BaseTrip(string name, Country country, string description, DateTime startDate, uint seatsCount)
        { 
            Name = name;
            Country = country;
            Description = description;
            StartDate = startDate;
            SeatsCount = seatsCount;
        }
    }
}
