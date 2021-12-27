using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Repository;
using Repository.Mongo;
using Repository.SQLServer;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiscoitoAPI
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
            services.AddControllers();

            #region SWAGGER
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "BiscoitoAPI",
                    Description = "Biscoito project API endpoints",
                    TermsOfService = new Uri("https://www.gnu.org/licenses/gpl-3.0.pt-br.html"),
                    Contact = new OpenApiContact
                    {
                        Name = "Contact",
                        Email = string.Empty,
                        Url = new Uri("https://www.linkedin.com/in/lucazsimoes/"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Licence",
                        Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.pt-br.html"),
                    }
                });

                c.AddSecurityDefinition("Administrator", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header usando um Bearer scheme. \r\n\r\n Type 'Bearer' [space] and your token inside the text input bellow.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },

                            Scheme = "JwtBearerDefaults",
                            Name = "Administrator",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
            #endregion

            #region TRANSIENTS
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IPasswordRepository, PasswordRepository>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();

            #endregion

            services.AddAuthentication();
            services.AddDistributedMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BiscoitoAPI v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
