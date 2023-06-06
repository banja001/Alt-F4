using booking.Domain.Model;
using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class ForumNotificationRepository: IForumNotificationRepository
    {
        private List<ForumNotification> forumNotifications;
        private Serializer<ForumNotification> serializer;

        private readonly string fileName = "../../../Resources/Data/forumNotification.csv";

        public ForumNotificationRepository()
        {
            serializer = new Serializer<ForumNotification>();
            Load();
        }

        public void Load()
        {
            forumNotifications = serializer.FromCSV(fileName);
        }

        public List<ForumNotification> GetAll()
        {
            return serializer.FromCSV(fileName);
        }

        public void Add(ForumNotification notification)
        {
            Load();
            forumNotifications.Add(notification);
            Save();
        }

        public void Save()
        {
            serializer.ToCSV(fileName, forumNotifications);
        }

        public void DeleteAllByOwnerId(int id)
        {
            forumNotifications = GetAll().Where(n => n.OwnerId != id).ToList();
            Save();
        }

        public int MakeId()
        {
            Load();
            return forumNotifications.Count == 0 ? 1 : forumNotifications.Max(n => n.Id) + 1;
        }
    }
}

