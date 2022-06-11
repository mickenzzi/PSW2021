using PSW.DAL;
using PSW.Model;
using PSW.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.Repository
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private readonly PSWStoreContext db;

        public FeedbackRepository(PSWStoreContext _db)
        {
            db = _db;
        }

        public bool Create(Feedback entity)
        {
            db.Feedback.Add(entity);
            db.SaveChanges();
            return true;
        }

        public void Delete(Feedback entity)
        {
            db.Feedback.Remove(entity);
            db.SaveChanges();
        }

        public List<Feedback> GetAll()
        {
            return db.Feedback.ToList();
        }

        public Feedback GetFeedbackById(string id)
        {
            return db.Feedback.Find(id);
        }

        public bool Update(Feedback entity)
        {

            Feedback result = db.Feedback.SingleOrDefault(u => u.Id == entity.Id);
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
