using MelodyMuse.Server.models;

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IUploadSongService
    {
        Task<bool> UploadSongAsync(SongUploadModel songUploadDto);
    }
}
