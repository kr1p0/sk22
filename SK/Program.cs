using Microsoft.AspNetCore.HttpOverrides;
using SK.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//Configure oracle connection
builder.Services.AddHostedService<OracleConnectionConfiguration>();

// test for Razor.RuntimeCompilation
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//For Ajax
builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");

//For background action
builder.Services.AddHostedService<BackgroundProcess>();

//For Authentication
builder.Services.AddAuthentication("Ciastko").AddCookie("Ciastko", options =>
{
    options.Cookie.Name = "Ciastko";
    options.LoginPath = "/Login";
    //options.AccessDeniedPath =
    //options.ExpireTimeSpan = TimeSpan.FromSeconds(10); //closing the browser deletes the cookie

});


//for aws
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});


//For realTime communication @signalR
builder.Services.AddSignalR();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




//test
//Because of icrosoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware => Failed to determine the https port for redirect.
app.UseForwardedHeaders();
//test

/*
//test
app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next();
});
//test
*/

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapHub<SK.Hubs.NotificationHub>("/NotificationHub");  //signalR
});

app.MapRazorPages();

app.Run();
