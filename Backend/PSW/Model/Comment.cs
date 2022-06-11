using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PSW.Model
{
    [Table("Comment")]
    public class Comment
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("grade")]
        public int Grade { get; set; }

        [Column("term")]
        public string TermId { get; set; }

        [Column("user")]
        public string UserId { get; set; }

        public Comment()
        {
            Id = "comment_" + Guid.NewGuid();
        }


        public Comment(Comment newComment)
        {
            Id = "comment_" + Guid.NewGuid();
            Content = newComment.Content;
            Grade = newComment.Grade;
            UserId = newComment.UserId;
            TermId = newComment.TermId;
        }

    }
}
