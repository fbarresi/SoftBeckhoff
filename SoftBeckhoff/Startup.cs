using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace SoftBeckhoff
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
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
                    Version = "v1",
			        Title = "SoftBeckhoff API",
			        Description = ".NET Core Web API to SoftBeckhoff",
			        Contact = new Microsoft.OpenApi.Models.OpenApiContact {
                        Name = "Federico Barresi",
                        Url = new Uri("https://github.com/fbarresi/SoftBeckhoff")
                    },
			        License = new Microsoft.OpenApi.Models.OpenApiLicense
			        {
				        Name = "Use under MIT",
				        Url = new Uri("https://github.com/fbarresi/SoftBeckhoff/blob/master/LICENSE")
					}
                });
                
                // Set the comments path for the Swagger JSON and UI.
		        //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
		        //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
		        //c.IncludeXmlComments(xmlPath);
            });
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
	        app.UseSwagger();

	        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
	        // specifying the Swagger JSON endpoint.
	        app.UseSwaggerUI(c =>
	        {
		        c.SwaggerEndpoint("/swagger/v1/swagger.json", "SoftBeckhoff API V1");
	        });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

			app.UseCors(options => options.AllowAnyOrigin());
        }
    }
}
