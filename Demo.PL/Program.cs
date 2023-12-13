using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Context;
using Demo.DAL.Models;
using Demo.PL.MappingProfiles;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.PL
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var Builder = WebApplication.CreateBuilder(args);
            #region Configure Services that allow dependency Injection
            Builder.Services.AddControllersWithViews();
            Builder.Services.AddDbContext<MVCAppDbContext>(Options =>
            {
                Options.UseSqlServer(Builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            ; // Allow Dependency Injection 
            Builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            Builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            //services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile())); // Transient Liftime
            //services.AddAutoMapper(M => M.AddProfile(new DepartmentProfile())); // Transient Liftime
            Builder.Services.AddAutoMapper(M => M.AddProfiles(new List<Profile>() { new EmployeeProfile(), new DepartmentProfile(), new UserProfile(), new RoleProfile() }));

            Builder.Services.AddScoped<UserManager<ApplicationUser>>();

            Builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(Options =>
                {
                    Options.LoginPath = ("Account/Login");
                    Options.AccessDeniedPath = "Home/Error";
                });

            Builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Options =>
            {
                Options.Password.RequireNonAlphanumeric = true; // @ #
                Options.Password.RequireDigit = true; // 123
                Options.Password.RequireLowercase = true; // aa
                Options.Password.RequireUppercase = true; // AF
            })
                    .AddEntityFrameworkStores<MVCAppDbContext>()
                    .AddDefaultTokenProviders();

            Builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Allow Dependency Injection Class Department Repository
            #endregion

            var app = Builder.Build();
            #region Configure Http Request PipeLines
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });

            #endregion

            app.Run();
    }


    }
}
