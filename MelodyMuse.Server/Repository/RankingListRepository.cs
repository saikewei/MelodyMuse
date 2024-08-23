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
    public class RankingListRepository : IRankingListRepository
    {
        private readonly ModelContext _context;

        public RankingListRepository()
        {
            _context = new ModelContext();
        }
         public async Task<List<SongPlayCountDto>> GetSongsWithPlayCountAsync()
    {
        var songsWithPlayCount = await _context.Songs
            .Select(song => new SongPlayCountDto
            {
                SongId = song.SongId,
                SongName = song.SongName,
                SongGenre = song.SongGenre,
                Duration = song.Duration,
                Lyrics = song.Lyrics,
                SongDate = song.SongDate,
                ComposerId = song.ComposerId,
                Status = song.Status,
                TotalPlays = song.SongPlayCounts.Sum(spc => spc.Count) ?? 0
            })
            .ToListAsync();

        return songsWithPlayCount;
    }
     
    }
}