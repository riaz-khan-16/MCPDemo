var builder = WebApplication.CreateBuilder(args); //CreateBuilder(): Sets up everything your Web API needs before it starts.

// Add services to the container.

builder.Services.AddControllers(); //enables the controller
builder.Services.AddEndpointsApiExplorer(); // find all your API endpoints so Swagger/OpenAPI can show them.
builder.Services.AddSwaggerGen(); // turn on Swagger UI for API testing.

builder.Services.AddCors(options =>           // enables CORS support in your Web API.It controls which websites are allowed to call your API
{
    options.AddPolicy("AllowAngular",  // You are creating a CORS rule (policy) named “AllowAngular”.
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "http://localhost:60309")  // This tells your API to allow requests only from your Angular app running at
                .AllowAnyHeader()   //allows the Angular app to send any HTTP headers to your API.
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


var app = builder.Build();

app.UseCors("AllowAngular");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run(); 
