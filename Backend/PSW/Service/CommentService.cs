using PSW.Model;
using PSW.Repository.Interface;
using System.Collections.Generic;

namespace PSW.Service
{
    public class CommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }


        public List<Comment> GetAllComments()
        {
            return _commentRepository.GetAll();
        }

        public void DeleteComment(Comment comment)
        {
            _commentRepository.Delete(comment);
        }

        public bool CreateComment(Comment comment)
        {
            return _commentRepository.Create(comment);
        }

        public bool UpdateComment(Comment newComment)
        {
            Comment comment = _commentRepository.GetCommentById(newComment.Id);
            if (comment == null)
            {
                return false;
            }
            comment.Content = newComment.Content;
            comment.Grade = newComment.Grade;
            return _commentRepository.Update(comment);
        }

        public Comment GetCommentById(string id)
        {
            return _commentRepository.GetCommentById(id);
        }
    }
}
