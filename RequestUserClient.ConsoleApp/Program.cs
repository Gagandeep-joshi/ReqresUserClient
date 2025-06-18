using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ReqresUserClient.Clients;
using ReqresUserClient.Services;

class Program
{
    static async Task Main(string[] args)
    {
        // Set up DI container manually
        var services = new ServiceCollection();

        services.AddHttpClient<ReqresApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://reqres.in/api/");
            client.DefaultRequestHeaders.Add("x-api-key", "reqres-free-v1");
        });

        services.AddTransient<ExternalUserService>();

        var serviceProvider = services.BuildServiceProvider();

        var service = serviceProvider.GetRequiredService<ExternalUserService>();

        Console.WriteLine("Fetching all users from Reqres API...\n");

        var users = await service.GetAllUsersAsync();

        foreach (var user in users)
        {
            Console.WriteLine($"{user.Id}: {user.FirstName} {user.LastName} ({user.Email})");
        }

    }
}
