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

            //Site bazl� izin vermek istiyorsak buras� kullan�lmal�.
            //services.AddControllers();
            //services.AddCors(options => {
            //    options.AddPolicy("AllowOrigin",
            //        builder => builder.WithOrigins("https://localhost:4200", "yeni site", "yeni site 2");
            //});

            //E�er t�m istekleri kar��lmaka istiyorsak
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
                    //Olu�turulacak token'�n kullan�c�lar�n kontrol edip etmeyece�ini kontrol ederiz.
                    ValidateIssuer = true,
                    //Token'� kimden ald���n� belirteyim mi? kontrol edilir.
                    ValidateLifetime = true,
                    //Token �zerinde token'�n biti� tarihini eklenecek mi? Eklenirse token�n ge�erlilik tarihi sonra ererse token kullan�lamaz.
                    ValidateIssuerSigningKey = true,
                    //�retecek token de�erinin uygulamam�za ait bir de�er oldu�unu kontrol ederiz.SecurityKey de�erinin do�rulamas�d�r.
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
            app.UseCors("AllowOrigin"); //Policy name yaz�l�r.
            app.UseHttpsRedirection();

            app.UseAuthentication(); //Kullan�c�n�n token'a sahip olup olmad���n� kontrol ederiz.
            app.UseRouting();

            app.UseAuthorization(); //Kullan�c�n o b�l�me yetkisisinin olup olmad���n� kontrol ederiz.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
