using booking.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.RepositoryInterfaces
{
    internal interface IUserRepository
    {
        List<User> GetAll();
        User GetByUsername(string userName);
        string GetUserNameById(int id);
        void UpdateById(int id, bool b);
    }
}
