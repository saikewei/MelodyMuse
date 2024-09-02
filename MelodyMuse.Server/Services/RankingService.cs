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

    public async Task<List<ArtistRankingDto>> GetArtistRankingAsync()
        {
            var artists = await _rankingRepository.GetArtistRankingDataAsync();

            var rankedArtists = artists
                .Select(a => new ArtistRankingDto
                {
                    ArtistId = a.ArtistId,
                    ArtistName = a.ArtistName,
                    TotalPlayCount = a.TotalPlayCount,
                    FansCount = a.FansCount,
                    RankScore = a.TotalPlayCount * 0.6m + a.FansCount * 0.4m
                })
                .OrderByDescending(a => a.RankScore)
                .Take(50)
                .ToList();

            return rankedArtists;
        }
}


}
