using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;

namespace MelodyMuse.Server.Services
{
    public class SongService : ISongService
    {
        private readonly ISongRepository _songRepository;

        public SongService(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        // 获取待审核的歌曲
        public async Task<IEnumerable<Song>> GetPendingApprovalSongsAsync()
        {
            // 调用 SongRepository 中的方法获取待审核歌曲
            return await _songRepository.GetPendingApprovalSongsAsync();
        }

        // 审核通过歌曲
        public async Task ApproveSongAsync(string songId)
        {
            // 调用 SongRepository 中的方法将歌曲状态更新为审核通过
            await _songRepository.ApproveSongAsync(songId);
        }

        // 审核不通过歌曲
        public async Task RejectSongAsync(string songId)
        {
            // 调用 SongRepository 中的方法将歌曲状态更新为审核不通过
            await _songRepository.RejectSongAsync(songId);
        }

         public async Task<List<Song>> GetSongsByComposerIdAsync(string composerId)
        {
            return await _songRepository.GetSongsByComposerIdAsync(composerId);
        }
         public async Task<string> GetAlbumBySongIdAsync(string songId)
        {
            var song = await _songRepository.GetSongByIdAsync(songId);

            if (song == null)
            {
                return "Song not found";
            }

            var album = song.Albums.FirstOrDefault();

            return album != null ? $"Album: {album.AlbumName}" : "No album associated with this song";
        }
    }
}

