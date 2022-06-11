using PSW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.Repository.Interface
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        public Feedback GetFeedbackById(string id);
    }
}
