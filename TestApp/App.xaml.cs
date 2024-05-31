// TestApp/App.xaml.cs
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using TestApp.Config;
using TestApp.Data;
using TestApp.Data.Repositories;
using TestApp.Services;

namespace TestApp
{
    public partial class App : Application
    {

        protected async override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var initService = new InitService();
            var serviceProvider =  await initService.Init();

            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
