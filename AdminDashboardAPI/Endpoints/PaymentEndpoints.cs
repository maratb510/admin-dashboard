using AdminDashboardAPI.Database;

namespace AdminDashboardAPI.Endpoints
{
    public static class PaymentEndpoints
    {
        public static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/payments", (int take) =>
            {
                var db = new DbAccess();
                var payments = db.GetPayments(take);
                return Results.Ok(payments);
            });

            return app;
        }
    }
}
