using Edge2.WebAPIs.Entities;
using Edge2.WebAPIs.Helpers;
using Edge2.WebAPIs.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace Edge2.WebAPIs
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
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Edge 2.0 API",
                    Version = "v1",
                    Description = "Web APIs for Edge 2.0 React App",
                    Contact = new OpenApiContact
                    {
                        Name = "Durga Prasad Chimmili",
                        Email = "durga.prasad@cybersoft.net",
                        Url = new Uri("https://test.primeroedge.co/System/Home.aspx")
                    },
                });
            });

            services.AddDbContext<DataContext>(x => x.UseInMemoryDatabase("Edge2Db"));
            services.AddCors();
            services.AddControllers().AddNewtonsoftJson().AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
                    ValidateAudience = false,

                    ClockSkew = TimeSpan.Zero
                };
            });

            // Dependency Injection
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMoviesService, MoviesService>();
            services.AddScoped<IMovieGenresService, MovieGenresService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseDefaultFiles(new DefaultFilesOptions
            //{
            //    DefaultFileNames = new
            //    List<string> { "index.html" }
            //});
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Edge 2.0 API V1");
                c.DocumentTitle = "Web APIs for Edge 2.0 React App";
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
            context.Users.Add(new User { FirstName = "Edge2", LastName = "User", Username = "Edge2User", Password = "Edge2$", IsAdmin = true });
            context.SaveChanges();
            context.MovieGenres.Add(new Entities.MovieGenre { Id = 1, Name = "Comedy" });
            context.MovieGenres.Add(new Entities.MovieGenre { Id = 2, Name = "Action" });
            context.MovieGenres.Add(new Entities.MovieGenre { Id = 3, Name = "Romance" });
            context.MovieGenres.Add(new Entities.MovieGenre { Id = 4, Name = "Thriller" });
            context.SaveChanges();

            context.Movies.Add(new Movie { Id = 1, MovieGenreId = 1, Title = "Airplane", NumberInStock = 5, DailyRentalRate = 2 });
            context.Movies.Add(new Movie { Id = 2, MovieGenreId = 1, Title = "The Hangover", NumberInStock = 10, DailyRentalRate = 2 });
            context.Movies.Add(new Movie { Id = 3, MovieGenreId = 1, Title = "Wedding Crashers", NumberInStock = 15, DailyRentalRate = 2 });

            context.Movies.Add(new Movie { Id = 4, MovieGenreId = 2, Title = "Die Hard", NumberInStock = 5, DailyRentalRate = 2 });
            context.Movies.Add(new Movie { Id = 5, MovieGenreId = 2, Title = "Terminator", NumberInStock = 10, DailyRentalRate = 2 });
            context.Movies.Add(new Movie { Id = 6, MovieGenreId = 2, Title = "The Avengers", NumberInStock = 15, DailyRentalRate = 2 });

            context.Movies.Add(new Movie { Id = 7, MovieGenreId = 3, Title = "The Notebook", NumberInStock = 5, DailyRentalRate = 2 });
            context.Movies.Add(new Movie { Id = 8, MovieGenreId = 3, Title = "When Harry Met Sally", NumberInStock = 10, DailyRentalRate = 2 });
            context.Movies.Add(new Movie { Id = 9, MovieGenreId = 3, Title = "Pretty Woman", NumberInStock = 15, DailyRentalRate = 2 });

            context.Movies.Add(new Movie { Id = 10, MovieGenreId = 4, Title = "The Sixth Sense", NumberInStock = 5, DailyRentalRate = 2 });
            context.Movies.Add(new Movie { Id = 11, MovieGenreId = 4, Title = "Gone Girl", NumberInStock = 10, DailyRentalRate = 2 });
            context.Movies.Add(new Movie { Id = 12, MovieGenreId = 4, Title = "The Others", NumberInStock = 15, DailyRentalRate = 2 });
            context.SaveChanges();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .SetIsOriginAllowed(origin => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(x => x.MapControllers());
        }
    }
}
