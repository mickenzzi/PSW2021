using PSW.DTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSW.Model
{
    [Table("Doctor")]
    public class Doctor
    {
        public enum DoctorSpecialization
        {
            Cardiology,
            Dermatology,
            Pediatrics,
            Oncology,
            Orthopedic,
            Psychiatry,
            GeneralPractitioner
        }

        [Key]
        [Column("id")]
        public string Id { get; set; }
        
        [Column("firstName")]
        public string FirstName { get; set; }

        [Column("lastName")]
        public string LastName { get; set; }

        [Column("specialization")]
        public string Specialization { get; set; }

        public Doctor()
        {
            Id = "doctor_" + Guid.NewGuid();
        }

        public Doctor(DoctorDTO doctorDTO)
        {
            Id = "doctor_" + Guid.NewGuid();
            FirstName = doctorDTO.FirstName;
            LastName = doctorDTO.LastName;
            Specialization = mappingSpecialization(doctorDTO.Specialization);
        }

        public string mappingSpecialization(string specialization)
        {
            if (specialization.ToLower().Equals(DoctorSpecialization.Cardiology.ToString().ToLower()))
            {
                return DoctorSpecialization.Cardiology.ToString();
            }
            else if (specialization.ToLower().Equals(DoctorSpecialization.Dermatology.ToString().ToLower()))
            {
                return DoctorSpecialization.Dermatology.ToString();
            }
            else if (specialization.ToLower().Equals(DoctorSpecialization.GeneralPractitioner.ToString().ToLower()))
            {
                return DoctorSpecialization.GeneralPractitioner.ToString();
            }
            else if (specialization.ToLower().Equals(DoctorSpecialization.Oncology.ToString().ToLower()))
            {
                return DoctorSpecialization.Oncology.ToString();
            }
            else if (specialization.ToLower().Equals(DoctorSpecialization.Orthopedic.ToString().ToLower()))
            {
                return DoctorSpecialization.Orthopedic.ToString();
            }
            else if (specialization.ToLower().Equals(DoctorSpecialization.Pediatrics.ToString().ToLower()))
            {
                return DoctorSpecialization.Pediatrics.ToString();
            }
            else
            {
                return DoctorSpecialization.Psychiatry.ToString();
            }
        }


    }
}
