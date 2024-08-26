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

        public RecommendService(IRecommendRepository recommendRepository)
        {
            _recommendRepository = recommendRepository;
        }
        public async Task<List<SongPlayCount>> GetSongPlayCountById(string userId)
        {
            return await _recommendRepository.GetSongPlayCountById(userId);
        }
        public async Task<List<Song>> GetAllSongs()
        {
            return await _recommendRepository.GetAllSongs();
        }
    }
}
