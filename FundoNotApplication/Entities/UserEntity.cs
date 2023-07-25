namespace FundoNotApplication.Entities
{
    public class UserEntity
    {
        public string EmailId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
     //   public string Email_TBD { get; set; }
        public string Password { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
