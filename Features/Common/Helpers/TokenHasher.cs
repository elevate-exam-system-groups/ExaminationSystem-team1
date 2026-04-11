namespace ExaminationSystem.Features.Common.Helpers
{
    public static class TokenHasher
    {
        public static string HashToken(string token)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(token);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }
    }
}
