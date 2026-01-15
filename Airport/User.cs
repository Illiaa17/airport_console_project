namespace lab_KN_23;

/// <summary>
/// Представляє користувача системи.
/// </summary>
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }