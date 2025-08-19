using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using universityManagementSys.Data;
using universityManagementSys.DTOs;
using universityManagementSys.Filters;
using universityManagementSys.Models;
using universityManagementSys.Repositories.Implementations;
using universityManagementSys.Repositories.Interfaces;
using universityManagementSys.Services.Implementations;
using universityManagementSys.Services.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace universityManagementSys
{
    public class Program
    {
        public static void Main(string[] args)
        {
            
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAutoMapper(conf => { }, typeof(DTOsMapper));
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();

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


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
