using AdminDashboardAPI.Database;
using Microsoft.AspNetCore.Mvc;

namespace AdminDashboardAPI.Endpoints
{
    public static class RateEndpoints
    {
        public static IEndpointRouteBuilder MapRateEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/rate", () =>
            {
                var db = new DbAccess();
                var rate = db.GetRate();
                return rate is not null ? Results.Ok(rate) : Results.NotFound();
            });

            app.MapPost("/api/rate", ([FromBody] UpdateRateRequest req) =>
            {
                var db = new DbAccess();
                db.UpdateRate(req.Value);
                return Results.Ok(new { success = true });
            });

            return app;
        }

        public record UpdateRateRequest(decimal Value);
    }
}
