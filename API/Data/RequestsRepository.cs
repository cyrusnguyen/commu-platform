using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class RequestsRepository : IRequestsRepository
{
    private readonly DataContext _context;

    public RequestsRepository(DataContext context)
    {
        _context = context;
    }
    /// <summary>
    /// Async func to find the FriendRequest entity that matches the primary keys in the combination of parameters
    /// </summary>
    /// <param name="sourceUserId"></param>
    /// <param name="targetUserId"></param>
    /// <returns>Return FriendRequest result</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<FriendRequest> GetUserRequest(int sourceUserId, int targetUserId)
    {
        return await _context.Requests.FindAsync(sourceUserId, targetUserId);
    }
    /// <summary>
    /// Async func to find the user requests based on the specified predicate of a specific user id
    /// </summary>
    /// <param name="predicate"></param>
    /// <param name="userId"></param>
    /// <returns>Return a list of Requests entity</returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<PagedList<RequestDTO>> GetUserRequests(RequestsParams requestsParams)
    {
        var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
        var requests = _context.Requests.AsQueryable();

        if (requestsParams.Predicate == "requested")
        {
            requests = requests.Where(request => request.SourceUserId == requestsParams.UserId);
            users = requests.Select(request => request.TargetUser);
        }

        if (requestsParams.Predicate == "requestedBy")
        {
            requests = requests.Where(request => request.TargetUserId == requestsParams.UserId);
            users = requests.Select(request => request.SourceUser);
        }

        var requestedUsers = users.Select(user => new RequestDTO
        {
            UserName = user.UserName,
            KnownAs = user.KnownAs,
            Age = user.DateOfBirth.CalculateAge(),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
            City = user.City,
            Id = user.Id
        });

        return await PagedList<RequestDTO>.CreateAsync(requestedUsers, requestsParams.PageNumber, requestsParams.PageSize);
    }

    /// <summary>
    /// Async func to get requests of a user
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>Return AppUser </returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task<AppUser> GetUserWithRequests(int userId)
    {
        return await _context.Users
                .Include(x => x.RequestedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
    }
}
