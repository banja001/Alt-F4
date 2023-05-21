using booking.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IReservedDatesRepository
    {
        public void Load();
        public List<ReservedDates> GetAll();
        public List<ReservedDates> GetAllCanceled();
        public List<ReservedDates> GetAllByAccommodationId(int id);
        public ReservedDates GetById(int id);
        public List<ReservedDates> GetByGuestId(int guestId);
        public void Update(ReservedDates reservedDates);
        public int MakeId();
        public void Add(ReservedDates reservedDate);
        public void Remove(ReservedDates reservedDate);
        public void Save();
        public void Delete(ReservedDates reservedDate);
        public void UpdateRating(int id);
        public void AddCanceled(ReservedDates reservedDate);
    }
}
