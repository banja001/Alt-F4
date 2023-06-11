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
        public bool SuperGuide { get; set; }
        public string SuperGuideLanguage { get; set; }

        public bool Tutorial { get; set; }
        public bool IsQuit { get; set; }
        public User() 
        {
        }

        public User(string username, string password, string role, bool super=false, 
            DateTime? dateOfBecomingSuper = null, int score = 0, int numOfAccommodationReservations = 0,bool tut=true)
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
            SuperGuide = false;
            SuperGuideLanguage = "";
            Tutorial = tut;
            IsQuit = false;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Username, Password, Role ,Super.ToString(),
                DateOfBecomingSuper.ToString("dd/MM/yyyy"), Score.ToString(), NumOfAccommodationReservations.ToString(),SuperGuide.ToString(),SuperGuideLanguage,Tutorial.ToString().ToLower(),IsQuit.ToString()  };
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
            SuperGuide = Convert.ToBoolean(values[8]);
            SuperGuideLanguage = values[9];
            Tutorial= values[10] == "true" ? true : false;
            IsQuit = Convert.ToBoolean(values[11]);
        }

        public string IsSuper(bool superGuide)
        {
            if (superGuide)
            {
                SuperGuide = true;

                return "Yes";
            }
            else
            {
                SuperGuide = false;
                return "No";

            }
        }
        

    }
}
