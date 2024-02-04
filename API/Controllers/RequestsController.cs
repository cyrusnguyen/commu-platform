using System.Reflection.Metadata.Ecma335;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
public class RequestsController : BaseApiController
{
    private readonly IUserRepository _userRepository;
    private readonly IRequestsRepository _requestsRepository;

    public RequestsController(IUserRepository userRepository, IRequestsRepository requestsRepository)
    {
        _userRepository = userRepository;
        _requestsRepository = requestsRepository;
    }

    [HttpPost("{username}")]
    public async Task<ActionResult> AddRequest(string username)
    {
        var sourceUserId = User.GetUserId();
        var requestedUser = await _userRepository.GetUserByUsernameAsync(username);
        var sourceUser = await _requestsRepository.GetUserWithRequests(sourceUserId);

        if (requestedUser == null) return NotFound();

        if (sourceUser.UserName == username) return BadRequest("You cannot request yourself!");

        var userRequest = await _requestsRepository.GetUserRequest(sourceUserId, requestedUser.Id);

        if (userRequest != null) return BadRequest("You already requested this user");

        userRequest = new FriendRequest
        {
            SourceUserId = sourceUser.Id,
            TargetUserId = requestedUser.Id
        };

        sourceUser.RequestedUsers.Add(userRequest);

        if (await _userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Request user failed");
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<RequestDTO>>> GetUserRequests([FromQuery] RequestsParams requestsParams)
    {
        requestsParams.UserId = User.GetUserId();

        var users = await _requestsRepository.GetUserRequests(requestsParams);

        Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));
        
        return Ok(users);
    }
}
