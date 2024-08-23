using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace MelodyMuse.Server.Services
{
  public class RankingListService : IRankingListService
{
    private readonly IRankingListRepository _RankingListRepository;

    public RankingListService(IRankingListRepository RankingListRepository)
    {
        _RankingListRepository = RankingListRepository;
    }

    public async Task<List<SongPlayCountDto>> GetSongsWithPlayCountAsync()
    {
        return await _RankingListRepository.GetSongsWithPlayCountAsync();
    }
}

}
