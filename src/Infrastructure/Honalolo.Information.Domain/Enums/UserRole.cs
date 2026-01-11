namespace Honalolo.Information.Domain.Enums
{
    public enum UserRole
    {
        Guest = 0,                 // Gość (users without accounts) 
        RegisteredUser = 1,        // Użytkownik zarejestrowany
        Partner = 2,               // Organizator / Partner lokalny
        Moderator = 3,             // Moderator / Redaktor treści
        Administrator = 99         // Administrator systemu
    }
}
