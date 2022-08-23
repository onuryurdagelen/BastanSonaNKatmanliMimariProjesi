using Core.DependencyResolvers;
using Core.Extensions;
using Core.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi
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

            //Site bazlý izin vermek istiyorsak burasý kullanýlmalý.
            //services.AddControllers();
            //services.AddCors(options => {
            //    options.AddPolicy("AllowOrigin",
            //        builder => builder.WithOrigins("https://localhost:4200", "yeni site", "yeni site 2");
            //});

            //Eðer tüm istekleri karþýlmaka istiyorsak
            services.AddDependencyResolvers(new ICoreModule[]
            {
                new CoreModule()
            });
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    //Oluþturulacak token'ýn kullanýcýlarýn kontrol edip etmeyeceðini kontrol ederiz.
                    ValidateIssuer = true,
                    //Token'ý kimden aldýðýný belirteyim mi? kontrol edilir.
                    ValidateLifetime = true,
                    //Token üzerinde token'ýn bitiþ tarihini eklenecek mi? Eklenirse tokenýn geçerlilik tarihi sonra ererse token kullanýlamaz.
                    ValidateIssuerSigningKey = true,
                    //Üretecek token deðerinin uygulamamýza ait bir deðer olduðunu kontrol ederiz.SecurityKey deðerinin doðrulamasýdýr.
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidAudience = Configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });
        
            services.AddSwaggerGen();
        }
     
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            app.ConfigureCustomExceptionMiddleware();
            app.UseCors("AllowOrigin"); //Policy name yazýlýr.
            app.UseHttpsRedirection();

            app.UseAuthentication(); //Kullanýcýnýn token'a sahip olup olmadýðýný kontrol ederiz.
            app.UseRouting();

            app.UseAuthorization(); //Kullanýcýn o bölüme yetkisisinin olup olmadýðýný kontrol ederiz.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
