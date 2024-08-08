using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Artist = MelodyMuse.Server.Models.Artist;

namespace MelodyMuse.Server.Repository
{
    public class SearchRepository: ISearchRepository
    {
        private readonly ModelContext _context;

        // 构造函数，创建新的ModelContext对象
        public SearchRepository()
        {
            _context = new ModelContext();
        }

        //查询歌手
        public async Task<List<Artist>> SearchArtists(string query)
        { 
                return await _context.Artists
                                .Where(a => a.ArtistName.Contains(query))
                                .ToListAsync();
        }

        //查询歌名
        public async Task<List<Song>> SearchSongsByName(string query)
        {
            return await _context.Songs
                       .Where(a => a.SongName.Contains(query))
                       .ToListAsync();

        }
        //查询歌词
        public async Task<List<SongSearchModel>> SearchSongsByLyrics(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<SongSearchModel>();
            }

            var songs = await _context.Songs
                .Where(s => s.Lyrics.Contains(query))
                .ToListAsync();

            var result = new List<SongSearchModel>();

            foreach (var song in songs)
            {
                var matchedLyricsLines = GetMatchedLyricsLines(song.Lyrics, query);

                var existingSong = result.FirstOrDefault(s => s.SongId == song.SongId);
                if (existingSong != null)
                {
                    existingSong.MatchedLyricsLines.AddRange(matchedLyricsLines);
                }
                else
                {
                    result.Add(new SongSearchModel
                    {
                        SongId = song.SongId,
                        SongName = song.SongName,
                        MatchedLyricsLines = matchedLyricsLines
                    });
                }
            }

            return result;
        }
        private List<MatchedLyricsLine> GetMatchedLyricsLines(string lyrics, string query)
        {
            // 正则表达式匹配所有时间戳及其内容
            var regex = new Regex(@"\[\d+(:\d+)?(\.\d+)?\](.*?)\n", RegexOptions.Singleline | RegexOptions.Compiled);

            // 提取所有时间戳之间的内容
            var matches = regex.Matches(lyrics);
            var matchedLines = new List<MatchedLyricsLine>();

            int lineNumber = 0;

            foreach (Match match in matches)
            {
                var lineContent = match.Groups[3].Value.Trim(); // 时间戳后的内容
                lineNumber++;

                // 处理每行，移除时间戳后的文本并查找匹配
                if (lineContent.Contains(query, StringComparison.OrdinalIgnoreCase))
                {
                    matchedLines.Add(new MatchedLyricsLine
                    {
                        LineNumber = lineNumber,
                        LyricsLine = lineContent
                    });
                }
            }

            return matchedLines;
        }
    }
}
