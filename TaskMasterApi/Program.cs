using Microsoft.EntityFrameworkCore;
using TaskMasterApi.Models;

namespace TaskMasterApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers(options => {
                options.SuppressAsyncSuffixInActionNames = false;
            });
            string connstr = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<TodoContext>(opt => opt.UseMySql(connstr, ServerVersion.AutoDetect(connstr)));
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

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}