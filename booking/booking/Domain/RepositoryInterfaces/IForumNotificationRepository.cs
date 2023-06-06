using booking.Domain.Model;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IForumNotificationRepository
    {
        public void Load();
        public List<ForumNotification> GetAll();
        public void Add(ForumNotification notification);
        public void Save();
        public void DeleteAllByOwnerId(int id);
        public int MakeId();
    }
}
