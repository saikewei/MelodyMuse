using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace MelodyMuse.Server.Repository.Interfaces
{
   public interface IRankingRepository
{
    Task<List<SongRankingDto>> GetTopSongsAsync();
    Task<List<ArtistRankingData>> GetArtistRankingDataAsync();
}

}
