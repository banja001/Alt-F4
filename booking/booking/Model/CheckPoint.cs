using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Model
{
    internal class CheckPoint
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
    }
}
