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
        Task<bool> IsFollowingArtistAsync(string userId, string artistId);

        //通过艺术家姓名得到所有同名艺术家相关信息
        Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name);
        

        //更新歌曲和音乐家的关系
        Task<bool> artistSingSongAsync(string SongId, string ArtistId);

        //删除歌曲和音乐家的关系
        Task<bool> DeleteartistSingSongAsync(string songId, string artistId);

        Task<bool> UnfollowArtistAsync(string userId, string artistId);
        Task<bool> IncrementArtistFansNumAsync(string artistId);
        Task<List<Artist>> GetAllArtistsAsync();
        Task<List<Artist>> GetArtistsByUserIdAsync(string userId);
        Task<int> GetArtistFansCountAsync(string artistId);

        //查询用户是否注册为歌手
        Task<bool> IsUserInArtistAsync(string userId);
        //将用户注册到歌手表中
        Task<bool> UserRegisterSinger(UserModel userInfo);
    }



}
