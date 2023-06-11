using booking.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    internal interface IUserRepository
    {
        List<User> GetAll();
        User GetById(int id);
        User GetByUsername(string userName);
        string GetUserNameById(int id);
        void UpdateById(int id, bool b);
        int GetScoreById(int id);
        void Update(User user);
        public void QuitJob(int id);
        public void UpdateSuperGuide(int id, bool super, string language);
    }
}
