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

        public List<Forum> GetByCreatorId(int id)
        {
            return _forumRepository.GetByCreatorId(id);
        }

        public void Load()
        {
            _forumRepository.Load();
        }

        public void Add(Forum forum)
        {
            _forumRepository.Add(forum);
            Save();
        }
        public void Save()
        {
            _forumRepository.Save();
        }
        public int MakeId()
        {
            return _forumRepository.MakeId();
        }
        public void Update(Forum forum)
        {
            _forumRepository.Update(forum);
            Save();
        }
    }
}
