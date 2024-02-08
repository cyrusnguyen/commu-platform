using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class PhotosRepository : IPhotoRepository
{
    private readonly DataContext _context;

    public PhotosRepository(DataContext _context)
    {
        this._context = _context;
    }
    public async Task<Photo> GetPhotoById(int photoId)
    {
        return await _context.Photos
            .IgnoreQueryFilters()
            .SingleOrDefaultAsync(p => p.Id == photoId);
    }

    public async Task<IEnumerable<PhotoApprovalDTO>> GetUnapprovedPhotos()
    {
        return await _context.Photos
            .IgnoreQueryFilters()
            .Where(p => p.IsApproved == false)
            .Select(u => new PhotoApprovalDTO
            {
                Id = u.Id,
                Username = u.AppUser.UserName,
                Url = u.Url,
                IsApproved = u.IsApproved
            }).ToListAsync();
    }

    public void RemovePhoto(Photo photo)
    {
        _context.Photos.Remove(photo);
    }
}
