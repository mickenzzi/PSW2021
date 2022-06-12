using Pharmacy.DAL;
using Pharmacy.Model;
using Pharmacy.Repository.Inteface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pharmacy.Repository
{
    public class MedicineRepository : IMedicineRepository
    {
        private readonly PSWStoreContext db;

        public MedicineRepository(PSWStoreContext _db)
        {
            db = _db;
        }

        public bool Create(Medicine entity)
        {
            db.Medicine.Add(entity);
            db.SaveChanges();
            return true;
        }

        public void Delete(Medicine entity)
        {
            db.Medicine.Remove(entity);
            db.SaveChanges();
        }

        public List<Medicine> GetAll()
        {
            return db.Medicine.ToList();
        }

        public Medicine GetMedicineById(string id)
        {
            return db.Medicine.Find(id);
        }

        public bool Update(Medicine entity)
        {

            Medicine result = db.Medicine.SingleOrDefault(u => u.Id == entity.Id);
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
