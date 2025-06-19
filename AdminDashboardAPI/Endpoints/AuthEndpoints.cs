using Microsoft.AspNetCore.Mvc;

namespace AdminDashboardAPI.Endpoints
{
    public static class AuthEndpoints
    {
        public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/auth/login", ([FromBody] LoginRequest req) =>
            {
                if (req.Email == "admin@mirra.dev" && req.Password == "admin123")
                    return Results.Ok(new { token = "demo" });

                return Results.Unauthorized();
            });

            return app;
        }

        public record LoginRequest(string Email, string Password);
    }
}
