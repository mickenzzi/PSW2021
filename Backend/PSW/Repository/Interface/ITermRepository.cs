using PSW.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.Repository.Interface
{
    public interface ITermRepository : IGenericRepository<Term>
    {
        public Term GetTermById(String id);
    }
}
