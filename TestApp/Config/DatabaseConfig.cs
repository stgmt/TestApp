namespace TestApp.Config
{
    class DatabaseConfig
    {
        public static string ConnectionString => "Host=host.docker.internal:5432;Database=bookcatalog;Username=user;Password=password";
    }
}
