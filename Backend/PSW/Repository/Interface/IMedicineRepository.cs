using PSW.Model;

namespace PSW.Repository.Interface
{
    public interface IMedicineRepository : IGenericRepository<Medicine>
    {
        public Medicine GetMedicineById(string id);
    }
}
