using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
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

        public List<Forum> GetByCreatorId(int id)
        {
            Load();
            return forums.Where(f => f.CreatorId == id).ToList();
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
        public void Update(Forum forum)
        {
            Load();
            int existingIndx = forums.FindIndex(f => f.Location == forum.Location);

            forums[existingIndx].Open = forum.Open;
            forums[existingIndx].CreatorId = forum.CreatorId;
            forums[existingIndx].VeryUseful = forum.VeryUseful;
            Save();
        }
    }
}
