using IronPdf;
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
        private readonly IUserRepository _userRepository;
        private readonly DoctorService _doctorService;

        public TermService(ITermRepository termRepository, IUserRepository userRepository, DoctorService doctorService)
        {
            _termRepository = termRepository;
            _userRepository = userRepository;
            _doctorService = doctorService;

        }

        public List<Term> GetAllTerms()
        {
            return _termRepository.GetAll();
        }

        public Term GetTermById(string id)
        {
            return _termRepository.GetTermById(id);
        }

        public bool ReserveTerm(Term term)
        {
            User doctor = _userRepository.GetUserById(term.DoctorId);
            if (!doctor.Specialization.Equals("GeneralPractitioner"))
            {
                CreateReferral(term);
            }
            return _termRepository.Create(term);
        }

        private void CreateReferral(Term term)
        {
            User doctor = _userRepository.GetUserById(term.DoctorId);
            User user = _userRepository.GetUserById(term.UserId);
            string userFullName = user.FirstName + " " + user.LastName;
            string doctorFullName = doctor.FirstName + " " + doctor.LastName;
            string termDate = term.DateTimeTerm;
            DateTime now = DateTime.Now;
            string html = "<br><h1 style = \"text-align: center;\">Uput doktoru specijalisti</h1><br>" +
                "<label style = \"font-size: medium;\">Doktor: " + doctorFullName + "</label>" +
                "<label style = \"margin-left: 40%; font-size: medium; position:fixed\">Datum: " + now.ToString() + "</label><br><br><br>" +
                "<label style = \"font-size: large; margin-left: 45%;\">Razlog: </label><br><br><label>Kreiranje zahteva za uput doktoru specijalisti na zahtev pacijenta za datum " + termDate + "." + "</label><br><br><br><br>" +
                "<label style = \"font-size: medium;\">Pacijent: " + userFullName + "</label>";
            ChromePdfRenderer render = new ChromePdfRenderer();
            PdfDocument pdf = render.RenderHtmlAsPdf(html);
            pdf.SaveAs(@"C:\Users\HP\Desktop\PSW\PSW2021\Backend\referrals\" + term.Id + ".pdf");
        }

        public bool RejectTerm(Term term)
        {
            DateTime now = DateTime.Now;
            DateTime start = DateTime.Parse(term.DateTimeTerm);
            if ((start - now).TotalDays < 2)
            {
                return false;
            }
            term.IsRejected = true;
            _termRepository.Update(term);
            return true;
        }

        public void DeleteTerm(Term term)
        {
            _termRepository.Delete(term);
        }

        public List<TermResponse> GetAllDoctorTerms(string id)
        {
            List<Term> allTerms = _termRepository.GetAll();
            List<TermResponse> doctorTerms = new List<TermResponse>();
            foreach (Term t in allTerms)
            {
                if (t.DoctorId.Equals(id))
                {
                    TermResponse termResponse = new TermResponse
                    {
                        DateTimeTerm = t.DateTimeTerm,
                        Id = t.Id,
                        TermDoctor = _userRepository.GetUserById(t.DoctorId),
                        TermUser = _userRepository.GetUserById(t.UserId),
                        IsRejected = t.IsRejected
                    };
                    doctorTerms.Add(termResponse);
                }
            }
            return doctorTerms;
        }

        public List<TermResponse> GetAllPatientTerms(string id)
        {
            List<Term> allTerms = _termRepository.GetAll();
            List<TermResponse> patientTerms = new List<TermResponse>();
            foreach (Term t in allTerms)
            {
                if (t.UserId.Equals(id))
                {
                    TermResponse termResponse = new TermResponse
                    {
                        DateTimeTerm = t.DateTimeTerm,
                        Id = t.Id,
                        TermDoctor = _userRepository.GetUserById(t.DoctorId),
                        TermUser = _userRepository.GetUserById(t.UserId),
                        IsRejected = t.IsRejected
                    };
                    patientTerms.Add(termResponse);
                }
            }
            return patientTerms;
        }

        public List<TermResponse> GetPatientFutureTerms(string id)
        {
            List<TermResponse> patientTerms = GetAllPatientTerms(id);
            List<TermResponse> featureTerms = new List<TermResponse>();
            DateTime now = DateTime.Now;
            foreach (TermResponse t in patientTerms)
            {
                if (DateTime.Parse(t.DateTimeTerm) >= now)
                {
                    featureTerms.Add(t);
                }
            }
            return featureTerms;
        }

        public List<TermResponse> GetPatientFinishedTerms(string id)
        {
            List<TermResponse> patientTerms = GetAllPatientTerms(id);
            List<TermResponse> featureTerms = new List<TermResponse>();
            DateTime now = DateTime.Now;
            foreach (TermResponse t in patientTerms)
            {
                if (DateTime.Parse(t.DateTimeTerm) < now)
                {
                    featureTerms.Add(t);
                }
            }
            return featureTerms;
        }


        public List<TermResponse> ScheduleTerm(TermDTO termDTO)
        {
            bool termExist = CheckUsedTerm(termDTO.StartDate, termDTO.EndDate, termDTO.DoctorId);
            List<Term> availableTerms = new List<Term>();
            if (termExist)
            {
                if (termDTO.DoctorPriority)
                {
                    availableTerms = GetAllTermsByDoctorPriority(termDTO);
                }
                else
                {
                    availableTerms = GettAllTermsByDatePriority(termDTO);
                }
            }
            else
            {
                Term term = new Term(termDTO);
                availableTerms.Add(term);
            }

            List<TermResponse> terms = new List<TermResponse>();
            foreach (Term t in availableTerms)
            {
                TermResponse termResponse = new TermResponse
                {
                    DateTimeTerm = t.DateTimeTerm
                };
                User user = _userRepository.GetUserById(t.UserId);
                User doctor = _userRepository.GetUserById(t.DoctorId);
                termResponse.Id = t.Id;
                termResponse.TermUser = user;
                termResponse.TermDoctor = doctor;
                termResponse.IsRejected = t.IsRejected;
                terms.Add(termResponse);

            }


            return terms;
        }

        private bool CheckUsedTerm(DateTime startDate, DateTime endDate, string doctorId)
        {
            List<Term> terms = _termRepository.GetAll();
            foreach (Term t in terms)
            {
                if (t.DoctorId == doctorId && DateTime.Parse(t.DateTimeTerm) <= endDate && DateTime.Parse(t.DateTimeTerm) >= startDate && !t.IsRejected)
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
            foreach (Term t in terms)
            {
                if (DateTime.Parse(t.DateTimeTerm) >= termDTO.StartDate && DateTime.Parse(t.DateTimeTerm) <= termDTO.EndDate && t.DoctorId == termDTO.DoctorId && !t.IsRejected)
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
            List<User> matchingDoctors = FindMatchingDoctors(termDTO);

            foreach (User d in matchingDoctors)
            {
                TermDTO dto = new TermDTO
                {
                    StartDate = termDTO.StartDate,
                    DoctorId = d.Id,
                    UserId = termDTO.UserId
                };
                Term term = new Term(dto);
                availableTerms.Add(term);
            }

            return availableTerms;

        }

        private List<User> FindMatchingDoctors(TermDTO termDTO)
        {
            List<User> allDoctors = _doctorService.GetAllDoctors();
            List<Term> allTerms = _termRepository.GetAll();
            List<Term> usedTerms = new List<Term>();
            User requiredDoctor = _userRepository.GetUserById(termDTO.DoctorId);
            List<User> matchingDoctors = new List<User>();
            foreach (Term t in allTerms)
            {
                User usedDoctor = _userRepository.GetUserById(t.DoctorId);
                if (usedDoctor.Specialization.Equals(requiredDoctor.Specialization) && DateTime.Parse(t.DateTimeTerm) >= termDTO.StartDate && DateTime.Parse(t.DateTimeTerm) <= termDTO.EndDate && !t.IsRejected)
                {
                    usedTerms.Add(t);
                }
            }

            foreach (Term t in usedTerms)
            {
                User doctor = _userRepository.GetUserById(t.DoctorId);
                allDoctors.Remove(doctor);
            }

            foreach (User d in allDoctors)
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
            for (DateTime day = termDTO.StartDate.Date; day.Date <= end.Date; day = day.AddDays(1))
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
            return availableTermsAfter.OrderBy(o => DateTime.Parse(o.DateTimeTerm)).Take(5).ToList();
        }

        private List<Term> FindFreeTermsBefore(TermDTO termDTO, List<Term> usedTerms)
        {
            List<Term> availableTermsBefore = new List<Term>();
            DateTime start = termDTO.StartDate.AddDays(-10);
            for (DateTime day = termDTO.StartDate.Date.AddDays(-1); day.Date >= start.Date; day = day.AddDays(-1))
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
