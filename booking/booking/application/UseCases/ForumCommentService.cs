using booking.Injector;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class ForumCommentService
    {
        private readonly IForumCommentRepository _forumCommentRepository;

        public ForumCommentService()
        {
            _forumCommentRepository = Injector.CreateInstance<IForumCommentRepository>();
        }

        public List<ForumComment> GetAll()
        {
            return _forumCommentRepository.GetAll();
        }
        public List<ForumComment> GetByForumId(int id)
        {
            return _forumCommentRepository.GetByForumId(id);
        }

        public void Load()
        {
            _forumCommentRepository.Load();
        }
        public void Add(ForumComment forum)
        {
            _forumCommentRepository.Add(forum);
        }
        public void Save()
        {
            _forumCommentRepository.Save();
        }
        public int MakeId()
        {
            return _forumCommentRepository.MakeId();
        }

        public void Update(int id)
        {
            _forumCommentRepository.Update(id);
        }
    }
}
