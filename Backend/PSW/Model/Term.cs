using PSW.DTO;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PSW.Model
{

    [Table("Term")]
    public class Term
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }
        [Column("user")]
        public string UserId { get; set; }
        [Column("doctor")]
        public string DoctorId { get; set; }
        [Column("dateTime")]
        public string DateTimeTerm { get; set; }
        [Column("rejected")]
        public bool IsRejected { get; set; }

        public Term()
        {
            Id = "term_" + Guid.NewGuid();
        }

        public Term(TermDTO termDTO) 
        {
            Id = "term_" + Guid.NewGuid();
            UserId = termDTO.UserId;
            DoctorId = termDTO.DoctorId;
            DateTimeTerm = termDTO.StartDate.ToString();
            IsRejected = false;
        }


    }
}
