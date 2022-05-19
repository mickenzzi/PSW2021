using PSW.Model;

namespace PSW.Repository.Interface
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public User GetUserByUsername(string username);
        public User GetUserById(string id);
    }
}
