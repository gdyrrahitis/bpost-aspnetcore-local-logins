namespace Authentication.Local.Models
{
    using System.Threading.Tasks;

    public class UserClaims
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public string Issuer { get; set; }
    }
}