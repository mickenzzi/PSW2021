using System;

namespace PSW.DTO
{
    public class TermDTO
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string UserId { get; set; }
        public string DoctorId { get; set; }
        public bool DoctorPriority { get; set; }

        public TermDTO() { }

    }
}
