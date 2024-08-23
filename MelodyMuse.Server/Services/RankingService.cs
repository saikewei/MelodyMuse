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
public class RankingService : IRankingService
{
    private readonly IRankingRepository _rankingRepository;

    public RankingService(IRankingRepository rankingRepository)
    {
        _rankingRepository = rankingRepository;
    }

    public async Task<List<SongRankingDto>> GetTopSongsAsync()
    {
        return await _rankingRepository.GetTopSongsAsync();
    }
}


}
