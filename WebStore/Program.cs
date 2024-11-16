using Microsoft.EntityFrameworkCore;
using WebStore.BlazorApp;
using WebStore.Data;

var builder = WebApplication.CreateBuilder(args);

// получаем строку подключения из файла конфигурации
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddControllersWithViews();
// добавляем сервисы компонентов Razor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();  // добавляем сервисы для серверного рендеринга
                                        

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


app.UseAntiforgery(); //Для работы компонентов Blazor также необходимо установить middleware для защиты от поддельных ресурсов:
					  // устанавливает корневой компонент и встраиваем его в конвейер обработки запроса
app.MapRazorComponents<ReportAnalise>()
    .AddInteractiveServerRenderMode();  // добавляем интерактивный рендеринг сервера 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
