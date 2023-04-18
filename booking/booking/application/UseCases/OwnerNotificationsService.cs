using booking.Domain.Model;
using booking.Injector;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class OwnerNotificationsService
    {
        private readonly IOwnerNotificationRepository _ownerNotificationRepository;

        public OwnerNotificationsService()
        {
            _ownerNotificationRepository = Injector.CreateInstance<IOwnerNotificationRepository>();
        }

        public List<OwnerNotification> GetAll()
        {
            return _ownerNotificationRepository.GetAll();
        }
        public void Add(OwnerNotification notification)
        {
            _ownerNotificationRepository.Add(notification);
        }

        public void Save()
        {
            _ownerNotificationRepository.Save();
        }

        public void DeleteAllByOwnerId(int id)
        {
            _ownerNotificationRepository.DeleteAllByOwnerId(id);
        }
        public int MakeId()
        {
            return _ownerNotificationRepository.MakeId();
        }

    }
}
