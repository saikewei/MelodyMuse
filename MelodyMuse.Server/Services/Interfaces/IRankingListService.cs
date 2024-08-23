using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IRankingListService
    {
        Task<List<SongPlayCountDto>> GetSongsWithPlayCountAsync();
    }
}
