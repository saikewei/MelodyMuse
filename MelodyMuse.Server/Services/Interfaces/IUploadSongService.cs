using MelodyMuse.Server.models;

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IUploadSongService
    {
        Task<bool> UploadSongAsync(SongUploadModel songUploadDto);
        Task<bool> CreateSongAsync(SongCreateModel song);//批量上传歌曲信息
    }
}
