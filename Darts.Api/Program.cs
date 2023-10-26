using Darts.Api.Attributes;
using Darts.Api.Data;
using Darts.Api.Data.Repositories;
using Darts.Api.Data.Repositories.Factories;
using Darts.Api.PlatformSpecific;
using Darts.Api.Services;
using Darts.Contract.Interfaces;
using Darts.Domain;
using Darts.Domain.Abstracts;
using Darts.Domain.Abstracts.Factories;
using Darts.Domain.Services;
using Development.Support.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Darts.Api
{
   public class Program
   {
      public static void Main(string[] args)
      {
         //ConfigureLogging();
         CreateHostBuilder(args).Run();

      }

		public static WebApplication CreateHostBuilder(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			builder.Host.UseSerilog((ctx, lc) => lc
             .MinimumLevel.Warning()
             .WriteTo.Console()
				 .WriteTo.Seq("http://194.60.87.105:5341"));

         builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
         builder.Configuration.AddJsonFile(
             $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
             optional: true);

         var appSettingsSection = builder.Configuration.GetSection("AppSettings");
         builder.Services.Configure<AppSettings>(appSettingsSection);

         builder.Services.AddCors();

         var appSettings = appSettingsSection.Get<AppSettings>();
         var key = Encoding.ASCII.GetBytes(appSettings.Secret);
         builder.Services.AddAuthentication(x =>
         {
            x.DefaultAuthenticateScheme = "Bearer";
            x.DefaultChallengeScheme = "Bearer";
         })
            .AddJwtBearer(x =>
            {
               x.RequireHttpsMetadata = false;
               x.SaveToken = true;
               x.TokenValidationParameters = new TokenValidationParameters
               {
                  ValidateIssuerSigningKey = true,
                  IssuerSigningKey = new SymmetricSecurityKey(key),
                  ValidateIssuer = false,
                  ValidateAudience = false
               };
            });

         builder.Services.AddControllers().AddNewtonsoftJson(options =>
         {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
         });

         builder.Services.AddMvc().AddNewtonsoftJson(options =>
         {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
         });

         builder.Services.AddScoped<IUserRepository, UserRepository>();
         builder.Services.AddScoped<ILeagueRepository, LeagueRepository>();
         builder.Services.AddScoped<ITeamsRepository, TeamsRepository>();
         builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
         builder.Services.AddScoped<IMatchRepository, MatchRepository>();
         builder.Services.AddScoped<IStatsRepository, StatsRepository>();
         builder.Services.AddScoped<ILeagueTeamRepository, LeagueTeamRepository>();
         builder.Services.AddScoped<ILeagueTeamPlayerRepository, LeagueTeamPlayerRepository>();
         builder.Services.AddScoped<IPremierLeaguePlayerRepository, PremierLeaguePlayerRepository>();
         builder.Services.AddScoped<IPremierLeagueMatchRepository, PremierLeagueMatchRepository>();
         builder.Services.AddScoped<IRequestRegistrationRepository, RequestRegistrationRepository>();

         builder.Services.AddScoped<TeamsService>();
         builder.Services.AddScoped<AuthenticateService>();
         builder.Services.AddScoped<LeagueService>();
         builder.Services.AddScoped<PlayerService>();
         builder.Services.AddScoped<MatchService>();
         builder.Services.AddScoped<StatsService>();
         builder.Services.AddScoped<PremierLeagueService>();
         builder.Services.AddScoped<PremierLeagueMatchService>();
         builder.Services.AddScoped<UserService>();
         builder.Services.AddScoped<PremierLeaguePlayerService>();
         builder.Services.AddScoped<RequestRegistrationService>();
         builder.Services.AddSingleton<IEmailService, EmailService>();
         builder.Services.AddSingleton<ILogService>(s => new ApiLogger("http://194.60.87.105:5341"));
         builder.Services.AddSingleton<IPassword, Password>();

         var connectionString = builder.Configuration.GetConnectionString("DataAccessContabo");
         builder.Services.AddDbContext<DartsContext>(options => options.UseSqlServer(connectionString));
         builder.Services.AddTransient<IGenericContextFactory>(s => new MssqlDbContextFactory(connectionString));

         builder.Services.AddSwaggerGen(c =>
         {
            c.OperationFilter<SwaggerParameterAttributeFilter>();
            c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Darts API", Version = "v1" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
         });

         builder.Services.Configure<ApiBehaviorOptions>(options =>
         {
            options.SuppressModelStateInvalidFilter = true;
         });

         var app = builder.Build();

         if (builder.Environment.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
         }

         using var scope = app.Services.CreateScope();
         var context = scope.ServiceProvider.GetRequiredService<DartsContext>();
         context.Database.EnsureCreated();

         app.UseHttpsRedirection();
         app.UseStaticFiles();
         app.UseCors(options => options.WithOrigins("http://194.60.87.105:8080/api").AllowAnyMethod().AllowAnyHeader().AllowAnyOrigin());

         app.UseRouting();
         app.UseAuthentication();
         app.UseAuthorization();
         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
         });

         app.UseHttpLogging();
         app.UseSwagger();

         // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
         // specifying the Swagger JSON endpoint.
         app.UseSwaggerUI(c =>
         {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Darts V1");
            c.RoutePrefix = String.Empty;
         });

         return app;
      }

		private static void ConfigureLogging()
		{
			var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile(
					$"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
					optional: true)
				.Build();

			//Log.Logger = new LoggerConfiguration()
			//	.Enrich.FromLogContext()
			//	.Enrich.WithMachineName()
			//	.WriteTo.Debug()
			//	.WriteTo.Console()
			//	.WriteTo.Elasticsearch(ConfigureElasticSink(configuration, environment))
			//	.Enrich.WithProperty("Environment", environment)
			//	.ReadFrom.Configuration(configuration)
			//	.CreateLogger();
		}

		private static ElasticsearchSinkOptions ConfigureElasticSink(IConfigurationRoot configuration, string environment)
		{
			var elasticUri = configuration["ElasticConfiguration:Uri"];

			return new ElasticsearchSinkOptions(new Uri(elasticUri))
			{
				AutoRegisterTemplate = true,
				IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{environment?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
			};
		}
	}
}
