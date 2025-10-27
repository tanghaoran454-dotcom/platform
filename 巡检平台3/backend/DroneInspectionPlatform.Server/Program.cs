using Furion;
using Microsoft.Extensions.DependencyInjection;

namespace DroneInspectionPlatform.Server;

/// <summary>
/// 应用程序入口类
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // 添加Furion框架
        builder.Services.AddInject(options =>
        {
            // 配置日志
            options.LogEnabled = true;
            
            // 配置跨域
            options.CorsEnabled = true;
            options.CorsPolicyName = "Default";
            
            // 配置JWT
            options.JwtBearerEnabled = true;
            
            // 配置API文档
            options.SpecificationDocumentEnabled = true;
            
            // 配置实时通信
            options.SignalREnabled = true;
        });

        var app = builder.Build();

        // 使用Furion中间件
        app.UseInject();

        await app.RunAsync();
    }
}

