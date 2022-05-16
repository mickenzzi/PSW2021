using PSW.DAL;
using PSW.Model;
using PSW.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSW.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PSWStoreContext _dataContext;

        public UserRepository(PSWStoreContext dataContext)
        {
            _dataContext = dataContext;
        }
        public void Delete(User entity)
        {
            _dataContext.User.Remove(entity);
            _dataContext.SaveChanges();
        }

        public List<User> GetAll()
        {
            return _dataContext.User.ToList();
        }

        public User GetUserById(string id)
        {
            return _dataContext.User.Find(id);
        }

        public User GetUserByUsername(string username)
        {
            return _dataContext.User.SingleOrDefault(u => u.Username == username);
        }

        public bool Create(User entity)
        {

            if (_dataContext.User.Any(u => u.Username == entity.Username))
            {
                return false;
            }

            _dataContext.User.Add(entity);
            _dataContext.SaveChanges();
            return true;
        }

        public bool Update(User entity)
        {
            User result = _dataContext.User.SingleOrDefault(u => u.Id == entity.Id);
            if (result != null)
            {
                result = entity;
                _dataContext.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
