namespace Core.Authorization
{
    /// <summary>
    /// Результат авторизации.
    /// </summary>
    public class JwtAuthResult
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Роль.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Токен.
        /// </summary>
        public string AccessToken { get; set; }
    }
}
