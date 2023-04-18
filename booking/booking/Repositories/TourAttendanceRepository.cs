using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Domain.RepositoryInterfaces;

namespace booking.Repository
{
    public class TourAttendanceRepository:ITourAttendanceRepository
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
            serializer.FromCSV(fileName);
            attendance = serializer.FromCSV(fileName);
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
        public TourAttendance GetById(int index) 
        {
            foreach (TourAttendance ta in attendance)
            {
                if(ta.Id == index)
                    return ta;
            }
            return null;
        }
        public int MakeID()
        {
            if(attendance.Count>0)
                return attendance[attendance.Count - 1].Id + 1;
            return 1;
        }
    }
}
