using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public class Location : ISerializable
    {

        public int Id { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public Location(int id, string city, string state)
        {
            Id = id;
            City = city;
            State = state;

        }

        public Location()
        {
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), City, State };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            City = values[1];
            State = values[2];
        }
        public string CityState
        {
            get { return City + ", " + State; }
        }

    
    }
}
