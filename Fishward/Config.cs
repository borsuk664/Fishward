using System.Text;

namespace Fishward;

public class Config
{
    public DatabaseConfig Database { get; set; }
    public DiscordConfig Discord { get; set; }

    
    public class DatabaseConfig
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        
        public string GetConnectionString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Server={Address};");
            stringBuilder.Append($"Port={Port};");
            stringBuilder.Append($"User ID={User};");
            stringBuilder.Append($"Password={Password};");
            stringBuilder.Append($"Database={DatabaseName};");
            stringBuilder.Append("Pooling=True;");
            return stringBuilder.ToString();
        }
    }

    public struct DiscordConfig
    {
        public string Token { get; set; }
    }
}