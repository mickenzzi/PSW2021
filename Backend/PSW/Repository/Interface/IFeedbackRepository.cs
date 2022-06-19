using PSW.Model;

namespace PSW.Repository.Interface
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        public Feedback GetFeedbackById(string id);
    }
}
