using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public class CheckPoint: ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool NotChecked { get; set; }
        public int TourId { get; set; }
        public int Order { get; set; }

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
            NotChecked = !active;
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
