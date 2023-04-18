using booking.Model;
using booking.Serializer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface ITourAttendanceRepository
    {
        public List<TourAttendance> GetAll();
        public int GetNextIndex();
        public void Add(TourAttendance tourAttendance);
        public TourAttendance GetById(int index);
        public int MakeID();
    
    }
}
