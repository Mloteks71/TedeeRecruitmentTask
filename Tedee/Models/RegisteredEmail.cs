using System.Text.Json.Serialization;

namespace Tedee.Models
{
    public class RegisteredEmail : BaseRegisteredEmail
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public ICollection<Trip> Trips { get; set; }

        public RegisteredEmail()
        {

        }

        public RegisteredEmail(BaseRegisteredEmail baseEmail)
        {
            Email = baseEmail.Email;
        }
    }
}
