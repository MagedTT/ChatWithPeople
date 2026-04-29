using ChatWithPeople;
using ChatWithPeople.Extensions;
using ChatWithPeople.Hubs;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ApiBehaviorOptions>(config =>
{
    config.SuppressModelStateInvalidFilter = true;
});

builder.Services.ConfigureCORS();

builder.Services.AddSignalR();

builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true)
    .AddApplicationPart(typeof(ChatWithPeople.Presentation.AssemblyReference).Assembly);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(options =>
{
    options.AddProfile<MappingProfile>();
});

builder.Services.ConfigureSQLConnection();
builder.Services.ConfigureLoggerManager();
builder.Services.ConfigureServiceManager();
builder.Services.ConfigureRepositoryManager();
builder.Services.ConfigureIdentity();
builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.ConfigureJWT(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<ConversationHub>("/hubs/ConversationHub");

app.Run();
