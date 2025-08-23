using Razor.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Authorization;
using Razor.Components.Services;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
 //.AddCircuitOptions(options => options.DetailedErrors = true);
builder.Services.AddScoped<FileService>();
builder.Services.AddScoped<FileAccessService>();
builder.Services.AddScoped<RequestFileService>();
builder.Services.AddScoped<UserManagementService>();
builder.Services.AddScoped<RoleAttributeService>();
builder.Services.AddScoped<DataSourceService>();
builder.Services.AddSingleton<FileCleanupService>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<FileCleanupService>());
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddBlazorBootstrap();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 50L * 1024 * 1024 * 1024; 
});


var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await RoleInitializerService.InitializeRoles(services);
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

var protectedPath = Path.Combine(app.Environment.ContentRootPath, "req");


app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/req"))
    {
        var user = context.User;
        if (user?.Identity?.IsAuthenticated != true)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }
    }
    await next();
});


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "req")),
    RequestPath = "/req"
});


//builder.Logging.ClearProviders();
builder.Logging.AddConsole();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
