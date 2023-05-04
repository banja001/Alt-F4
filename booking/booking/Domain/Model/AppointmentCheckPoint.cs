using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    public class AppointmentCheckPoint:ISerializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool NotChecked { get; set; }
        public int AppointmentId { get; set; }
        public int Order { get; set; }

        public AppointmentCheckPoint()
        {

        }

        public AppointmentCheckPoint(int id, string name, bool active,bool notchecked, int appointmentId, int order)
        {
            Id = id;
            Name = name;
            Active = active;
            NotChecked= notchecked;
            AppointmentId = appointmentId;
            Order = order;
            NotChecked = !active;
        }

        public string[] ToCSV()
        {
            string[] csvValues = {
                                    Id.ToString(),
                                    Name.ToString(),
                                    Active.ToString(),
                                    NotChecked.ToString(),
                                    AppointmentId.ToString(),
                                    Order.ToString(),
                                 };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToInt32(values[0]);
            Name = Convert.ToString(values[1]);
            Active = Convert.ToBoolean(values[2]);
            NotChecked= Convert.ToBoolean(values[3]);
            AppointmentId = Convert.ToInt32(values[4]);
            Order = Convert.ToInt32(values[5]);
        }
    }
}
