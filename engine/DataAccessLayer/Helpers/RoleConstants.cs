namespace BusinessLayer.Helpers;

public static class RoleConstants
{
    public const string USER_ROLE = "User";
    public const string ADMIN_ROLE = "Admin";
    public static List<string> Roles { get; set; } = new List<string>()
    {
        USER_ROLE,
        ADMIN_ROLE
    };    
}