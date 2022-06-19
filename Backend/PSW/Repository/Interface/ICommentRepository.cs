using PSW.Model;

namespace PSW.Repository.Interface
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        public Comment GetCommentById(string id);
    }
}
