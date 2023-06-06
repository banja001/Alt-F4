﻿using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repositories
{
    public class ForumCommentRepository : IForumCommentRepository
    {
        private List<ForumComment> forumComments;
        private Serializer<ForumComment> serializer;

        private readonly string fileName = "../../../Resources/Data/forumComments.csv";

        public ForumCommentRepository()
        {
            serializer = new Serializer<ForumComment>();
            Load();
        }
        public List<ForumComment> GetAll()
        {
            Load();
            return forumComments;
        }

        public void Load()
        {
            forumComments = serializer.FromCSV(fileName);
        }
        public void Add(ForumComment forum)
        {
            forumComments.Add(forum);
        }
        public void Save()
        {
            serializer.ToCSV(fileName, forumComments);
        }
        public int MakeId()
        {
            return forumComments.Count == 0 ? 1 : forumComments.Max(f => f.Id) + 1;
        }
    }
}