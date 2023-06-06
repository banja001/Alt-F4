using booking.Domain.Model;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace booking.application.UseCases
{
    public class ForumNotificationService
    {
        private readonly IForumNotificationRepository _forumNotificationRepository;

        public ForumNotificationService()
        {
            _forumNotificationRepository = Injector.Injector.CreateInstance<IForumNotificationRepository>();
        }

        public List<ForumNotification> GetAll()
        {
            return _forumNotificationRepository.GetAll();
        }
        public void Add(ForumNotification notification)
        {
            _forumNotificationRepository.Add(notification);
        }

        public void Save()
        {
            _forumNotificationRepository.Save();
        }

        public void DeleteAllByOwnerId(int id)
        {
            _forumNotificationRepository.DeleteAllByOwnerId(id);
        }
        public int MakeId()
        {
            return _forumNotificationRepository.MakeId();
        }


    }
}
