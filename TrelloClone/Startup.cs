using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using TrelloClone.Data;
using TrelloClone.Infra;
using TrelloClone.Interfaces;

namespace TrelloClone
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
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // 데이터
            services.AddAutoMapper();
            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // 의존성 주입
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IBoardRepository, BoardRepository>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new Info { Title = "Trello Clone API", Version = "v1" });
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<Config>(appSettingsSection);

            // JWT Authentication
            var appSettings = appSettingsSection.Get<Config>();
            var securityKey = Encoding.ASCII.GetBytes(appSettings.JWTSecurityKey);

            services.AddAuthentication(x => {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options => {
                    options.Events = new JwtBearerEvents {
                        OnTokenValidated = context => {

                            // IP 검증
                            var ipClaim = context.Principal.FindFirst(appSettings.IPClaimType);
                            if (ipClaim == null
                                || ipClaim.Value != context.HttpContext.Connection.RemoteIpAddress.ToString()) {
                                context.Fail("Unauthorized");
                            }
                            return Task.CompletedTask;
                        },
                    };

                    var symmetricSecurityKey = new SymmetricSecurityKey(securityKey);
                    options.TokenValidationParameters = new TokenValidationParameters {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidIssuer = "trelloclone",
                        ValidAudience = "readers",
                        IssuerSigningKey = symmetricSecurityKey
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Trello Clone API V1");
                });
            }
            else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // 쿠키로 넘어온 토큰 헤더 주입
            app.UseMiddleware<JWTInHeaderMiddleware>();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());

            // static file 사용
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // 권한
            app.UseAuthentication();
            app.UseHttpsRedirection();

            // 토큰 일정 만료기간내 요청시 재발급
            app.UseMiddleware<JWTRefreshMiddleware>();

            // Mvc
            app.UseMvc();
        }
    }
}
