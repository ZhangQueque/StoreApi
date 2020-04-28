using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Store.Api.Attributes;
using Store.Api.Filter;
using Store.Data;
using Store.Service;
using AutoMapper;
using Store.Api.RedisCache;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using System.IdentityModel.Tokens;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Store.Api.Models;

namespace Store.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add<NlogFilter>());
            services.AddDbContext<StoreDbContext>(options => options.UseMySql(Configuration["MySql"]));
            services.AddCors(options => options.AddPolicy("cors", handler => handler.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("product", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Product Api", Version = "1" });
                //Store.Api.xml
                var basePath = Directory.GetCurrentDirectory();//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "Store.Api.xml");
                option.IncludeXmlComments(xmlPath);
            });
            services.AddDistributedRedisCache(options =>
            {
                options.InstanceName = "cache_";
                options.Configuration = Configuration["Redis"];
            });
            services.AddAutoMapper(typeof(Startup));

            services.AddHttpClient("service", options =>
            {
                options.BaseAddress = new Uri("https://www.zhangqueque.top:5000/");
            });
            var security = Configuration.GetSection("Security:Token");
         
            services.AddAuthentication(options=> {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {

                        ValidIssuer = security["Issuer"],
                        ValidAudience = security["Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(security["Key"])),
                        NameClaimType = JwtClaimTypes.Name
                        
                    };
                });

            services.Configure<SecurityConfigOptions>(Configuration.GetSection("Security:Token"));
            //单例没办法注册DB上下文
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

           
            services.AddScoped<IsExistProductAttribute>();
            services.AddScoped<RedisCacheHelper>();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseStaticFiles();
            app.UseCors("cors");
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/product/swagger.json", "Product Api"));
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
