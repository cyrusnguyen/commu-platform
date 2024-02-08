using API.DTOs;
using API.Entities;

namespace API.Interfaces;
public interface IPhotoRepository
{
    Task<IEnumerable<PhotoApprovalDTO>> GetUnapprovedPhotos();
    Task<Photo> GetPhotoById(int photoId);
    void RemovePhoto(Photo photo);

}
