using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.Model
{
    [Table("Feedback")]
    public class Feedback
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("grade")]
        public int Grade { get; set; }

        [Column("user")]
        public string UserId { get; set; }

        [Column("private")]
        public bool IsPrivate { get; set; }

        [Column("visible")]
        public bool IsVisible { get; set; }

        public Feedback()
        {
            Id = "feedback_" + Guid.NewGuid();
        }


        public Feedback(Feedback newFeedback)
        {
            Id = "feedack_" + Guid.NewGuid();
            Content = newFeedback.Content;
            Grade = newFeedback.Grade;
            UserId = newFeedback.UserId;
            IsPrivate = newFeedback.IsPrivate;
            IsVisible = false;
        }
    }
}
