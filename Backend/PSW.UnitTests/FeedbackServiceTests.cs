using Moq;
using PSW.Model;
using PSW.Repository.Interface;
using PSW.Service;
using Shouldly;
using System.Collections.Generic;
using Xunit;

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
        public void Get_All_Feedbacks()
        {
            Mock<IFeedbackRepository> feedbackRepository = CreateFeedbackRepository();
            FeedbackService service = new FeedbackService(feedbackRepository.Object);
            List<Feedback> result = service.GetAllFeedbacks();
            result.ShouldNotBe(null);
        }

        [Fact]
        public void Get_Feedback_By_Id()
        {
            Feedback feedbackDTO = new Feedback
            {
                Id = "1",
                Content = "content",
                Grade = 4,
                UserId = "jovan",
                IsPrivate = true,
                IsVisible = false
            };
            _feedbackRepoMoq.Setup(x => x.GetFeedbackById("1")).Returns(feedbackDTO);
            Feedback result = _feedbackService.GetFeedbackById("1");
            Assert.Equal("1", result.Id);
        }

        [Fact]
        public void Get_Feedback_By_Invalid_Id()
        {
            Mock<IFeedbackRepository> feedbackRepository = CreateFeedbackRepository();
            FeedbackService service = new FeedbackService(feedbackRepository.Object);
            Feedback result = service.GetFeedbackById("10");
            result.ShouldBe(null);
        }

        [Fact]
        public void Delete_Feedback()
        {
            Mock<IFeedbackRepository> feedbackRepository = CreateFeedbackRepository();
            FeedbackService service = new FeedbackService(feedbackRepository.Object);
            Feedback feedback = new Feedback
            {
                Id = "1",
                Content = "content",
                Grade = 4,
                UserId = "jovan",
                IsPrivate = true,
                IsVisible = false
            };
            service.DeleteFeedback(feedback);
            Feedback result = service.GetFeedbackById("1");
            result.ShouldBe(null);

        }


        private static Mock<IFeedbackRepository> CreateFeedbackRepository()
        {
            Mock<IFeedbackRepository> feedbackRepository = new Mock<IFeedbackRepository>();
            List<Feedback> feedbacks = new List<Feedback>();
            Feedback feedback = new Feedback
            {
                Id = "1",
                Content = "content",
                Grade = 4,
                UserId = "jovan",
                IsPrivate = true,
                IsVisible = false
            };
            feedbacks.Add(feedback);

            feedbackRepository.Setup(x => x.GetAll()).Returns(feedbacks);

            return feedbackRepository;
        }
    }
}
