﻿using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IForumRepository
    {
        public void Load();
        public List<Forum> GetAll();
        public void Add(Forum forum);
        public void Save();
        public int MakeId();
    }
}