using booking.Injector;
using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace booking.application.UseCases
{
    public class ForumCommentService
    {
        private readonly IForumCommentRepository _forumCommentRepository;

        public ForumCommentService()
        {
            _forumCommentRepository = Injector.Injector.CreateInstance<ForumCommentRepository>();
        }

        public List<ForumComment> GetAll()
        {
            Load();
            return _forumCommentRepository.GetAll();
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
    }
}
