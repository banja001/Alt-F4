using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class TourAttendanceRepository
    {
        private List<TourAttendance> attendance;
        private Serializer<TourAttendance> serializer;

        private readonly string fileName = "../../../Resources/Data/tourattendance.csv";

        public TourAttendanceRepository()
        {
            serializer = new Serializer<TourAttendance>();
            attendance = serializer.FromCSV(fileName);
        }

        public List<TourAttendance> GetAll()
        {
            return attendance;
        }
        public int GetNextIndex()
        {
            return attendance.Count() + 1;
        }
        public void Add(TourAttendance tourAttendance)
        {
            attendance.Add(tourAttendance);
            serializer.ToCSV(fileName, attendance);
        }
    }
}
