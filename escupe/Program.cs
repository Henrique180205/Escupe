using escupe.Data;
using escupe.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Teste de conexão com a mesma string usada no EF Core
            TestDatabaseConnection(builder.Configuration.GetConnectionString("DefaultConnection"));

            // Configuração de serviços
            builder.Services.AddControllersWithViews();
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Home/Index";
                    options.LogoutPath = "/Home/Logout";
                    options.AccessDeniedPath = "/Home/Index";
                });

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer("Server=.\\SQLEXPRESS;Database=EscupeDB;Trusted_Connection=True;TrustServerCertificate=True;")
                       .EnableSensitiveDataLogging()
                       .LogTo(Console.WriteLine));

            // Registro dos serviços e APIs
            builder.Services.AddHttpClient();
            builder.Services.AddScoped<ICPFService, CPFService>();
            builder.Services.AddScoped<ICNPJService, CNPJService>();
            builder.Services.AddScoped<ICEPService, CEPService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddHttpClient<ICEPService, CEPService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            var app = builder.Build();

            // Pipeline
            ConfigurePipeline(app);

            // Inicializa banco de dados
            InitializeDatabase(app);

            app.Run();
        }

        // Métodos auxiliares
        private static void ConfigurePipeline(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication(); // <-- Aqui é o local correto
            app.UseAuthorization();

            app.MapControllers();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }

        private static void InitializeDatabase(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();

                // Aplica migrações automáticas no dev
                if (app.Environment.IsDevelopment())
                {
                    context.Database.Migrate();
                }

                // Seed opcional
                // SeedData.Initialize(context);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Erro ao inicializar o banco de dados");
            }
        }

        private static void TestDatabaseConnection(string connectionString)
        {
            using var connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
                Console.WriteLine("✅ Conexão bem-sucedida com SQLEXPRESS!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erro ao conectar: {ex.Message}");
            }
        }
    }
