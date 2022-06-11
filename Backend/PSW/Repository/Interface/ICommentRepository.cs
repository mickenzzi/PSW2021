using PSW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.Repository.Interface
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        public Comment GetCommentById(string id);
    }
}
