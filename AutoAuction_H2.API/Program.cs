namespace AutoAuction_H2.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 🔹 Ingen services lige nu
            // builder.Services.AddControllers();

            var app = builder.Build();

            // 🔹 Kun basis middleware
            app.MapGet("/", () => "API is running (stub)");

            app.Run();
        }
    }
}
