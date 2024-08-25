using System.Threading.Tasks;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using System.Collections.Generic;
namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IArtistRepository
    {
        Task<Artist> GetArtistByIdAsync(string artistId);
         Task<List<Song>> GetSongsByArtistIdAsync(string artistId);
         Task<bool> FollowArtistAsync(string userId, string artistId);
           Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name);
        //通过艺术家姓名得到所有同名艺术家相关信息
        Task<int> GetArtistFansCountAsync(string artistId);
        Task<bool> artistSingSongAsync(string SongId, string ArtistId);
        //更新歌曲和音乐家的关系
        Task<bool> UnfollowArtistAsync(string userId, string artistId);
        Task<bool> IncrementArtistFansNumAsync(string artistId);
         Task<List<Artist>> GetAllArtistsAsync();
         Task<List<Artist>> GetArtistsByUserIdAsync(string userId);
    }

      
    
}
