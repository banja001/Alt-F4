using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IForumCommentRepository
    {
        public void Load();
        public List<ForumComment> GetAll();
        public List<ForumComment> GetByForumId(int id);
        public void Add(ForumComment forum);
        public void Save();
        public int MakeId();
        public void Update(int id);
    }
}
