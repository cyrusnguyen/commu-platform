using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces;
public interface IRequestsRepository
{
    Task<FriendRequest> GetUserRequest(int sourceUserId, int targetUserId);
    Task<AppUser> GetUserWithRequests(int userId);
    Task<PagedList<RequestDTO>> GetUserRequests(RequestsParams requestsParams);
}
