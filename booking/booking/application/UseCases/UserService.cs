using booking.Injector;
using booking.Model;
using booking.Repository;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace application.UseCases
{
    public class UserService
    {
        IUserRepository userRepository;

        public UserService()
        {
            this.userRepository = Injector.CreateInstance<IUserRepository>();
        }

        public List<User> GetAll()
        {
            return userRepository.GetAll();
        }
        public User GetByUsername(string userName)
        {
            return userRepository.GetByUsername(userName);
        }
        public string GetUserNameById(int id)
        {
            return userRepository.GetUserNameById(id);
        }
        public void UpdateById(int id, bool b)
        {
            userRepository.UpdateById(id, b);
        }
    }
}
