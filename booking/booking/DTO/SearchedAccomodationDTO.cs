using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.DTO
{
    public class SearchedAccomodationDTO
    {
        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public List<string> Type { get; set; }

        public int NumOfGuests { get; set; }

        public int NumOfDays { get; set; }

        public SearchedAccomodationDTO() 
        { 
            Type = new List<string>();
        }

        public SearchedAccomodationDTO(string name, string city, string country, List<string> type, int numOfGuests, int numOfDays)
        {
            Name = name;
            City = city;
            Country = country;
            Type = type;
            NumOfGuests = numOfGuests;
            NumOfDays = numOfDays;
        }
    }
}
