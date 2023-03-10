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
        public string Grad { get; set; }
        public string Drzava { get; set; }

        public Location(int id, string grad, string drzava)
        {
            Id = id;
            Grad = grad;
            Drzava = drzava;
        }

        public Location()
        {
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), Grad, Drzava };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Grad = values[1];
            Drzava = values[2];
        }
    }
}
