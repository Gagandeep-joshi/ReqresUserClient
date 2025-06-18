
using ReqresUserClient.Models;
using ReqresUserClient.Clients;

namespace ReqresUserClient.Services
{
    public class ExternalUserService
    {
        private readonly ReqresApiClient _client;

        public ExternalUserService(ReqresApiClient client)
        {
            _client = client;
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            var dto = await _client.GetUserByIdAsync(userId);
            return MapToUser(dto);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            var dtos = await _client.GetAllUsersAsync();
            return dtos.Select(MapToUser);
        }

        private User MapToUser(UserDto dto)
        {
            var user = new User
            {
                Id = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Avatar = dto.Avatar
            };

            Console.WriteLine($"{user.Id}: {user.FirstName} {user.LastName} ({user.Email})");

            return user;
        }
    }
}
