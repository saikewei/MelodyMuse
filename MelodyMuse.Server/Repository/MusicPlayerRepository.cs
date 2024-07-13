using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;

namespace MelodyMuse.Server.Repository
{
    public class MusicPlayerRepository : IMusicPlayerRepository
    {
        private readonly ModelContext _context;

        public MusicPlayerRepository()
        {
            //创建新的对象
            _context = new ModelContext();
        }

        public async Task<Song> GetSongBySongId(string songId)
        {
            //在数据库中查找是否有该歌曲ID
            return await _context.Songs.FirstOrDefaultAsync(song => song.SongId == songId);
        }
    }
}
