using PSW.Model;

namespace PSW.DTO
{
    public class TermResponse
    {
        public string Id { get; set; }
        public string DateTimeTerm { get; set; }
        public User TermUser { get; set; }
        public User TermDoctor { get; set; }

        public bool IsRejected { get; set; }

        public TermResponse() { }
    }
}
