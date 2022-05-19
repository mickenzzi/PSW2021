using PSW.Model;

namespace PSW.Repository.Interface
{
    public interface IDoctorRepository : IGenericRepository<Doctor>
    {
        public Doctor GetDoctorById(string id);
    }
}
