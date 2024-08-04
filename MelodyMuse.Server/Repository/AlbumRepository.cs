using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System;


namespace MelodyMuse.Server.Repository
{

    
    public class AlbumRepository : IAlbumRepository
    {
        private readonly ModelContext _context;

        public AlbumRepository()
        {
            _context = new ModelContext();
        }

       
        public async Task<bool> CreateAlbumAsync(Album album)
        {
            _context.Albums.Add(album);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<Album> GetAlbumByIdAsync(string albumId)
        {
            return await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.AlbumId == albumId);
        }
        public async Task<List<string>> GetSongsByAlbumIdAsync(string albumId)
        {
            var sql = "SELECT SONG_ID FROM SONG_MAKEUP_ALBUM WHERE ALBUM_ID = :albumId";
            var songIds = await _context.Database
                .SqlQueryRaw<string>(sql, new OracleParameter("albumId", albumId))
                .ToListAsync();

            return songIds;
        }
        public async Task<List<ArtistDto>> GetArtistsBySongIdAsync(string songId)
        {
            var sql = "SELECT a.ARTIST_ID, a.ARTIST_NAME FROM ARTIST_SING_SONG ass " +
                      "JOIN ARTIST a ON ass.ARTIST_ID = a.ARTIST_ID " +
                      "WHERE ass.SONG_ID = :songId";

            var artists = await _context.Artists
                .FromSqlRaw(sql, new OracleParameter("songId", songId))
                .Select(a => new ArtistDto
                {
                    ArtistId = a.ArtistId,
                    ArtistName = a.ArtistName
                })
                .ToListAsync();

            return artists;
        }


        public async Task<IEnumerable<AlbumDto>> GetAllAlbumsByArtistIdAsync(string artistId)
        {
            var albums = await _context.Albums
                .Where(a => a.ArtistId == artistId)
                .ToListAsync();

            var albumIds = albums.Select(a => a.AlbumId).ToList();

            var albumDtos = new List<AlbumDto>();

            // 遍历每个专辑，查询相关的歌曲和艺术家
            foreach (var album in albums)
            {
                // 查询获取与该专辑关联的歌曲ID
                var songIds = await GetSongsByAlbumIdAsync(album.AlbumId);

                // 查询与这些歌曲ID相关的歌曲
                var songs = await _context.Songs
                    .Where(s => songIds.Contains(s.SongId))
                    .ToListAsync();


                // 创建 AlbumDto 对象
                var albumDto = new AlbumDto
                {
                    AlbumId = album.AlbumId,
                    AlbumName = album.AlbumName,
                    AlbumReleasedate = album.AlbumReleasedate,
                    AlbumCompany = album.AlbumCompany,
                    AlbumProducer = album.AlbumProducer,
                    ArtistId = album.ArtistId,
                    
                    Songs = new List<SongDto>()
                };

                // 遍历每个歌曲，查询演唱者信息并添加到 SongDto 中
                foreach (var song in songs)
                {
                    var songDto = new SongDto
                    {
                        SongId = song.SongId,
                        SongName = song.SongName,
                        Duration = song.Duration,
                        Lyrics = song.Lyrics,
                        SongDate = song.SongDate,
                        SongGenre = song.SongGenre,
                        Artists = await GetArtistsBySongIdAsync(song.SongId)
                    };
                    
                    albumDto.Songs.Add(songDto);
                }

                // 将 AlbumDto 对象添加到列表中
                albumDtos.Add(albumDto);
            }


            return albumDtos;
        }

    }

}
