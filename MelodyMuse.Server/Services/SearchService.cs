/*
  搜索服务层的函数实现(调用下一层repository提供的接口)
 */

using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MelodyMuse.Server.Services
{
    // Search服务类，实现ISearchService接口
    public class SearchService : ISearchService
    {
        // 依赖的仓库接口
        private readonly ISearchRepository _searchRepository;

        // 构造函数，注入ISearchRepository实例
        public SearchService(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }
        public async Task<List<Artist>> SearchArtists(string query)
        {
            return await _searchRepository.SearchArtists(query);
        }
        public async Task<List<SongModel>> SearchSongsByName(string query)
        {
            return await _searchRepository.SearchSongsByName(query);
        }
        public async Task<List<SongSearchModel>> SearchSongsByLyrics(string query)
        {
            return await _searchRepository.SearchSongsByLyrics(query);
        }
        public async Task<List<Artist>> GetArtistsBySongId(string songId)
        {
            return await _searchRepository.GetArtistsBySongId(songId);
        }
    }
}
