using System.Text.Json.Serialization;
using Tedee.Models.Enums;
using Tedee.Models.Validators;

namespace Tedee.Models
{
    public class Trip : BaseTrip
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public ICollection<RegisteredEmail> Emails { get; set; }

        private Trip(): base()
        {
            Emails = new List<RegisteredEmail>();
        }

        public Trip(BaseTrip baseTrip) : base(baseTrip)
        {
            Emails = new List<RegisteredEmail>();
        }
    }
}
