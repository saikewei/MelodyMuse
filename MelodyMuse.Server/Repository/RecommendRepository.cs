using MelodyMuse.Server.Configure;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services;
using Microsoft.EntityFrameworkCore;
using TencentCloud.Ame.V20190916.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MelodyMuse.Server.Repository
{
    public class RecommendRepository : IRecommendRepository
    {
        private readonly ModelContext _context;

        // 构造函数，创建新的ModelContext对象
        public RecommendRepository()
        {
            _context = new ModelContext();
        }


        //根据用户id获取播放基本信息
        public async Task<List<SongPlayCount>> GetSongPlayCountById(string userId)
        {
            var songplaycount = await _context.SongPlayCounts
                .Where(u => u.UserId == userId)
                .ToListAsync();

            if (songplaycount == null)
            {
                return null; // 返回 null 表示用户不存在
            }
            return songplaycount;
        }
        //获取所有歌曲
        public async Task<List<Song>> GetAllSongs()
        {
            // 从数据库中获取所有歌曲
            var songs = await _context.Songs
                .Where(s => s.Status == 1) // 过滤状态为1的歌曲
                .OrderBy(x => Guid.NewGuid()) // 随机排序
                .Include(s => s.Albums)
                .Take(50) // 选择前 count 条记录
                .ToListAsync();
            return songs;
            //return await _context.Songs.ToListAsync();
        }
        public async Task<List<SongModel>> RecommendSongsbyArtist(string userId)
        {
            // 获取用户播放数据
            var userPlayCounts = await GetSongPlayCountById(userId);
            var songIds = userPlayCounts.Select(pc => pc.SongId).Distinct().ToList();

            // 批量获取艺术家信息
            var songs = await _context.Songs
                .Include(s => s.Artists)
                .Include(s => s.Albums)
                .Where(s => songIds.Contains(s.SongId))
                .ToListAsync();

            var artistPlayCounts = new Dictionary<string, decimal>();

            // 计算每个艺术家的播放总数
            foreach (var song in songs)
            {
                decimal playCount = userPlayCounts.First(pc => pc.SongId == song.SongId).Count.GetValueOrDefault();
                if (playCount > 0)
                {
                    foreach (var artist in song.Artists)
                    {
                        if (artistPlayCounts.ContainsKey(artist.ArtistId))
                        {
                            artistPlayCounts[artist.ArtistId] += playCount;
                        }
                        else
                        {
                            artistPlayCounts[artist.ArtistId] = playCount;
                        }
                    }
                }
            }

            var topArtistIds = artistPlayCounts
                .OrderByDescending(apc => apc.Value)
                .Take(2)
                .Select(apc => apc.Key)
                .ToList();

            // 如果不满两个艺术家，随机补充
            var random = new Random();
            while (topArtistIds.Count < 2)
            {
                var randomArtistId = random.Next(5, 121).ToString();
                if (!topArtistIds.Contains(randomArtistId))
                {
                    topArtistIds.Add(randomArtistId);
                }
            }

            // 根据最受欢迎的艺术家ID获取其歌曲
            var recommendedSongs = songs
                .Where(song => song.Artists.Any(artist => topArtistIds.Contains(artist.ArtistId)))
                .ToList();

            // 以下为获取歌曲的歌手
            var songModelList = new List<SongModel>();

            foreach (var song in recommendedSongs)
            {
                var songModelArtists = new SongModel
                {
                    SongId = song.SongId,
                    SongName = song.SongName,
                    SongGenre = song.SongGenre,
                    Duration = song.Duration,
                    Status = song.Status,
                    AlbumId = song.Albums.FirstOrDefault()?.AlbumId,
                    Artists = song.Artists.Select(a => new Models.Artist
                    {
                        ArtistId = a.ArtistId,
                    }).ToList()
                };

                songModelList.Add(songModelArtists);
            }

            return songModelList;
        }

        public async Task<List<SongModel>> RecommendSongsById(string userId)
        {
            // 获取用户播放数据
            var userPlayCounts = await GetSongPlayCountById(userId);

            //由于卡顿，改为50个
            // 获取所有歌曲列表
            var allSongs = await GetAllSongs();

            // 提取用户已经播放过的歌曲ID列表
            var playedSongsIds = userPlayCounts.Select(spc => spc.SongId).ToList();

            // 从所有歌曲中过滤掉已经播放过的歌曲
            var playedSongs = allSongs.Where(song => playedSongsIds.Contains(song.SongId)).ToList();


            // 使用 HashSet 来存储推荐歌曲，避免重复
            var recommendedSongsSet = new HashSet<Song>();

            // 获取用户最常听的流派
            var preferredGenres = playedSongs
                .GroupBy(song => song.SongGenre)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .Take(3) // 取前3个最常听的流派
                .ToList();

            // 推荐随机歌曲中符合用户流派的歌曲
            var genreBasedSongs = allSongs
                .Where(song => preferredGenres.Contains(song.SongGenre))
                .OrderBy(x => Guid.NewGuid()) // 使用 Guid.NewGuid() 进行随机排序
                .Take(15) // 取前15首
                .ToList();

            // 将符合流派的歌曲添加到 HashSet
            foreach (var song in genreBasedSongs)
            {
                recommendedSongsSet.Add(song);
            }

            // 如果流派推荐的歌曲不足15个，则补充推荐
            if (recommendedSongsSet.Count < 15)
            {
                // 从未播放的歌曲中补充推荐（包括其他流派）
                var additionalSongs = allSongs
                    .Where(song => !recommendedSongsSet.Contains(song)) // 确保不重复
                    .OrderBy(x => Guid.NewGuid()) // 使用 Guid.NewGuid() 进行随机排序
                    .Take(15 - recommendedSongsSet.Count) // 补充到20个
                    .ToList();

                // 将补充歌曲添加到 HashSet
                foreach (var song in additionalSongs)
                {
                    recommendedSongsSet.Add(song);
                }
            }

            // 选择完全随机的歌曲
            var randomSongs = allSongs
                .Where(song => !recommendedSongsSet.Contains(song)) // 确保不重复

                .OrderBy(x => Guid.NewGuid()) // 使用 Guid.NewGuid() 进行随机排序
                .Take(5)
                .ToList();

            //合并流派推荐和完全随机推荐
            var finalRecommendedSongs = recommendedSongsSet.ToList();
            finalRecommendedSongs.AddRange(randomSongs);

            // 使用 Guid.NewGuid() 对最终推荐的歌曲列表进行随机排序
            finalRecommendedSongs = finalRecommendedSongs
                .OrderBy(x => Guid.NewGuid())
                .ToList();


            //以下为获取歌曲的歌手
            var songModelList = new List<SongModel>();

            foreach (var song in finalRecommendedSongs)
            {
                // 获取每首歌的艺术家
                var artists = await _context.Songs
                            .Where(s => s.SongId == song.SongId)
                            .SelectMany(s => s.Artists)
                            .ToListAsync();
                string? albumId = song.Albums.FirstOrDefault()?.AlbumId;

                // 创建 SongWithArtists 对象并填充数据
                var songModelArtists = new SongModel
                {
                    SongId = song.SongId,
                    SongName = song.SongName,
                    SongGenre = song.SongGenre,
                    Duration = song.Duration,
                    Lyrics = song.Lyrics,
                    SongDate = song.SongDate,
                    ComposerId = song.ComposerId,
                    Status = song.Status,
                    AlbumId = albumId,
                    Artists = artists?.Select(a => new Models.Artist
                    {
                        ArtistId = a.ArtistId,
                        ArtistName = a.ArtistName,
                        ArtistBirthday = a.ArtistBirthday,
                        ArtistIntro = a.ArtistIntro,
                        ArtistGenre = a.ArtistGenre,
                        ArtistFansNum = a.ArtistFansNum
                    }).ToList() // 如果 artists 为 null，结果也将为 null
                };

                // 将结果添加到列表中
                songModelList.Add(songModelArtists);
            }
            return songModelList;
        }
    }
}