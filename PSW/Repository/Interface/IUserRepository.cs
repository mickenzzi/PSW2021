using PSW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public User GetUserByUsername(string username);
        public User GetUserById(string id);
    }
}
