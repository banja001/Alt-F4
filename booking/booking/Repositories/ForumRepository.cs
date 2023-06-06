using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class ForumRepository : IForumRepository
    {
        private List<Forum> forums;
        private Serializer<Forum> serializer;
        private readonly string fileName = "../../../Resources/Data/forums.csv";

        public ForumRepository()
        {
            serializer = new Serializer<Forum>();
            Load();
        }

        public List<Forum> GetAll()
        {
            Load();
            return forums;
        }

        public void Load()
        {
            forums = serializer.FromCSV(fileName);
        }

        public void Add(Forum forum)
        {
            forums.Add(forum);
            Save();
        }
        public void Save()
        {
            serializer.ToCSV(fileName, forums);
        }
        public int MakeId()
        {
            return forums.Count == 0 ? 1 : forums.Max(f => f.Id) + 1;
        }
    }
}
