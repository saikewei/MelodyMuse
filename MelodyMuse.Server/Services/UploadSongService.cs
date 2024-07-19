using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;

namespace MelodyMuse.Server.Services
{
    public class UploadSongService : IUploadSongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;

        public UploadSongService(ISongRepository songRepository, IAlbumRepository albumRepository, IArtistRepository artistRepository)
        {
            _songRepository = songRepository;
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
        }

        public async Task<bool> UploadSongAsync(SongUploadModel songUploadDto)
        {
            // 获取专辑信息
            var album = await _albumRepository.GetAlbumByIdAsync(songUploadDto.AlbumId);
            if (album == null)
            {
                return false; // 如果专辑不存在，返回错误
            }

            // 检查是否存在相同的歌曲
            var existingSong = await _songRepository.GetSongByNameAndAlbumAsync(songUploadDto.SongName, songUploadDto.AlbumId);
            if (existingSong != null)
            {
                return false; // 如果存在相同的歌曲，返回错误
            }

            // 保存歌曲文件到Resources文件夹
            var resourcesFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Songs");
            if (!Directory.Exists(resourcesFolderPath))
            {
                Directory.CreateDirectory(resourcesFolderPath); // 如果文件夹不存在，则创建
            }

            var songFilePath = Path.Combine(resourcesFolderPath, $"{Guid.NewGuid()}{Path.GetExtension(songUploadDto.SongFile.FileName)}");
            using (var stream = new FileStream(songFilePath, FileMode.Create))
            {
                await songUploadDto.SongFile.CopyToAsync(stream);
            }

            // 获取所有歌手信息
            var artists = new List<Artist>();
            foreach (var artistId in songUploadDto.ArtistIds)
            {
                var artist = await _artistRepository.GetArtistByIdAsync(artistId);
                if (artist != null)
                {
                    artists.Add(artist);
                }
            }

            // 创建歌曲实体对象
            var song = new Song
            {
                SongId = Guid.NewGuid().ToString(),
                SongName = songUploadDto.SongName,
                Duration = songUploadDto.Duration,
                SongGenre = songUploadDto.SongGenre,
                Lyrics = songUploadDto.Lyrics,
                SongDate = album.AlbumReleasedate,
                Artists = artists // 设置歌曲的所有歌手
            };

            // 将歌曲信息保存到数据库
            return await _songRepository.CreateSongAsync(song);
        }
    }
}
