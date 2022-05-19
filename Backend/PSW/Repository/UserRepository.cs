using PSW.DAL;
using PSW.Model;
using PSW.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace PSW.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PSWStoreContext db;

        public UserRepository(PSWStoreContext _db)
        {
            db = _db;
        }
        public void Delete(User entity)
        {
            db.User.Remove(entity);
            db.SaveChanges();
        }

        public List<User> GetAll()
        {
            return db.User.ToList();
        }

        public User GetUserById(string id)
        {
            return db.User.Find(id);
        }

        public User GetUserByUsername(string username)
        {
            return db.User.SingleOrDefault(u => u.Username == username);
        }

        public bool Create(User entity)
        {

            if (db.User.Any(u => u.Username == entity.Username))
            {
                return false;
            }

            db.User.Add(entity);
            db.SaveChanges();
            return true;
        }

        public bool Update(User entity)
        {
            User result = db.User.SingleOrDefault(u => u.Id == entity.Id);
            if (result != null)
            {
                result = entity;
                db.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
