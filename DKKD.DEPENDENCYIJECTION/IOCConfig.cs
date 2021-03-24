using DKKD.MANAGER;
using DKKD.MODELS;
using DKKD.REPOSITORY;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Portal.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace DKKD.DependencyInjection
{
    public class IOCConfig
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddDbContext<ShopContext>(ServiceLifetime.Scoped, ServiceLifetime.Singleton);
            services.AddTransient<IDbContextFactory<ShopContext>, ShopContextFactory>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<INhaCungCapManager, NhaCungCapManager>();
            services.AddTransient<IDanhMucManager, DanhMucManager>();
            services.AddTransient<ISanPhamManager, SanPhamManager>();
            services.AddTransient<IDonHangManager, DonHangManager>();
        }
    }
}
