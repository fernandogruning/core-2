using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebAPI.Data;
using WebAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace WebAPI {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddDbContext<Core2DbContext>(options => options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("Core2Db")));

      services.AddScoped<ClientService>();
      services.AddScoped<AccountService>();
      services.AddScoped<TransactionService>();
      services.AddScoped<LoanService>();

      // this down below was the default
      // services.AddControllersWithViews();
      services.AddMvc().AddNewtonsoftJson().SetCompatibilityVersion(CompatibilityVersion.Latest);
      services.AddControllersWithViews();

      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Core 2 Web API", Version = "v1" });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {

      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      } else {
        // app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }
      app.UseHttpsRedirection();
      // app.UseStaticFiles();

      app.UseSwagger();

      app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Core 2 Web API v1");
        c.RoutePrefix = string.Empty;
      });

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}
