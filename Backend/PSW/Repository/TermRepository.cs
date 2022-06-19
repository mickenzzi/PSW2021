using PSW.DAL;
using PSW.Model;
using PSW.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace PSW.Repository
{
    public class TermRepository : ITermRepository
    {

        private readonly PSWStoreContext db;

        public TermRepository(PSWStoreContext _db)
        {
            db = _db;
        }

        public bool Create(Term entity)
        {
            db.Term.Add(entity);
            db.SaveChanges();
            return true;
        }

        public void Delete(Term entity)
        {
            db.Term.Remove(entity);
            db.SaveChanges();
        }

        public List<Term> GetAll()
        {
            return db.Term.ToList();
        }

        public Term GetTermById(string id)
        {
            return db.Term.Find(id);
        }

        public bool Update(Term entity)
        {
            Term result = db.Term.SingleOrDefault(t => t.Id == entity.Id);
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
