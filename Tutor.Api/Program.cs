using Tutor.api.Services;

var builder = WebApplication.CreateBuilder(args);

//Change CORS policy

string allowance = "AllowAll";

var allowAll = builder.Services.AddCors(options => {
    options.AddPolicy(allowance, builder =>
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ILeaderBoardService, LeaderBoardServiceMemory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseCors(allowance);

app.UseAuthorization();

app.MapControllers();

app.Run();
