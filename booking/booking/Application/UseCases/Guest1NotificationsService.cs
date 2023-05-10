using booking.Injector;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace application.UseCases
{
    public class Guest1NotificationsService
    {
        private IGuest1NotificationsRepository guest1NotificationsRepository;

        public Guest1NotificationsService()
        {
            guest1NotificationsRepository = Injector.CreateInstance<IGuest1NotificationsRepository>();
        }

        public void Load()
        {
            guest1NotificationsRepository.Load();
        }
        public List<Guest1Notifications> GetAll()
        {
            return guest1NotificationsRepository.GetAll();
        }
        public List<Guest1Notifications> GetAllByGuest1Id(int id)
        {
            return guest1NotificationsRepository.GetAllByGuest1Id(id);
        }
        public void RemoveByGuest1Id(int id)
        {
            guest1NotificationsRepository.RemoveByGuest1Id(id);
        }
        public void Add(Guest1Notifications notification)
        {
            guest1NotificationsRepository.Add(notification);
        }
        public void Save()
        {
            guest1NotificationsRepository.Save();
        }
        public int MakeId()
        {
            return guest1NotificationsRepository.MakeId();
        }
    }
}
