using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;


namespace MelodyMuse.Server.Repository
{
    public class UserFollowArtist
    {
        public string UserId { get; set; }
        public string ArtistId { get; set; }
    }

    public class ArtistRepository : IArtistRepository
    {
        private readonly ModelContext _context;

        public ArtistRepository()
        {
            _context = new ModelContext();
        }

        public async Task<Artist> GetArtistByIdAsync(string artistId)
        {
            return await _context.Artists.FirstOrDefaultAsync(a => a.ArtistId == artistId);
        }

        public async Task<List<Song>> GetSongsByArtistIdAsync(string artistId)
        {
            // Assuming there is a many-to-many relationship between Songs and Artists
            return await _context.Songs
                .Where(song => song.Artists.Any(artist => artist.ArtistId == artistId))
                .ToListAsync();
        }
        public async Task<bool> FollowArtistAsync(string userId, string artistId)
        {
            var sql = "INSERT INTO USER_FOLLOW_ARTIST (USER_ID, ARTIST_ID) VALUES (:userId, :artistId)";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, new OracleParameter("userId", userId), new OracleParameter("artistId", artistId));
            return result > 0;

        }

        public async Task<bool> IsFollowingArtistAsync(string userId, string artistId)
        {
            var sql = "SELECT COUNT(*) FROM USER_FOLLOW_ARTIST WHERE USER_ID = :userId AND ARTIST_ID = :artistId";

            // 获取数据库连接
            var connection = _context.Database.GetDbConnection();

            try
            {
                // 确保连接在使用前处于打开状态
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                // 创建并配置 SQL 命令
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    command.Parameters.Add(new OracleParameter("userId", userId));
                    command.Parameters.Add(new OracleParameter("artistId", artistId));

                    // 执行命令并获取结果
                    var result = await command.ExecuteScalarAsync();
                    var count = Convert.ToInt32(result);

                    // 返回是否存在记录
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                // 处理异常
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        public async Task<bool> UnfollowArtistAsync(string userId, string artistId)
        {
            var sql = "DELETE FROM USER_FOLLOW_ARTIST WHERE USER_ID = :userId AND ARTIST_ID = :artistId";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, new OracleParameter("userId", userId), new OracleParameter("artistId", artistId));
            return result > 0;
        }

        public async Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name)
        {
            return await _context.Artists
                .Where(a => a.ArtistName.Contains(name))
                .ToListAsync();
        }



        public async Task<bool> artistSingSongAsync(string songId, string artistId)
        {
            var sql = "INSERT INTO ARTIST_SING_SONG (SONG_ID, ARTIST_ID) VALUES (:songId, :artistId)";
            var result = await _context.Database.ExecuteSqlRawAsync(sql, new OracleParameter("songId", songId), new OracleParameter("artistId", artistId));
            return result > 0;
        }

        public async Task<bool> DeleteartistSingSongAsync(string songId, string artistId)
        {
            // SQL 语句用于检查记录是否存在
            var checkSql = "SELECT 1 FROM ARTIST_SING_SONG WHERE SONG_ID = :songId AND ARTIST_ID = :artistId";

            // SQL 语句用于删除记录
            var deleteSql = "DELETE FROM ARTIST_SING_SONG WHERE SONG_ID = :songId AND ARTIST_ID = :artistId";

            // 创建参数
            var parameters = new[]
            {
                 new OracleParameter("songId", songId),
                 new OracleParameter("artistId", artistId)
            };

            try
            {
                var connection = (OracleConnection)_context.Database.GetDbConnection();

                // 确保连接在使用前处于打开状态
                if (connection.State != System.Data.ConnectionState.Open)
                {
                    await connection.OpenAsync();
                }

                // 检查是否存在该记录
                using (var checkCommand = connection.CreateCommand())
                {
                    checkCommand.CommandText = checkSql;
                    checkCommand.Parameters.AddRange(parameters);

                    var exists = (await checkCommand.ExecuteScalarAsync()) != null;

                    // 如果不存在记录，返回 true，因为没有要删除的内容
                    if (!exists)
                    {
                        return true;
                    }
                }

                // 如果存在记录，则执行删除操作
                using (var deleteCommand = connection.CreateCommand())
                {
                    deleteCommand.CommandText = deleteSql;
                    deleteCommand.Parameters.AddRange(parameters);

                    var result = await deleteCommand.ExecuteNonQueryAsync();

                    // 返回删除结果，是否有记录被删除
                    return result > 0;
                }
            }
            catch (Exception ex)
            {
                // 记录异常日志或处理异常
                Console.WriteLine(ex.Message);
                return false;
            }
        }



        public async Task<bool> IncrementArtistFansNumAsync(string artistId)
        {
            try
            {
                var artist = await _context.Artists.FirstOrDefaultAsync(a => a.ArtistId == artistId);
                if (artist != null)
                {
                    artist.ArtistFansNum = (artist.ArtistFansNum ?? 0) + 1;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Artist>> GetAllArtistsAsync()
        {
            return await _context.Artists.ToListAsync();
        }

        public async Task<List<Artist>> GetArtistsByUserIdAsync(string userId)
        {
            return await _context.Users
                .Where(u => u.UserId == userId)
                .SelectMany(u => u.Artists)
                .ToListAsync();
        }

        public async Task<int> GetArtistFansCountAsync(string artistId)
        {
            // 使用艺术家和用户之间的关系表来获取粉丝数
            var artist = await _context.Artists
                .Include(a => a.Users)
                .FirstOrDefaultAsync(a => a.ArtistId == artistId);

            return artist?.Users.Count ?? 0;
        }
        public async Task<bool> IsUserInArtistAsync(string userId)
        {
            var artist = await _context.Artists
                .FirstOrDefaultAsync(a => a.ArtistId == userId);

            // 返回布尔值，指示是否找到相应的记录
            return artist != null;
        }
        public async Task<bool> UserRegisterSinger(UserModel userInfo)
        {
            var artist = new Artist
            {
                ArtistId = "user"+userInfo.UserId,
                UserId = userInfo.UserId,
                ArtistName = userInfo.UserName,
                ArtistBirthday = userInfo.UserBirthday,
            };
            _context.Artists.Add(artist);
            return await _context.SaveChangesAsync() > 0;
        }

        


    }
}
