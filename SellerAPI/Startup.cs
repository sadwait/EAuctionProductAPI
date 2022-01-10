using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using SellerAPI.Common;
using SellerAPI.MessageBroker;
using SellerAPI.Models;
using SellerAPI.Repositories;
using SellerAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
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

            services.AddScoped<ICacheService, CacheService>();

            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = Configuration.GetValue<string>("RedisCacheConString");
            });

            services.AddScoped<IRabbitMqListener, RabbitMqListener>();

            services.AddSingleton(service => {
                var _config = Configuration.GetSection("RabbitMQ");
                return new ConnectionFactory() {
                    HostName = _config["HostName"],
                    UserName = _config["UserName"],
                    Password = _config["Password"],
                    Port = Convert.ToInt32(_config["Port"]),
                    VirtualHost = _config["VirtualHost"],
                   }; 
            });

            services.AddScoped<ISellerService, SellerService>();
            services.AddSingleton<ISellerRepository>(InitializeCosmosClientIntance(Configuration.GetSection("CosmosDb")).GetAwaiter().GetResult());
            services.AddCors(c => { c.AddPolicy("AllowOrigin", option => option.AllowAnyMethod()); });
        }

        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();               
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SellerAPI v1"));

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors(cors=>cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static async Task<ISellerRepository> InitializeCosmosClientIntance(IConfigurationSection configurationSection)
        {
            var account = configurationSection["Account"];
            var key = configurationSection["Key"];
            var databaseName = configurationSection["DatabaseName"];
            var containerName = configurationSection["ContainerName"];

            var cosmosClient = new CosmosClient(account, key);
            var db = await cosmosClient.CreateDatabaseIfNotExistsAsync(databaseName);
            await db.Database.CreateContainerIfNotExistsAsync(containerName, "/id");
            var productRepository = new SellerRepository(cosmosClient, databaseName, containerName);
            return productRepository;
        }       
    }
}
