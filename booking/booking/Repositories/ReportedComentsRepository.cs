
using booking.Serializer;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repositories
{
    public class ReportedComentsRepository:IReportedComentsRepository
    {

        private List<ReportedComents> reportedComents;
        private Serializer<ReportedComents> serializer;

        private readonly string fileName = "../../../Resources/Data/reportedComents.csv";

        public ReportedComentsRepository()
        {
            serializer = new Serializer<ReportedComents>();
            Load();
        }
        public List<ReportedComents> GetAll()
        {
            Load();
            return reportedComents;
        }

        public void Load()
        {
            reportedComents = serializer.FromCSV(fileName);
        }
        public void Add(ReportedComents comment)
        {
            reportedComents.Add(comment);
            Save();
        }
        public void Save()
        {
            serializer.ToCSV(fileName, reportedComents);
        }
        public int MakeId()
        {
            return reportedComents.Count == 0 ? 1 : reportedComents.Max(f => f.Id) + 1;
        }
        
    }
}
