using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMuse.Server.Models;
using Oracle.ManagedDataAccess.Client;
using MelodyMuse.Server.Repository.Interfaces;

namespace MelodyMuse.Server.Repository
{
    public class SongRepository : ISongRepository
    {
        private readonly string _connectionString;

        public SongRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 获取待审核的歌曲
        public async Task<IEnumerable<Song>> GetPendingApprovalSongsAsync()
        {
            var songs = new List<Song>();

            using (var connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Songs WHERE Status = 0"; // 0 表示待审核
                using (var command = new OracleCommand(query, connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var song = new Song
                            {
                                SongId = reader["SongId"].ToString(),
                                SongName = reader["SongName"] as string,
                                SongGenre = reader["SongGenre"] as string,
                                Duration = reader["Duration"] as decimal?,
                                Lyrics = reader["Lyrics"] as string,
                                SongDate = reader["SongDate"] as DateTime?,
                                ComposerId = reader["ComposerId"] as string,
                                Status = reader["Status"] as byte?
                            };
                            songs.Add(song);
                        }
                    }
                }
            }

            return songs;
        }

        // 审核通过歌曲
        public async Task ApproveSongAsync(string songId)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Songs SET Status = 1 WHERE SongId = :songId"; // 1 表示审核通过
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("songId", songId));

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        // 审核不通过歌曲
        public async Task RejectSongAsync(string songId)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "UPDATE Songs SET Status = 2 WHERE SongId = :songId"; // 2 表示审核不通过
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("songId", songId));

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

