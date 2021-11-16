using DinkToPdf;
using DinkToPdf.Contracts;
using KEDB.Audit;
using KEDB.Data;
using KEDB.Data.Interface;
using KEDB.Data.Repository;
using KEDB.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
using Microsoft.Identity.Web;

namespace KEDB
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /* services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                  .AddMicrosoftIdentityWebApi(Configuration, "AzureAd");  */

            services.AddHealthChecks()
                .AddDbContextCheck<KEDBContext>();

            services.AddDbContext<KEDBContext>(options =>
            {
                options.UseSqlite(
                    Configuration["ConnectionStrings:DefaultConnection"]);
            });

            services.AddScoped<IKontrolrapportRepository, KontrolrapportRepository>();

            services.AddScoped<IToldrapportRepository, ToldrapportRepository>();
            services.AddScoped<IToldrapportTransportmiddelRepository, ToldrapportTransportmiddelRepository>();
            services.AddScoped<IToldrapportOpdagendeAktoerRepository, ToldrapportOpdagendeAktoerRepository>();
            services.AddScoped<IToldrapportFejlKategoriRepository, ToldrapportFejlKategoriRepository>();
            services.AddScoped<IToldrapportKommunikationRepository, ToldrapportKommunikationRepository>();
            services.AddScoped<IToldrapportOvertraedelsesAktoerRepository, ToldrapportOvertraedelsesAktoerRepository>();

            services.AddScoped<IRubrikRepository, RubrikRepository>();
            services.AddScoped<IRubrikMuligFejlRepository, RubrikMuligFejlRepository>();
            services.AddScoped<IRubrikTypeRepository, RubrikTypeRepository>();
            services.AddScoped<IProfilRepository, ProfilRepository>();
            services.AddScoped<IFejltekstRepository, FejltekstRepository>();

            services.AddScoped<GraphServiceClient>(_ => GraphServiceClientFactory.Create(Configuration["AzureAd:ClientId"], Configuration["AzureAd:TenantId"], Configuration["AzureAd:ClientSecret"]));
            services.AddScoped<IReportService, ReportService>();
            services.AddSingleton<IConfiguration>(Configuration);

            /* services.AddSingleton<IAuditLog, AzureEventHubAuditLog>(
               _ => new AzureEventHubAuditLog(Configuration["ConnectionStrings:AzureEventHubAuditLog"])); */

            services.AddSingleton<IAuditLog, AzureEventHubAuditLog>();

            services.AddControllers();

            services.AddAutoMapper(typeof(Startup).Assembly);

            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder //.WithOrigins("http://localhost:3000", "http://localhost:5001", "")
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseCors(MyAllowSpecificOrigins);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}