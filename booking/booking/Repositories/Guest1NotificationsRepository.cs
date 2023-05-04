using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class Guest1NotificationsRepository : IGuest1NotificationsRepository
    {
        private List<Guest1Notifications> guest1Notifications;
        private Serializer<Guest1Notifications> serializer;

        public readonly string fileName = "../../../Resources/Data/guest1Notifications.csv";

        public Guest1NotificationsRepository()
        {
            serializer = new Serializer<Guest1Notifications>();
            Load();
        }
        public void Load()
        {
            guest1Notifications = serializer.FromCSV(fileName);
        }
        public List<Guest1Notifications> GetAll()
        {
            return serializer.FromCSV(fileName);
        }

        public List<Guest1Notifications> GetAllByGuest1Id(int id)
        {
            Load();
            return guest1Notifications.FindAll(x => x.Guest1Id == id);
        }

        public void RemoveByGuest1Id(int id)
        {
            Load();
            guest1Notifications = guest1Notifications.Where(n => n.Guest1Id != id).ToList();
            Save();
        }

        public void Add(Guest1Notifications notification)
        {
            Load();
            guest1Notifications.Add(notification);
            Save();
        }

        public void Save()
        {
            serializer.ToCSV(fileName, guest1Notifications);
        }

        public int MakeId()
        {
            Load();
            return guest1Notifications.Count == 0 ? 0 : guest1Notifications.Max(n => n.Id) + 1;
        }
    }
}
