using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using System;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;

namespace MelodyMuse.Server.Repository
{
    // 音乐播放器仓库类，实现IMusicPlayerRepository接口
    public class MusicPlayerRepository : IMusicPlayerRepository
    {
        private readonly ModelContext _context;

        // 构造函数，创建新的ModelContext对象
        public MusicPlayerRepository()
        {
            _context = new ModelContext();
        }

        // 根据歌曲ID获取歌曲信息
        public async Task<Song> GetSongBySongId(string songId)
        {
            // 在数据库中查找是否有该歌曲ID
            return await _context.Songs.FirstOrDefaultAsync(song => song.SongId == songId);
        }

        // 根据歌曲ID获取歌手信息
        public async Task<List<Artist>> GetSingersBySongId(string songId)
        {
            // 在数据库中查找相关歌手信息
            return await _context.Songs
                .Where(s => s.SongId == songId)
                .SelectMany(s => s.Artists)
                .ToListAsync();
        }

        //根据歌曲ID获取所属专辑ID
        public async Task<string> GetAlbumIdBySongId(string songId)
        {
            var sql = "SELECT ALBUM_ID FROM SONG_MAKEUP_ALBUM WHERE SONG_ID = :songId";
            string albumId = null;

            // 执行SQL查询并获取结果
            await using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;
                command.Parameters.Add(new OracleParameter("songId", songId));

                await _context.Database.OpenConnectionAsync();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync()) // 只读取第一条记录
                    {
                        albumId = reader.GetString(0); // 获取ALBUM_ID
                    }
                }

                await _context.Database.CloseConnectionAsync();
            }

            return albumId;
        }

    }
}
