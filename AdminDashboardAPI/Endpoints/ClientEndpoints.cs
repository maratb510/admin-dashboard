using AdminDashboardAPI.Database;

namespace AdminDashboardAPI.Endpoints
{
    public static class ClientEndpoints
    {
        public static IEndpointRouteBuilder MapClientEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/clients", () =>
            {
                var db = new DbAccess();
                var clients = db.GetClients();
                return Results.Ok(clients);
            });

            return app;
        }
    }
}
