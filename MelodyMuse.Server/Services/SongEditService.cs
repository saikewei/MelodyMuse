using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;

namespace MelodyMuse.Server.Services
{
    public class SongEditService : ISongEditService
    {
        private readonly ISongEditRepository _songEditRepository;

        public SongEditService(ISongEditRepository songRepository)
        {
            _songEditRepository = songRepository;
        }

        public async Task<Song> GetSongByIdAsync(string id)
        {
            return await _songEditRepository.GetSongByIdAsync(id);
        }
        public async Task UpdateSongAsync(string id, SongUpdateModel songDto)
        {
            var existingSong = await _songEditRepository.GetSongByIdAsync(id);
            if (existingSong == null)
            {
                throw new Exception("Song not found");
            }

            existingSong.SongName = songDto.SongName ?? existingSong.SongName;
            existingSong.SongGenre = songDto.SongGenre ?? existingSong.SongGenre;
            existingSong.SongDate = songDto.SongDate ?? existingSong.SongDate;
            existingSong.Lyrics = songDto.Lyrics ?? existingSong.Lyrics;

            await _songEditRepository.UpdateSongAsync(existingSong);
        }

        public async Task<IList<SongUpdateModel>> GetAllSongsAsync()
        {
            return await _songEditRepository.GetAllSongsAsync();
        }

        public async Task<Stream> GetPosterAsync(string id)
        {
            var filePath = _songEditRepository.GetPosterPath(id);
            if (File.Exists(filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return await Task.FromResult(fileStream);
            }
            return null;
        }

        public async Task SavePosterAsync(string id, IFormFile file)
        {
            var filePath = _songEditRepository.GetPosterPath(id);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
    }
}
