using PSW.DAL;
using PSW.Model;
using PSW.Repository.Interface;
using System.Collections.Generic;
using System.Linq;

namespace PSW.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly PSWStoreContext db;

        public DoctorRepository(PSWStoreContext _db)
        {
            db = _db;
        }

        public bool Create(Doctor entity)
        {
            db.Doctor.Add(entity);
            db.SaveChanges();
            return true;
        }

        public void Delete(Doctor entity)
        {
            db.Doctor.Remove(entity);
            db.SaveChanges();
        }

        public List<Doctor> GetAll()
        {
            return db.Doctor.ToList();
        }

        public Doctor GetDoctorById(string id)
        {
            return db.Doctor.Find(id);
        }

        public bool Update(Doctor entity)
        {
            Doctor result = db.Doctor.SingleOrDefault(u => u.Id == entity.Id);
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
