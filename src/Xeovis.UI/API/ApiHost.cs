using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Xeovis.UI;

public static class ApiHost
{
    public static event EventHandler<BundleReloadCalledEventArgs> BundleReloadCalled;
    public static event EventHandler<LoadNewBundleCalledEventArgs> LoadNewBundleCalled;
    public static event EventHandler<RefreshBundleUIAskedEventArgs> RefreshBundleUIAsked;
    public static event EventHandler<ApiActivityChangedEventArgs> ApiActivityChanged;
    
    private record ReloadRequest(string Name, bool Append);
    private record LoadRequest(string Name, string Directory);
    private record RefreshRequest(string Name);

    public static async Task Start()
    {
        var builder = WebApplication.CreateBuilder();
        var app = builder.Build();

        // Example GET endpoint
        app.MapGet("/status", () => new { status = "running" });
            
        // Example POST endpoint to receive data
        app.MapPost("/update", async (HttpContext context) =>
        {
            using var reader = new StreamReader(context.Request.Body);
            var json = await reader.ReadToEndAsync();
            
            var msg = $"Received from Mathematica: {json}";
            return Results.Ok(msg);
        });
        
        // Xeovis
        app.MapPost("/Load", (LoadRequest bundle) =>
        {
            ApiActivityChanged?.Invoke(null, new  ApiActivityChangedEventArgs($"Loading {bundle.Name}"));
            LoadNewBundleCalled?.Invoke(null, new LoadNewBundleCalledEventArgs(bundle.Name, bundle.Directory));
            return Results.Ok();
        });
        
        app.MapPost("/Reload", (ReloadRequest data) =>
        {
            ApiActivityChanged?.Invoke(null, new  ApiActivityChangedEventArgs($"Loading {data.Name}"));
            BundleReloadCalled?.Invoke(null, new BundleReloadCalledEventArgs(data.Name, data.Append));
            return Results.Ok(new { status = "ok" });
        });
        
        app.MapPost("/Refresh", (RefreshRequest bundle) =>
        {
            ApiActivityChanged?.Invoke(null, new  ApiActivityChangedEventArgs($"Refreshing UI for {bundle.Name}"));
            RefreshBundleUIAsked?.Invoke(null, new RefreshBundleUIAskedEventArgs(bundle.Name));
            return Results.Ok(new { status = "ok" });
        });
        
        await app.RunAsync("http://localhost:5007");
    }
}