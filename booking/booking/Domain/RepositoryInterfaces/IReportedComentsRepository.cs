using Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    public interface IReportedComentsRepository
    {
        public void Load();
        public List<ReportedComents> GetAll();
        public void Add(ReportedComents comment);
        public void Save();
        public int MakeId();
        

    }
}
