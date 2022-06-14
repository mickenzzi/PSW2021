using System.Collections.Generic;
using PSW.Model;
using PSW.Repository.Interface;
using PSW.Service;
using Xunit;
using Shouldly;
using Moq;

namespace PSW.UnitTests
{
    public class FeedbackServiceTests
    {
        private readonly FeedbackService _feedbackService;
        private readonly Mock<IFeedbackRepository> _feedbackRepoMoq = new Mock<IFeedbackRepository>();

        public FeedbackServiceTests()
        {
            _feedbackService = new FeedbackService(_feedbackRepoMoq.Object);
        }

        [Fact]
        public void Get_all_feedbacks()
        {
            var stubRepository = CreateStubRepository();
            FeedbackService service = new FeedbackService(stubRepository.Object);
            var result = service.GetAllFeedbacks();
            result.ShouldNotBe(null);
        }

        [Fact]
        public void Get_feedback_by_id()
        {
            var feedbackDTO = new Feedback
            {
                Id = "1",
                Content = "content",
                Grade = 4,
                UserId = "jovan",
                IsPrivate = true,
                IsVisible = false
            };
            _feedbackRepoMoq.Setup(x => x.GetFeedbackById("1")).Returns(feedbackDTO);
            var result = _feedbackService.GetFeedbackById("1");
            Assert.Equal("1", result.Id);
        }

        [Fact]
        public void Get_feedback_by_invalid_id()
        {
            var stubRepository = CreateStubRepository();
            FeedbackService service = new FeedbackService(stubRepository.Object);
            var result = service.GetFeedbackById("10");
            result.ShouldBe(null);
        }


        private static Mock<IFeedbackRepository> CreateStubRepository()
        {
            var stubRepository = new Mock<IFeedbackRepository>();
            var feedbacks = new List<Feedback>();
            Feedback feedback = new Feedback();
            feedback.Id = "1";
            feedback.Content = "content";
            feedback.Grade = 4;
            feedback.UserId = "jovan";
            feedback.IsPrivate = true;
            feedback.IsVisible = false;
            feedbacks.Add(feedback);

            stubRepository.Setup(x => x.GetAll()).Returns(feedbacks);

            return stubRepository;
        }
    }
}
