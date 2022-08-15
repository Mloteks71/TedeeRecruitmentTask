using System.Text.Json.Serialization;

namespace Tedee.Models
{
    public class BaseRegisteredEmail
    {
        public string Email { get; set; }
        public BaseRegisteredEmail()
        {

        }

        public BaseRegisteredEmail(string email)
        {
            Email = email;
        }
    }
}
