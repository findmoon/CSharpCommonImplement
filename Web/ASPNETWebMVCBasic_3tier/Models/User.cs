using Newtonsoft.Json;

namespace WebAPI_CURD.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; } 
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }

        [JsonIgnore]
        public string PasswordHash { get; set; }
    }
}
