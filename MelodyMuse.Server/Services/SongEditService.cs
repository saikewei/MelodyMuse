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
            existingSong.Lyrics = songDto.Lyrics ?? existingSong.Lyrics;
            existingSong.SongDate = songDto.SongDate ?? existingSong.SongDate;

            await _songEditRepository.UpdateSongAsync(existingSong);
        }

        public async Task<IList<Song>> GetAllSongsAsync()
        {
            return await _songEditRepository.GetAllSongsAsync();
        }
    }
}
