using booking.Model;
using booking.Serializer;
using Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace booking.Repository
{
    public class UserRepository: IUserRepository
    {
        private const string FilePath = "../../../Resources/Data/users.csv";

        private readonly Serializer<User> _serializer;

        private List<User> _users;

        public UserRepository()
        {
            _serializer = new Serializer<User>();
            _users = _serializer.FromCSV(FilePath);
        }
        public User GetById(int id)
        {
            return _users.Find(u => u.Id == id);
        }
        public List<User> GetAll()
        {
            return _serializer.FromCSV(FilePath);
        }

        public User GetByUsername(string userName)
        {
            _users = _serializer.FromCSV(FilePath);
            return _users.FirstOrDefault(u => u.Username == userName);
        }

        public string GetUserNameById(int id)
        {
            _users = _serializer.FromCSV(FilePath);

            return _users.Find(u => u.Id == id).Username;
        }
        public void UpdateById(int id,bool b)
        {
            User u=_users.Find(u => u.Id == id);
            if (u.Super == b)
            {
                return;
            }
            else
            {
                _users.Remove(u);
                u.Super = b;
                _users.Add(u);
                _serializer.ToCSV(FilePath, _users);
            }
        }

        public int GetScoreById(int id)
        {
            return _users.Find(u => u.Id == id).Score;
        }

        public void Update(User user)
        {
            int i = _users.FindIndex(u => u.Id == user.Id);

            _users[i] = user;
            _serializer.ToCSV(FilePath, _users);
        }
        public void UpdateSuperGuide(int id,bool super,string language)
        {
            GetById(id).SuperGuide= super;
            GetById(id).SuperGuideLanguage = language;
            _serializer.ToCSV(FilePath, _users);
        }
        public void QuitJob(int id)
        {
            GetById(id).IsQuit = true;
            _serializer.ToCSV(FilePath, _users);
        }
    }
}
