﻿
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;
[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly IPhotoService _photoService;

    public UsersController(IUnitOfWork uow, IMapper mapper,
        IPhotoService photoService)
    {
        _uow = uow;
        _mapper = mapper;
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<MemberDTO>>> GetUsers([FromQuery] UserParams userParams)
    {
        userParams.CurrentUsername = User.GetUsername();

        var users = await _uow.UserRepository.GetMembersAsync(userParams);

        Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize,
            users.TotalCount, users.TotalPages));

        return Ok(users);
    }

    [HttpGet("{username}")] // /api/user/2
    public async Task<ActionResult<MemberDTO>> GetUser(string username)
    {
        var currentUserName = User.GetUsername();
        return await _uow.UserRepository.GetMemberAsync(username, username == currentUserName);

    }

    [HttpPut]
    public async Task<ActionResult<MemberDTO>> UpdateUser(MemberUpdateDTO memberUpdateDTO)
    {
        var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return NotFound();

        _mapper.Map(memberUpdateDTO, user);

        if (await _uow.Complete()) return NoContent();

        return BadRequest("Failed to update user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
    {
        var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return NotFound();

        var result = await _photoService.AddPhotoAsync(file);

        if (result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        user.Photos.Add(photo);

        if (await _uow.Complete())
        {
            return CreatedAtAction(nameof(GetUser),
                new { username = user.UserName }, _mapper.Map<PhotoDTO>(photo)
            );
        }

        return BadRequest("Adding photo failed");
    }

    [HttpPut("set-main-photo/{photoId}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return NotFound();

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("This is already your main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        if (await _uow.Complete()) return NoContent();
        return BadRequest("Setting main photo failed");
    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await _uow.UserRepository.GetUserByUsernameAsync(User.GetUsername());

        var photo = await _uow.PhotosRepository.GetPhotoById(photoId);

        if (photo == null) return NotFound();

        if (photo.IsMain) return BadRequest("You cannot delete your main photo");

        if (photo.PublicId != null)
        {
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (await _uow.Complete()) return Ok();

        return BadRequest("Deleting photo failed");
    }
}
