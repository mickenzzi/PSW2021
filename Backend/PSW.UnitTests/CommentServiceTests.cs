using Moq;
using PSW.Model;
using PSW.Repository.Interface;
using PSW.Service;
using Shouldly;
using System.Collections.Generic;
using Xunit;

namespace PSW.UnitTests
{
    public class CommentServiceTests
    {
        private readonly CommentService _commentService;
        private readonly Mock<ICommentRepository> _commentRepoMoq = new Mock<ICommentRepository>();

        public CommentServiceTests()
        {
            _commentService = new CommentService(_commentRepoMoq.Object);
        }

        [Fact]
        public void Check_If_You_Already_Comment()
        {
            Mock<ICommentRepository> commentRepository = CreateCommentRepository();
            CommentService service = new CommentService(commentRepository.Object);
            Comment comment = new Comment
            {
                Id = "1",
                Content = "content",
                Grade = 2,
                UserId = "jovan",
                TermId = "today"
            };
            bool result = service.CreateComment(comment);
            result.ShouldBe(false);
        }

        [Fact]
        public void Get_All_Comments()
        {
            Mock<ICommentRepository> commentRepository = CreateCommentRepository();
            CommentService service = new CommentService(commentRepository.Object);
            List<Comment> result = service.GetAllComments();
            result.ShouldNotBe(null);
        }

        [Fact]
        public void Delete_Comment()
        {
            Mock<ICommentRepository> commentRepository = CreateCommentRepository();
            CommentService service = new CommentService(commentRepository.Object);
            Comment comment = new Comment
            {
                Id = "1",
                Content = "content",
                Grade = 2,
                UserId = "jovan",
                TermId = "today"
            };
            service.DeleteComment(comment);
            Comment result = service.GetCommentById("1");
            result.ShouldBe(null);
        }


        private static Mock<ICommentRepository> CreateCommentRepository()
        {
            Mock<ICommentRepository> commentRepository = new Mock<ICommentRepository>();
            List<Comment> comments = new List<Comment>();
            Comment comment = CreateComment();
            comments.Add(comment);

            commentRepository.Setup(x => x.GetAll()).Returns(comments);

            return commentRepository;
        }

        private static Comment CreateComment()
        {
            Comment comment = new Comment
            {
                Id = "1",
                Content = "content",
                Grade = 2,
                UserId = "jovan",
                TermId = "today"
            };
            return comment;
        }
    }
}
