using TimeMap.Core.Interfaces;
using TimeMap.Core.Repositories;

namespace TimeMap.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddAuthorization();
            builder.Services.AddConnections();
            builder.Services.AddControllers();

            // Настройка CORS для работы с фронтендом
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.AllowAnyOrigin()           // Разрешаем любые источники (включая file://)
                         .AllowAnyMethod()           // Разрешаем любые HTTP методы
                         .AllowAnyHeader();          // Разрешаем любые заголовки
                });
            });

            builder.Services.AddSingleton<IUserRepository, JsonUserRepository>();
            builder.Services.AddSingleton<IAvailabilityRepository, JsonAvailabilityRepository>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Включаем CORS (должен быть перед UseHttpsRedirection)
            app.UseCors("AllowFrontend");

            app.UseHttpsRedirection();

            app.MapControllers();
            //app.UseAuthorization();

            app.Run();
        }
    }
}
