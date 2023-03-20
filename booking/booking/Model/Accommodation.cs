using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using booking.Serializer;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace booking.Model
{
    public class Accommodation : ISerializable
    {
        public int Id { get; set; }

        public int OwnerId { get; set; }
        public string Name { get; set; }

        public int LocationId { get; set; }
        public string Type { get; set; }

        public int MaxCapacity { get; set; }


        public int MinDaysToUse { get; set; }
        public int MinDaysToCancel { get; set; }

        
        public Accommodation() { }
        public Accommodation(int id,int oid, string name,int loc ,string type, int maxCapacity, int minDaysToUse, int minDaysToCancel)
        {
            Id = id;
            OwnerId = oid;
            Name = name;
            LocationId = loc;
            Type = type;
            MaxCapacity = maxCapacity;
            MinDaysToUse = minDaysToUse;
            MinDaysToCancel = minDaysToCancel;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),OwnerId.ToString() ,Name,LocationId.ToString(), Type, MaxCapacity.ToString(),MinDaysToUse.ToString(),MinDaysToCancel.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            OwnerId = Convert.ToInt32(values[1]);
            Name = values[2];
            LocationId = Convert.ToInt32(values[3]);
            Type = values[4];
            MaxCapacity = Convert.ToInt32(values[5]);
            MinDaysToUse = Convert.ToInt32(values[6]);
            MinDaysToCancel = Convert.ToInt32(values[7]);
        }

        

        
    }
}
