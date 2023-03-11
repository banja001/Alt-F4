using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    internal class CheckPoint: ISerializable
    {
        int Id { get; set; }
        string Name { get; set; }
        bool Active { get; set; }
        int TourId { get; set; }
        int Order { get; set; }

        public CheckPoint() 
        {

        }

        public CheckPoint(int id, string name, bool active, int tourId, int order)
        {
            Id = id;
            Name = name;
            Active = active;
            TourId = tourId;
            Order = order;
        }

        public string[] ToCSV()
        {
            string[] csvValues = { 
                                    Id.ToString(),
                                    Name.ToString(),
                                    Active.ToString(),
                                    TourId.ToString(),
                                    Order.ToString(),
                                 };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id= Convert.ToInt32(values[0]);
            Name= Convert.ToString(values[1]);
            Active = Convert.ToBoolean(values[2]);
            TourId = Convert.ToInt32(values[3]); 
            Order = Convert.ToInt32(values[4]);
        }
    }
}
