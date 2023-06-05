using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
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
    }
}
