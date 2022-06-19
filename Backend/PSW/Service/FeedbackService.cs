using PSW.Model;
using PSW.Repository.Interface;
using System.Collections.Generic;

namespace PSW.Service
{
    public class FeedbackService
    {
        private readonly IFeedbackRepository _feedbackRepository;

        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }


        public List<Feedback> GetAllFeedbacks()
        {
            return _feedbackRepository.GetAll();
        }

        public void DeleteFeedback(Feedback feedback)
        {
            _feedbackRepository.Delete(feedback);
        }

        public bool CreateFeedback(Feedback feedback)
        {
            return _feedbackRepository.Create(feedback);
        }

        public bool UpdateFeedback(Feedback newFeedback)
        {
            Feedback feedback = _feedbackRepository.GetFeedbackById(newFeedback.Id);
            if (feedback == null)
            {
                return false;
            }
            feedback.Content = newFeedback.Content;
            feedback.Grade = newFeedback.Grade;
            feedback.IsPrivate = newFeedback.IsPrivate;
            feedback.IsVisible = newFeedback.IsVisible;
            return _feedbackRepository.Update(feedback);
        }

        public Feedback GetFeedbackById(string id)
        {
            return _feedbackRepository.GetFeedbackById(id);
        }
    }
}
