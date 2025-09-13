using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TicketFlowRabbitMQ.Order.Domain.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public DateTime BirthDate { get; set; }

        // --- Private Constructor to ensure that the only way to create it is using the factory.
        private User() { }

        public static User Create(string name, string email, string password, string phone, string birthDate)
        {
            string hash = String.Empty;
            DateTime.TryParseExact(birthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out DateTime birthDateObj);
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                hash = Convert.ToBase64String(hashBytes); // Base64 representation

            }


            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Password = hash,
                Phone = phone,
                BirthDate = birthDateObj
            };

            return newUser;
        }
    }
}
