using System.Collections.Generic;
using PSW.Model;
using PSW.Repository.Interface;
using PSW.Service;
using Xunit;
using Shouldly;
using Moq;

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
        public void Check_if_you_already_comment()
        {
            var stubRepository = CreateStubRepository();
            CommentService service = new CommentService(stubRepository.Object);
            Comment comment = new Comment();
            comment.Id = "1";
            comment.Content = "content";
            comment.Grade = 2;
            comment.UserId = "jovan";
            comment.TermId = "today";
            var result = service.CreateComment(comment);
            result.ShouldBe(false);
        }

        [Fact]
        public void Get_all_comments()
        {
            var stubRepository = CreateStubRepository();
            CommentService service = new CommentService(stubRepository.Object);
            var result = service.GetAllComments();
            result.ShouldNotBe(null);
        }

        [Fact]
        public void Delete_comment()
        {
            var stubRepository = CreateStubRepository();
            CommentService service = new CommentService(stubRepository.Object);
            Comment comment = new Comment();
            comment.Id = "1";
            comment.Content = "content";
            comment.Grade = 2;
            comment.UserId = "jovan";
            comment.TermId = "today";
            service.DeleteComment(comment);
            var result = service.GetCommentById("1");
            result.ShouldBe(null);
        }


        private static Mock<ICommentRepository> CreateStubRepository()
        {
            var stubRepository = new Mock<ICommentRepository>();
            var comments = new List<Comment>();
            Comment comment = CreateComment();
            comments.Add(comment);

            stubRepository.Setup(x => x.GetAll()).Returns(comments);

            return stubRepository;
        }

        private static Comment CreateComment()
        {
            Comment comment = new Comment();
            comment.Id = "1";
            comment.Content = "content";
            comment.Grade = 2;
            comment.UserId = "jovan";
            comment.TermId = "today";
            return comment;
        }
    }
}
