namespace COMP72070_Section3_Group1 {
    using COMP72070_Section3_Group1.Controllers;
    class ProgramClient
    {


        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            Account accountInstance = new Account();

            builder.Services.AddSingleton<Account>(accountInstance);


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
                pattern: "{controller=Home}/{action=loginwgoogle}/{id?}");

            app.Run();
            Task task = ProgramServer.MainServer();

        }
    }
}