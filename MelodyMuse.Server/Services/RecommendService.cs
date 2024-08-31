using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;

namespace MelodyMuse.Server.Services
{
    public class RecommendService: IRecommendService
    {
        //内部维护一个下层数据库访问服务(Repository)的接口
        private readonly IRecommendRepository _recommendRepository;
        private readonly ISongRepository _songRepository;

        public RecommendService(IRecommendRepository recommendRepository, ISongRepository songRepository)
        {
            _recommendRepository = recommendRepository;
            _songRepository = songRepository;
        }
        public async Task<List<SongPlayCount>> GetSongPlayCountById(string userId)
        {
            return await _recommendRepository.GetSongPlayCountById(userId);
        }
        public async Task<List<Song>> GetAllSongs()
        {
            return await _recommendRepository.GetAllSongs();
        }
        public async Task<List<SongModel>> RecommendSongsById(string userId)
        {
            var songlists =  await _recommendRepository.RecommendSongsById(userId);
            return songlists;
        }
        public async Task<List<SongModel>> RecommendSongsbyArtist(string userId)
        {
            return await _recommendRepository.RecommendSongsbyArtist(userId);
        }

    }
}
