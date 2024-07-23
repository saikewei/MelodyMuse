//歌曲审核功能的Repository接口和实现
using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface ISongRepository
    {
        Task<IEnumerable<Song>> GetPendingApprovalSongsAsync();
        Task ApproveSongAsync(string songId);
        Task RejectSongAsync(string songId);
    }
}
