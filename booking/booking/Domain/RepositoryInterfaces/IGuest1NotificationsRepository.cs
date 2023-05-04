using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IGuest1NotificationsRepository
    {
        public void Load();
        public List<Guest1Notifications> GetAll();
        public List<Guest1Notifications> GetAllByGuest1Id(int id);
        public void RemoveByGuest1Id(int id);
        public void Add(Guest1Notifications notification);
        public void Save();
        public int MakeId();
    }
}
