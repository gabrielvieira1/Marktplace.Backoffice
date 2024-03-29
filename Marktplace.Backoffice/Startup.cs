using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Marktplace.Backoffice
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
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Marktplace.Backoffice", Version = "v1" });
      });

      services.AddAuthentication(options =>

      {

        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

      }).AddJwtBearer(options =>
      {
        options.RequireHttpsMetadata = bool.Parse(Configuration["Authentication:RequireHttpsMetadata"]);

        options.Authority = Configuration["Authentication:Authority"];

        options.IncludeErrorDetails = bool.Parse(Configuration["Authentication:IncludeErrorDetails"]);

        options.TokenValidationParameters = new TokenValidationParameters()
        {

          ValidateAudience = bool.Parse(Configuration["Authentication:ValidateAudience"]),

          ValidAudience = Configuration["Authentication:ValidAudience"],

          ValidateIssuerSigningKey = bool.Parse(Configuration["Authentication:ValidateIssuerSigningKey"]),

          ValidateIssuer = bool.Parse(Configuration["Authentication:ValidateIssuer"]),

          ValidIssuer = Configuration["Authentication:ValidIssuer"],

          ValidateLifetime = bool.Parse(Configuration["Authentication:ValidateLifetime"]),
        };
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Marktplace.Backoffice v1"));
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
