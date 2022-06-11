using PSW.DAL;
using PSW.Model;
using PSW.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly PSWStoreContext db;

        public CommentRepository(PSWStoreContext _db)
        {
            db = _db;
        }

        public bool Create(Comment entity)
        {
            if (db.Comment.Any(c => c.TermId == entity.TermId && c.UserId == entity.UserId))
            {
                return false;
            }

            db.Comment.Add(entity);
            db.SaveChanges();
            return true;
        }

        public void Delete(Comment entity)
        {
            db.Comment.Remove(entity);
            db.SaveChanges();
        }

        public List<Comment> GetAll()
        {
            return db.Comment.ToList();
        }

        public Comment GetCommentById(string id)
        {
            return db.Comment.Find(id);
        }

        public bool Update(Comment entity)
        {

            Comment result = db.Comment.SingleOrDefault(u => u.Id == entity.Id);
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
