using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IRankingListRepository
    {
        Task<List<SongPlayCountDto>> GetSongsWithPlayCountAsync();
    }
}
