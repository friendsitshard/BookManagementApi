namespace BookManagmentApi.Helpers
{
    public class JwtSettings
    {
        public required string Key { get; init; }
        public required string Issuer { get; init; }
    }
}
