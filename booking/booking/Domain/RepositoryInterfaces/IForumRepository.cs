using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IForumRepository
    {
        public void Load();
        public List<Forum> GetAll();
        public List<Forum> GetByCreatorId(int id);
        public void Add(Forum forum);
        public void Save();
        public int MakeId();
        public void Update(Forum forum);
    }
}
