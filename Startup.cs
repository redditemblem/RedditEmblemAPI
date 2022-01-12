using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RedditEmblemAPI.Services;

namespace RedditEmblemAPI
{
    public class Startup
    {
        public const string AppS3BucketKey = "AppS3Bucket";
        private const string AllowedOriginsPolicy = "_AllowedOriginsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            //Allow requests from restricted origins
            services.AddCors(options =>
            {
                options.AddPolicy(AllowedOriginsPolicy,
                builder =>
                {
                    builder.WithOrigins("http://127.0.0.1:8080",
                                        "https://redditemblem.github.io")
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            services.AddRazorPages().AddNewtonsoftJson();

            // Add S3 to the ASP.NET Core dependency injection framework.
            services.AddAWSService<Amazon.S3.IAmazonS3>();
            services.AddSingleton<IAPIService, APIService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(AllowedOriginsPolicy);
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
