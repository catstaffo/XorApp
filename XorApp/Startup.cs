using ElectronNET.API;
using ElectronNET.API.Entities;
using XorApp.Services;

namespace XorApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<XorService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Xor}/{action=Index}/{id?}");
            });

            if (HybridSupport.IsElectronActive)
            {
                ElectronBootstrap();
            }
        }
        
        public async void ElectronBootstrap()
        {
            //NotificationTest();
            var browserWindow = await Electron.WindowManager.CreateWindowAsync(new BrowserWindowOptions
            {
                Width = 600,
                Height = 900,
                Show = false,
                Resizable = true
            });

            await browserWindow.WebContents.Session.ClearCacheAsync();

            browserWindow.OnReadyToShow += () => browserWindow.Show();
            browserWindow.SetTitle(Configuration["AppTitle"]);
            
            // TODO
            browserWindow.OnMove += UpdateView;
            browserWindow.OnResize += UpdateView;
            
            await Electron.IpcMain.On("request-notification-permission", async (args) =>
            {
                await browserWindow
                    .WebContents
                    .ExecuteJavaScriptAsync($"require('./js/notification-permissions').requestNotificationPermission()");
            });

            Electron.IpcMain.Send(browserWindow, "request-notification-permission");
        }

        private async void UpdateView()
        {
            var browserWindow = Electron.WindowManager.BrowserWindows.Last();
            var size = await browserWindow.GetSizeAsync();
        }

        private void NotificationTest()
        {
            Electron.App.On("activate", (obj) =>
            {
                var hasWindows = (bool) obj;

                Electron.Notification.Show(
                    new NotificationOptions("~Activated~", $"Application activated action. Active windows = {hasWindows}")
                    {
                        Silent = false,
                    });
            });
        }
    }
}
