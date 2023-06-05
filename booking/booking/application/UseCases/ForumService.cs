using booking.Injector;
using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class ForumService
    {
        private readonly IForumRepository _forumRepository;

        public ForumService()
        {
            _forumRepository = Injector.CreateInstance<IForumRepository>();
        }

        public List<Forum> GetAll()
        {
            Load();
            return _forumRepository.GetAll();
        }

        public void Load()
        {
            _forumRepository.Load();
        }
    }
}
