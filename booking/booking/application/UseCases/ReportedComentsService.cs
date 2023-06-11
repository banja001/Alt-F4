using booking.Injector;
using Domain.Model;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class ReportedComentsService
    {
        private readonly IReportedComentsRepository _forumCommentRepository;

        public ReportedComentsService()
        {
            _forumCommentRepository = Injector.CreateInstance<IReportedComentsRepository>();
        }

        public List<ReportedComents> GetAll()
        {
            Load();
            return _forumCommentRepository.GetAll();
        }

        public void Load()
        {
            _forumCommentRepository.Load();
        }
        public void Add(ReportedComents comment)
        {
            _forumCommentRepository.Add(comment);
        }
        public void Save()
        {
            _forumCommentRepository.Save();
        }
        public int MakeId()
        {
            return _forumCommentRepository.MakeId();
        }
    }
}
