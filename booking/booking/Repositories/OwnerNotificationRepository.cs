using booking.Domain.Model;
using booking.Model;
using booking.Serializer;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class OwnerNotificationRepository
    {
        private List<OwnerNotification> ownerNotifications;
        private Serializer<OwnerNotification> serializer;

        private readonly string fileName = "../../../Resources/Data/ownerNotifications.csv";

        public OwnerNotificationRepository()
        {
            serializer = new Serializer<OwnerNotification>();
            ownerNotifications = serializer.FromCSV(fileName);
        }

        public List<OwnerNotification> GetAll()
        {
            return ownerNotifications;
        }

        public void Add(OwnerNotification notification)
        {
            ownerNotifications.Add(notification);
            Save();
        }

        public void Save()
        {
            serializer.ToCSV(fileName, ownerNotifications);
        }

        public void DeleteAllByOwnerId(int id)
        {
            ownerNotifications = ownerNotifications.Where(n => n.OwnerId != id).ToList();
            Save();
        }

        public int MakeId()
        {
            return ownerNotifications.Count == 0 ? 1 : ownerNotifications.Max(n => n.Id) + 1;
        }
    }
}
