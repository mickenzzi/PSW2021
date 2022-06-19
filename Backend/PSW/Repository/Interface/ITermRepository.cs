using PSW.Model;

namespace PSW.Repository.Interface
{
    public interface ITermRepository : IGenericRepository<Term>
    {
        public Term GetTermById(string id);
    }
}
