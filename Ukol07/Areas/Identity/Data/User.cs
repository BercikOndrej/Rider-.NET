using Microsoft.AspNetCore.Identity;

namespace Ukol07.Areas.Identity.Data;

// Add profile data for application users by adding properties to the User class
public class User : IdentityUser
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string BlogNickname { get; set; }
}

