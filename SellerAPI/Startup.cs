using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SellerAPI.Repositories;
using SellerAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SellerAPI
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SellerAPI", Version = "v1" });
            });
            services.AddScoped<ISellerService, SellerService>();
            services.AddSingleton<ISellerRepository>(InitializeCosmosClientIntance(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult()); 
        }

        private static async Task<ISellerRepository> InitializeCosmosClientIntance(IConfigurationSection configurationSection)
        {
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName"];

            var cosmosClient = new CosmosClient(account, key);
            var db = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
            await db.Database.CreateContainerIfNotExistsAsync(containerName,"/id");
            var productRepository = new SellerRepository(cosmosClient,databaseName,containerName);
            return productRepository;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SellerAPI v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}