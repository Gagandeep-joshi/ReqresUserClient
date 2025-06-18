using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using ReqresUserClient.Models;
using System;

namespace ReqresUserClient.Clients
{
    public class ReqresApiClient
    {
        private readonly HttpClient _httpClient;

        public ReqresApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDto> GetUserByIdAsync(int userId)
        {
            string url = $"users/{userId}";
            Console.WriteLine($"➡️ GET {url}");

            var response = await _httpClient.GetAsync(url);
            await LogIfFailed(response);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<ApiSingleUserResponse>(content);

            return result?.Data;
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = new List<UserDto>();
            int page = 1;
            int totalPages;

            do
            {
                string url = $"users?page={page}";
                Console.WriteLine($"🔵 GET {url}");

                var response = await _httpClient.GetAsync(url);

                Console.WriteLine($"🟡 Status Code: {response.StatusCode}");

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"🟢 Response Body:\n{content}");

                var result = JsonSerializer.Deserialize<ApiUserListResponse>(content);

                if (result?.Data != null)
                {
                    Console.WriteLine($"✅ Page {page} contains {result.Data.Count} users.");
                    users.AddRange(result.Data);
                }
                else
                {
                    Console.WriteLine($"⚠️ No data on page {page}.");
                }

                totalPages = result?.Total_Pages ?? 0;
                page++;
            }
            while (page <= totalPages);

            return users;
        }


        private async Task LogIfFailed(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ {response.StatusCode} - {response.ReasonPhrase}");
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("🔍 Response Body:");
                Console.WriteLine(errorContent);
            }
        }
    }
}
