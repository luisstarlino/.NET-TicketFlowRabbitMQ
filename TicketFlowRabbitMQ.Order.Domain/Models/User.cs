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
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public string Phone { get; private set; }
        public DateTime BirthDate { get; private set; }

        // --- Private Constructor to ensure that the only way to create it is using the factory.
        private User() { }

        public static User Create(string name, string email, string password, string phone, int Age, string birthDate)
        {
            string hash = String.Empty;
            DateTime.TryParseExact(birthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime birthDateObj);
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
