using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TestApp.Config;
using TestApp.Data;
using TestApp.Data.Models;
using TestApp.Data.Repositories;

namespace TestApp.Services
{
    public class InitService
    {
        public async Task<AutofacServiceProvider> Init()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            var builder = new ContainerBuilder();
            builder.Populate(serviceCollection);

            builder.RegisterType<BookRepository>().AsSelf();
            builder.RegisterType<BookService>().AsSelf();
            builder.RegisterType<DesignTimeDbContextFactory>().AsSelf();
            builder.RegisterType<MainWindow>().AsSelf();
            builder.RegisterType<AddEditBookWindow>().AsSelf();
            builder.RegisterType<InitService>().AsSelf();
            var result = builder.Build();

            return await AddTestBooksIfNeeded(result);
        }

        private void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<BookCatalogContext>(options =>
              options.UseNpgsql(DatabaseConfig.ConnectionString));

        }

        public async Task<AutofacServiceProvider> AddTestBooksIfNeeded(IContainer container)
        {
            var serviceProvider = new AutofacServiceProvider(container);
            var bookService = serviceProvider.GetRequiredService<BookService>();

            try
            {
                var exist = await bookService.GetAllBooksAsync(1, 1);
                if (exist == null || !(exist.Any()))
                {
                    for (int i = 0; i < 500; i++)
                    {
                        Book newBook = new Book
                        {
                            Title = $"Book {i + 1}",
                            Author = $"Author {i + 1}",
                            ReleaseYear = 2000 + i,
                            ISBN = $"ISBN-{i + 1}",
                            Description = $"Description for book {i + 1}",
                            Genre = $"Genre {i + 1}"
                        };

                        await bookService.AddBookAsync(newBook);
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while adding test books: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return serviceProvider;
        }
    }
}
