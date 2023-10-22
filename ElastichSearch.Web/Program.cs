using ElastichSearch.Web.Extensions;
using ElastichSearch.Web.Repository;
using ElastichSearch.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddElasctic(builder.Configuration);
builder.Services.AddScoped<BlogService>();
builder.Services.AddScoped<ECommerceService>();
builder.Services.AddScoped<BlogRepository>();
builder.Services.AddScoped<ECommerceRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
