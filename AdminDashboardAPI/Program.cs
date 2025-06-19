using AdminDashboardAPI.Database;
using AdminDashboardAPI.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173") // ����� ������ ������
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// �������������� ���� ������ (������� + ����)
DbInitializer.Initialize("dashboard.db");

app.UseCors("AllowFrontend");

// ���������� ���������
app.MapAuthEndpoints();
app.MapClientEndpoints();
app.MapPaymentEndpoints();
app.MapRateEndpoints();

app.Use(async (context, next) =>
{
    // ������� �������� ��������� Authorization: Bearer demo
    var path = context.Request.Path.ToString();
    if (path.StartsWith("/api/auth")) await next(context);
    else if (context.Request.Headers.Authorization == "Bearer demo") await next(context);
    else context.Response.StatusCode = 401;
});

app.Run();
