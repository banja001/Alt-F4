using booking.Injector;
using booking.Model;
using booking.Repository;
using booking.Serializer;
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

        public User GetById(int id)
        {
            return userRepository.GetById(id);
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

        public int GetScoreById(int id)
        {
            return userRepository.GetScoreById(id);
        }

        public void Update(User user)
        {
            userRepository.Update(user);
        }
        public void UpdateSuperGuide(int id, bool super, string language)
        {
           userRepository.UpdateSuperGuide(id, super, language);
        }
        public void QuitJob(int id)
        {
            userRepository.QuitJob(id);
        }
    }
}
