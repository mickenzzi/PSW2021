using PSW.DTO;
using PSW.Model;
using PSW.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSW.Service
{
    public class TermService
    {
        private readonly ITermRepository _termRepository;
        private readonly IDoctorRepository _doctorRepository;

        public TermService(ITermRepository termRepository, IDoctorRepository doctorRepository)
        {
            _termRepository = termRepository;
            _doctorRepository = doctorRepository;
        }

        public List<Term> GetAllTerms()
        {
            return _termRepository.GetAll();
        }

        public List<Term> ScheduleTerm(TermDTO termDTO)
        {
            bool termExist = CheckUsedTerm(termDTO.StartDate, termDTO.EndDate, termDTO.DoctorId);
            List<Term> availableTerms = new List<Term>();
            if(termExist) {
                if (termDTO.DoctorPriority)
                {
                    availableTerms = GetAllTermsByDoctorPriority(termDTO);
                }
                else
                {
                    availableTerms = GettAllTermsByDatePriority(termDTO);
                }
            } 
            else {
                Term term = new Term(termDTO);
                availableTerms.Add(term);
            }


            return availableTerms;
        }

        private bool CheckUsedTerm(DateTime startDate, DateTime endDate, string doctorId)
        {
            List<Term> terms = _termRepository.GetAll();
            foreach(Term t in terms){
                if (t.DoctorId == doctorId && DateTime.Parse(t.DateTimeTerm) <= endDate && DateTime.Parse(t.DateTimeTerm) >= startDate)
                {
                    return true;
                }
            }
            return false;
        }

        private List<Term> GetAllTermsByDoctorPriority(TermDTO termDTO)
        {
            DateTime start = termDTO.StartDate.AddDays(-10);
            DateTime end = termDTO.StartDate.AddDays(10);

            List<Term> terms = _termRepository.GetAll();
            List<Term> usedTerms = new List<Term>();
            foreach(Term t in terms)
            {
                if(DateTime.Parse(t.DateTimeTerm) >= termDTO.StartDate && DateTime.Parse(t.DateTimeTerm) <= termDTO.EndDate && t.DoctorId == termDTO.DoctorId )
                {
                    usedTerms.Add(t);
                }
            }

            List<Term> availableTermsAfter = FindFreeTermsAfter(termDTO, usedTerms);
            List<Term> availableTermsBefore = FindFreeTermsBefore(termDTO, usedTerms);
            return availableTermsAfter.Concat(availableTermsBefore).ToList();
        
        }

        private List<Term> GettAllTermsByDatePriority(TermDTO termDTO)
        {
            List<Term> terms = _termRepository.GetAll();
            List<Term> availableTerms = new List<Term>();
            List<Doctor> matchingDoctors = FindMatchingDoctors(termDTO);

            foreach(Doctor d in matchingDoctors)
            {
                TermDTO dto = new TermDTO();
                dto.StartDate = termDTO.StartDate;
                dto.DoctorId = d.Id;
                dto.UserId = termDTO.UserId;
                Term term = new Term(dto);
                availableTerms.Add(term);
            }

            return availableTerms;

        }

        private List<Doctor> FindMatchingDoctors(TermDTO termDTO)
        {
            List<Doctor> allDoctors = _doctorRepository.GetAll();
            List<Term> allTerms = _termRepository.GetAll();
            List<Term> usedTerms = new List<Term>();
            Doctor requiredDoctor = _doctorRepository.GetDoctorById(termDTO.DoctorId);
            List<Doctor> matchingDoctors = new List<Doctor>();
            foreach(Term t in allTerms)
            {
                Doctor usedDoctor = _doctorRepository.GetDoctorById(t.DoctorId);
                if(usedDoctor.Specialization.Equals(requiredDoctor.Specialization) && DateTime.Parse(t.DateTimeTerm) >= termDTO.StartDate && DateTime.Parse(t.DateTimeTerm) <= termDTO.EndDate)
                {
                    usedTerms.Add(t);
                }
            }

            foreach(Term t in usedTerms)
            {
                Doctor doctor = _doctorRepository.GetDoctorById(t.DoctorId);
                allDoctors.Remove(doctor);
            }

            foreach(Doctor d in allDoctors)
            {
                if (d.Specialization.Equals(requiredDoctor.Specialization))
                {
                    matchingDoctors.Add(d);
                }
            }
            return matchingDoctors;
        }

        private List<Term> FindFreeTermsAfter(TermDTO termDTO, List<Term> usedTerms)
        {
            List<Term> availableTermsAfter = new List<Term>();
            DateTime end = termDTO.StartDate.AddDays(10);
            for (var day = termDTO.StartDate.Date; day.Date <= end.Date; day = day.AddDays(1))
            {
                for (int i = 8; i < 20; i++)
                {
                    Term term = new Term();
                    DateTime dateTimeTerm = new DateTime(day.Year, day.Month, day.Day, i, 0, 0);
                    term.DateTimeTerm = dateTimeTerm.ToString();
                    term.DoctorId = termDTO.DoctorId;
                    term.UserId = termDTO.UserId;
                    availableTermsAfter.Add(term);
                }
            }

            List<Term> usedAvailableTermsAfter = new List<Term>();
            foreach (Term t in usedTerms)
            {
                foreach (Term t1 in availableTermsAfter)
                {
                    if (DateTime.Parse(t.DateTimeTerm) == DateTime.Parse(t1.DateTimeTerm))
                    {
                        usedAvailableTermsAfter.Add(t1);
                    }
                }
            }
            foreach (Term t in usedAvailableTermsAfter)
            {
                availableTermsAfter.Remove(t);
            }
            return  availableTermsAfter.OrderBy(o => DateTime.Parse(o.DateTimeTerm)).Take(5).ToList();
        }

        private List<Term> FindFreeTermsBefore(TermDTO termDTO, List<Term> usedTerms)
        {
            List<Term> availableTermsBefore = new List<Term>();
            DateTime start = termDTO.StartDate.AddDays(-10);
            for (var day = termDTO.StartDate.Date.AddDays(-1); day.Date >= start.Date; day = day.AddDays(-1))
            {
                for (int i = 8; i < 20; i++)
                {
                    Term term = new Term();
                    DateTime dateTimeTerm = new DateTime(day.Year, day.Month, day.Day, i, 0, 0);
                    term.DateTimeTerm = dateTimeTerm.ToString();
                    term.DoctorId = termDTO.DoctorId;
                    term.UserId = termDTO.UserId;
                    availableTermsBefore.Add(term);
                }
            }

            List<Term> usedAvailableTermsBefore = new List<Term>();
            foreach (Term t in usedTerms)
            {
                foreach (Term t1 in availableTermsBefore)
                {
                    if (DateTime.Parse(t.DateTimeTerm) == DateTime.Parse(t1.DateTimeTerm))
                    {
                        usedAvailableTermsBefore.Add(t1);
                    }
                }
            }
            foreach (Term t in usedAvailableTermsBefore)
            {
                availableTermsBefore.Remove(t);
            }
            return availableTermsBefore.OrderByDescending(o => DateTime.Parse(o.DateTimeTerm)).Take(5).ToList();
        }
    }
}
