using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.models;

namespace MelodyMuse.Server.Services
{
    public class AlbumService : IAlbumService
    {
        private readonly IAlbumRepository _albumRepository;

        public AlbumService(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task<IEnumerable<AlbumDto>> GetAllAlbumsByArtistIdAsync(string artistId)
        {
            return await _albumRepository.GetAllAlbumsByArtistIdAsync(artistId);
        }
        public async Task<AlbumsDto> GetAlbumAsync(string albumId)
    {
        var album = await _albumRepository.GetAlbumAsync(albumId);

        if (album == null)
        {
            return null; // 或者抛出异常
        }

        // 映射 Album 实体到 AlbumDto
        return new AlbumsDto
        {
            AlbumId = album.AlbumId,
            AlbumName = album.AlbumName,
            AlbumReleasedate = album.AlbumReleasedate,
            AlbumCompany = album.AlbumCompany,
            AlbumProducer = album.AlbumProducer,
            ArtistId = album.ArtistId,
            Songs = album.Songs?.Select(s => new SongsDto
            {
                SongId = s.SongId,
                SongName = s.SongName,
                Duration = s.Duration
                // 添加 SongDto 中的其他属性映射
            }).ToList()
        };
    }
    }
}
