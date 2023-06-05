using booking.Injector;
using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
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
            Load();
            return _forumCommentRepository.GetAll();
        }

        public void Load()
        {
            _forumCommentRepository.Load();
        }
    }
}
