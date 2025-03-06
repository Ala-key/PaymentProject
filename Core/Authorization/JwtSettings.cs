namespace Core.Authorization
{
    /// <summary>
    /// Параметры Jwt.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Секрет.
        /// </summary>
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpiration { get; set; }
    }
}
