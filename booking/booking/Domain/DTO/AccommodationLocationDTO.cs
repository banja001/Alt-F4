using booking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace booking.DTO
{
    public class AccommodationLocationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Location { get; set; }
        public string Type { get; set; }

        public int MaxCapacity { get; set; }

        public int MinDaysToUse { get; set; }
        public int MinDaysToCancel { get; set; }
        public int AccommodationId { get; set; }
        public AccommodationLocationDTO() { }

        public AccommodationLocationDTO(int id, string name, string location, string type, int maxCapacity, int minDaysToUse, int minDaysToCancel, int accommodationId)
        {
            Id = id;
            Name = name;
            Location = location;
            Type = type;
            MaxCapacity = maxCapacity;
            MinDaysToUse = minDaysToUse;
            MinDaysToCancel = minDaysToCancel;
            AccommodationId = accommodationId;
        }
    }
}
