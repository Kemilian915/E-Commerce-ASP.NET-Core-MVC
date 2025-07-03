using Microsoft.AspNetCore.Identity;

namespace test_e4.Models
{
    public class Users : IdentityUser
    {
        public string FullName { get; set; }
    }
}
