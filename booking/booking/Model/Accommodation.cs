using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using booking.Serializer;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Location { get; set; }
        public string Type { get; set; }

        public int MaxCapacity { get; set; }

        public int MinDaysToUse { get; set; }
        public int MinDaysToCancel { get; set; }
        public Accommodation() { }
        public Accommodation(int id, string name,string loc ,string type, int maxCapacity, int minDaysToUse, int minDaysToCancel)
        {
            Id = id;
            Name = name;
            Location = loc;
            Type = type;
            MaxCapacity = maxCapacity;
            MinDaysToUse = minDaysToUse;
            MinDaysToCancel = minDaysToCancel;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Name,Location, Type, MaxCapacity.ToString(),MinDaysToUse.ToString(),MinDaysToCancel.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = values[1];
            Location = values[2];
            Type = values[3];
            MaxCapacity = Convert.ToInt32(values[4]);
            MinDaysToUse = Convert.ToInt32(values[5]);
            MinDaysToCancel = Convert.ToInt32(values[6]);
        }
        
    }
}
