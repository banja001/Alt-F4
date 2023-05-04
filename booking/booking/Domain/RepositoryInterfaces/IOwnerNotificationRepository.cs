using booking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IOwnerNotificationRepository
    {
        public void Load();
        public List<OwnerNotification> GetAll();
        public void Add(OwnerNotification notification);
        public void Save();
        public void DeleteAllByOwnerId(int id);
        public int MakeId();
    }
}
