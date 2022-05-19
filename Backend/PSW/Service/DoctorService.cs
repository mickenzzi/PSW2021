using PSW.DTO;
using PSW.Model;
using PSW.Repository.Interface;
using System.Collections.Generic;

namespace PSW.Service
{
    public class DoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        
        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetAll();
        }

        public Doctor GetDoctorById(string id)
        {
            return _doctorRepository.GetDoctorById(id);
        }

        public void DeleteDoctor(Doctor doctor)
        {
            _doctorRepository.Delete(doctor);
        }

        public bool CreateDoctor(Doctor doctor)
        {
            return _doctorRepository.Create(doctor);
        }

        public bool UpdateDoctor(DoctorDTO doctorDTO)
        {
            Doctor doctor = _doctorRepository.GetDoctorById(doctorDTO.Id);
            if(doctor == null)
            {
                return false;
            }
            doctor.FirstName = doctorDTO.FirstName;
            doctor.LastName = doctorDTO.LastName;
            doctor.Specialization = doctor.mappingSpecialization(doctorDTO.Specialization);
            return _doctorRepository.Update(doctor);
        }
    }
}
