using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IArtistRepository
    {
        Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name);
        //通过艺术家姓名得到所有同名艺术家相关信息
        Task<Artist> GetArtistByIdAsync(string artistId);
        //通过艺术家ID得到艺术家相关信息

        Task<bool> artistSingSongAsync(string SongId, string ArtistId);
        //更新歌曲和音乐家的关系
           Task<int> GetArtistFansCountAsync(string artistId);
    }
}
