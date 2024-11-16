using Microsoft.EntityFrameworkCore;
using WebStore.BlazorApp;
using WebStore.Data;

var builder = WebApplication.CreateBuilder(args);

// �������� ������ ����������� �� ����� ������������
string connection = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connection));
builder.Services.AddControllersWithViews();
// ��������� ������� ����������� Razor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();  // ��������� ������� ��� ���������� ����������
                                        

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


app.UseAntiforgery(); //��� ������ ����������� Blazor ����� ���������� ���������� middleware ��� ������ �� ���������� ��������:
					  // ������������� �������� ��������� � ���������� ��� � �������� ��������� �������
app.MapRazorComponents<ReportAnalise>()
    .AddInteractiveServerRenderMode();  // ��������� ������������� ��������� ������� 

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
