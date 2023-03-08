using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Tutor.api.Services;
using Tutor.Api.Identity;
using Tutor.Api.Models;

var builder = WebApplication.CreateBuilder(args);

//Change CORS policy

string allowance = "AllowAll";

var allowAll = builder.Services.AddCors(options =>
{
    options.AddPolicy(allowance, builder =>
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(config =>
{
    config.SwaggerDoc("v1", new OpenApiInfo { Title = "Tutor API", Version = "v1" });
    config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer"
    });
    config.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});
var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<Tutor.Api.Services.EmailService>();

Console.WriteLine("Begin SQL Connection");
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<tutor_dbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<DatabaseService>();
Console.WriteLine("End SQL Connection.");

Console.WriteLine("Begin Identity Generation");
//Identity stuff
builder.Services.AddIdentityCore<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<tutor_dbContext>();
Console.WriteLine("End Identity Generation");

Console.WriteLine("Begin JWT configuration");
//JWT Token setup
JwtConfiguration jwtConfiguration = builder.Configuration.GetSection("Jwt").Get<JwtConfiguration>();
builder.Services.AddSingleton(jwtConfiguration);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfiguration.Issuer,
            ValidAudience = jwtConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfiguration.Secret))
        };
    });
Console.WriteLine("End JWT configuration");

//Add Policies
//builder.Services.AddAuthorization(options =>
//{
//  options.AddPolicy(Policies.RequireAdmin, Policies.RequireAdminPolicy); 
//});
Console.WriteLine("Begin Build");
var app = builder.Build();
Console.WriteLine("End Build");

Console.WriteLine("Begin database build");
//Create database
using (var scope = app.Services.CreateScope())
{
    Console.WriteLine("Begin context scope grab");
    var context = scope.ServiceProvider.GetRequiredService<tutor_dbContext>();
    Console.WriteLine("Begin migrations");
    context.Database.Migrate();
    Console.WriteLine("Begin identity seed.");
    await IdentitySeed.SeedAsync(scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>(),
        scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>(), context);
    Console.WriteLine("Begin class seeding");
    Class.Seed(context);
    Console.WriteLine("Begin topic seeding");
    Topic.Seed(context);
    Console.WriteLine("Begin question seeding");
    Question.Seed(context);

}
Console.WriteLine("End database build");

Console.WriteLine("Begin HTTP configuration");
// Configure the HTTP request pipeline.
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    Console.WriteLine("Enable Swagger UI");
    app.UseSwaggerUI();
}
Console.WriteLine("End HTTP configuration");

app.UseHttpsRedirection();

app.UseCors(allowance);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
