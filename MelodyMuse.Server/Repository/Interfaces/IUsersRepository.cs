/*
   UsersRepository提供的服务接口
 */
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;


namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserModel> GetUserById(string userId);
        Task<User?> UpdateUserStatus(string userId, string newStatus);
        Task<List<User>> GetAllUsers();
        // 更新用户资料
        Task UpdateUserAsync(User user);
        Task AddUserCollectSongAsync(UserCollectSong userCollectSong);
        Task<UserCollectSong?> GetUserCollectSongAsync(string userId, string songId);
        Task RemoveUserCollectSongAsync(UserCollectSong userCollectSong); // 新增
                Task AddUserCollectAlbumAsync(UserCollectAlbum userCollectAlbum);
        Task<UserCollectAlbum?> GetUserCollectAlbumAsync(string userId, string albumId);
        Task RemoveUserCollectAlbumAsync(UserCollectAlbum userCollectAlbum);
        Task<List<Album>> GetUserCollectedAlbumsAsync(string userId);
        Task<List<Song>> GetCollectedSongsByUserId(string userId);
    }
}
