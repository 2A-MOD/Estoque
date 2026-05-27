using Estoque.Interfaces;
using Estoque.Repository;
using EstoqueLoja.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Configurando a autenticação usando cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => {
        options.LoginPath = "/Usuario/Login"; // Onde o usuário cai se não estiver logado
        options.AccessDeniedPath = "/Usuario/AcessoNegado"; // Se não tiver permissão
    });



// registrando injecao de dependencia
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

// Habilita a autenticação e autorização no pipeline de requisições
app.UseAuthentication();
// Habilita a autorização para proteger as rotas que exigem autenticação
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
