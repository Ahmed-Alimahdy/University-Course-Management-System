using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using universityManagementSys.Data;
using universityManagementSys.DTOs;
using universityManagementSys.Filters;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Implementations;
using universityManagementSys.Repositories.Interfaces;
using universityManagementSys.Services.Implementations;
using universityManagementSys.Services.Interfaces;

namespace universityManagementSys
{
    public class Program
    {
        public static void Main(string[] args)
        {
           
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddAutoMapper(conf => { }, typeof(DTOsMapper));
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllersWithViews();
            
            //Add Swagger versioning
            builder.Services.AddEndpointsApiExplorer();

            //Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200", "https://myclientapp.com") // الكلاينت المسموح بيه
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });





            // Add services to the container.


            builder.Services.AddDbContext<Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<ValidateModelNotEmptyFilter>();
            builder.Services.AddScoped<DbExceptionFilter>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            builder.Services.AddScoped<IDepartmentCourseRepository, DepartmentCourseRepository>();

            builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();

            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IGradeRepository, GradeRepository>();
            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();

            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(conf =>
            {
                conf.Password.RequireDigit = true;
                conf.Password.RequireLowercase = true;
                conf.Password.RequireUppercase = true;
                conf.Password.RequiredLength = 8;
            })
                  .AddEntityFrameworkStores<Context>();
            
            builder.Services.AddApiVersioning(options =>
            {
                //Default versioning strategy
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);

                //Display API versions in the header response
                options.ReportApiVersions = true;

                //Versioning by query string, header, or URL segment
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("X-Version"),
                    new UrlSegmentApiVersionReader()
                );
            });
            var app = builder.Build();

            //Active swagger UI
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    //API Versioning
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                });
            }
            //Active swagger UI
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    //API Versioning
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                });
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("AllowAll");
            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
