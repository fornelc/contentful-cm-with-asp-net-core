using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Contentful.AspNetCore;
using Nest;
using Elasticsearch.Net;
using Products.Models;

namespace Products
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
            services.AddContentful(Configuration);
            services.AddControllersWithViews();
            services.AddSingleton<IElasticClient>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>();

                var settings = new ConnectionSettings("enterprise-search-deployment:dXMtd2VzdDEuZ2NwLmNsb3VkLmVzLmlvJDllMjY4YzgxNzE3ZDQzYWViZTEwNTFjNWM3OWZiNDZmJDJiZmU1YTc4ZmI5ZjRjOGZiYzljNzlkYWYxOTE0Nzhi",
                    new BasicAuthenticationCredentials(
                        "elastic", "nzw8oH4A4caAIQXliYKC3bCi"))
                        .DefaultIndex("contentful-entries")
                        .DefaultMappingFor<Product>(i => i.IndexName("contentful-entries"));

                return new ElasticClient(settings);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
