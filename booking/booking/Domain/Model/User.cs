using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace booking.Model
{
    public class User : ISerializable
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public bool Super { get; set; }
        public DateTime DateOfBecomingSuper { get; set; }
        public int Score { get; set; }
        public int NumOfAccommodationReservations { get; set; }
        public User() 
        {
        }

        public User(string username, string password, string role, bool super=false, 
            DateTime? dateOfBecomingSuper = null, int score = 0, int numOfAccommodationReservations = 0)
        {
            Username = username;
            Password = password;
            Role = role;
            Super = super;
            Score = score;
            if (!dateOfBecomingSuper.HasValue)
                dateOfBecomingSuper = new DateTime(0001, 01, 01);
            DateOfBecomingSuper = dateOfBecomingSuper.Value;
            NumOfAccommodationReservations = numOfAccommodationReservations;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Role ,Super.ToString(),
                DateOfBecomingSuper.ToString("dd/MM/yyyy"), Score.ToString(), NumOfAccommodationReservations.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Username = values[1];
            Password = values[2];
            Role = values[3];
            Super = Convert.ToBoolean(values[4]);
            DateOfBecomingSuper = DateTime.ParseExact(values[5], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            Score = Convert.ToInt32(values[6]);
            NumOfAccommodationReservations = Convert.ToInt32(values[7]);
        }

        public string IsSuper()
        {
            if (Super)
                return "Yes";
            else
                return "No";
        }

    }
}
