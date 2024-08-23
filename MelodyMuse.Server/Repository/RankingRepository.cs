using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using MelodyMuse.Server.Repository.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
namespace MelodyMuse.Server.Repository
{
   public class RankingRepository : IRankingRepository
{
    private readonly ModelContext _context;

    public RankingRepository()
    {
 _context = new ModelContext();
    }

    public async Task<List<SongRankingDto>> GetTopSongsAsync()
    {
         return await _context.Songs
            .Select(song => new SongRankingDto
            {
                SongId = song.SongId,
                SongName = song.SongName,
                Duration = song.Duration,
                ArtistName = song.Artists.Select(a => a.ArtistName).FirstOrDefault(),
                PlayCount = song.SongPlayCounts.Sum(spc => spc.Count) ?? 0
            })
            .OrderByDescending(song => song.PlayCount)
            .Take(50)
            .ToListAsync();
    }
}

}