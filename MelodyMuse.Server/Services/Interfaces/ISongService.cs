using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface ISongService
    {
        // 获取待审核的歌曲
        Task<IEnumerable<Song>> GetPendingApprovalSongsAsync();

        // 审核通过歌曲
        Task ApproveSongAsync(string songId);

        // 审核不通过歌曲
        Task RejectSongAsync(string songId);
    }
}
