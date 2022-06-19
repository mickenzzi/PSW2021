namespace PSW.DTO
{
    public class SuspiciousUser
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }

        public int RejectsNumber { get; set; }

        public bool IsBlocked { get; set; }

        public SuspiciousUser() { }
    }
}
