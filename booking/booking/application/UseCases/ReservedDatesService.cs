using booking.Injector;
using booking.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class ReservedDatesService
    {
        private readonly IReservedDatesRepository _reservedDatesRepository;

        public ReservedDatesService()
        {
            _reservedDatesRepository = Injector.CreateInstance<IReservedDatesRepository>();
        }
        public List<ReservedDates> GetAll()
        {
            return _reservedDatesRepository.GetAll();
        }
        public List<ReservedDates> GetAllCanceled()
        {
            return _reservedDatesRepository.GetAllCanceled();
        }
        public List<ReservedDates> GetAllByAccommodationId(int id)
        {
            return _reservedDatesRepository.GetAllByAccommodationId(id);
        }
        public ReservedDates GetById(int id)
        {
            return _reservedDatesRepository.GetById(id);
        }

        public List<ReservedDates> GetByGuestId(int guestId)
        {
            return _reservedDatesRepository.GetByGuestId(guestId);
        }
        public void Update(ReservedDates reservedDate)
        {
            _reservedDatesRepository.Update(reservedDate);
        }
        public int MakeId()
        {
            return _reservedDatesRepository.MakeId();
        }
        public void Add(ReservedDates reservedDate)
        {
            _reservedDatesRepository.Add(reservedDate);
        }
        public void AddCanceled(ReservedDates reservedDate)
        {
            _reservedDatesRepository.AddCanceled(reservedDate);
        }
        public void Remove(ReservedDates reservedDate)
        {
            _reservedDatesRepository.Remove(reservedDate);
        }
        public void Save()
        {
            _reservedDatesRepository.Save();
        }
        public void Delete(ReservedDates reservedDate)
        {
            _reservedDatesRepository.Delete(reservedDate);
        }
        public void UpdateRating(int id)
        {
            _reservedDatesRepository.UpdateRating(id);
        }
    }
}
