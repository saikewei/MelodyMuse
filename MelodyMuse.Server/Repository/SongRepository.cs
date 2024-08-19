using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;
using TencentCloud.Ame.V20190916.Models;

namespace MelodyMuse.Server.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly ModelContext _context;

        public SongRepository()
        {
            _context = new ModelContext();
        }

        public async Task<bool> CreateSongAsync(Song song)
        {
            _context.Songs.Add(song);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Song> GetSongByNameAndAlbumAsync(string songName, string albumId)
        {
            return await _context.Songs
                .Include(s => s.Albums)
                .FirstOrDefaultAsync(s => s.SongName == songName && s.Albums.Any(a => a.AlbumId == albumId));
        }

        public async Task<bool> songMakeupAlbumAsync(string songId, string albumId)
        {
            var sql = "INSERT INTO SONG_MAKEUP_ALBUM (SONG_ID, ALBUM_ID) VALUES (:songId, :albumId)";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, new OracleParameter("songId", songId), new OracleParameter("albumId", albumId));
            return result > 0;
        }

    }
}


/*
// 创建歌曲实体对象
var song = new Song
{
    SongId = songid,
    SongName = songUploadDto.SongName,
    Duration = songUploadDto.Duration,
    SongGenre = songUploadDto.SongGenre,
    Lyrics = songUploadDto.Lyrics,
    SongDate = album.AlbumReleasedate,
    ComposerId = album.ArtistId,
    Status = 1,//表示已发布
    Artists = artists // 设置歌曲的所有歌手
};*/