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
        public async Task<bool> CountPlays(string songId, string userId)
        {
            var sql0 = "UPDATE SONG_PLAY_COUNT SET COUNT = COUNT + 1 WHERE USER_ID = :userId AND SONG_ID = :songId";
            var sql1 = "INSERT INTO SONG_PLAY_COUNT (SONG_ID, USER_ID, COUNT) VALUES (:songId, :userId, 1)";

            await using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql0;
                command.Parameters.Add(new OracleParameter("userId", userId));
                command.Parameters.Add(new OracleParameter("songId", songId));
                

                await _context.Database.OpenConnectionAsync();
                var transaction = await _context.Database.BeginTransactionAsync(); // 开启事务

                try
                {
                    Console.WriteLine("Executing UPDATE query...");

                    var result0 = await command.ExecuteNonQueryAsync();
                    Console.WriteLine($"UPDATE result: {result0} rows affected.");

                    if (result0 == 0)
                    {
                        Console.WriteLine("No rows updated, executing INSERT query...");

                        var result1 = await _context.Database.ExecuteSqlRawAsync(sql1,
                            new OracleParameter("songId", songId),
                            new OracleParameter("userId", userId));

                        Console.WriteLine($"INSERT result: {result1} rows affected.");

                        await transaction.CommitAsync(); // 提交事务
                        Console.WriteLine("Transaction committed.");

                        return result1 > 0;
                    }
                    else
                    {
                        await transaction.CommitAsync(); // 提交事务
                        Console.WriteLine("Transaction committed.");
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(); // 回滚事务
                    Console.WriteLine("Transaction rolled back due to an error.");
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
                finally
                {
                    await _context.Database.CloseConnectionAsync();
                    Console.WriteLine("Database connection closed.");
                }
            }
        }





    }
}
