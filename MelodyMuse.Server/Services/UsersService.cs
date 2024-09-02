/*
   用户服务(UserSrvice)的函数实现(调用下一层repository提供的接口)
 */
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;


namespace MelodyMuse.Server.Services
{
    public class UsersService: IUsersService
    {
        //内部维护一个下层数据库访问服务(Repository)的接口
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }
        // 构造函数，注入实例
        public UsersService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        // 根据用户ID获取用户元数据
        public async Task<UserModel> GetUserById(string userId)
        {
            // 调用下层接口，从仓库层获取用户信息
            return await _usersRepository.GetUserById(userId);
        }

        //更新用户数据
        public async Task<User?> UpdateUserStatus(string userId, string newStatus)
        {
            return await _usersRepository.UpdateUserStatus(userId, newStatus);
        }

        //获取用户列表
        public async Task<List<string>> GetAllUserIds()
        {
            var users = await _usersRepository.GetAllUsers();
            return users.Select(u => u.UserId).ToList();
        }

        // 更新用户资料
        public async Task UpdateUserAsync(User user)
        {
            // 调用 IUserRepository 中的方法更新用户资料
            await _usersRepository.UpdateUserAsync(user);
        }
       public async Task AddUserCollectSongAsync(string userId, string songId)
        {

            // 检查是否已经收藏了该歌曲
            var existingCollect = await _usersRepository.GetUserCollectSongAsync(userId, songId);
            if (existingCollect != null)
            {
                throw new InvalidOperationException("歌曲已经被收藏");
            }

            // 创建 UserCollectSong 实体
            var userCollectSong = new UserCollectSong
            {
                UserId = userId,
                SongId = songId,
                CollectSongDate = DateTime.UtcNow
            };

            // 添加到仓储
            await _usersRepository.AddUserCollectSongAsync(userCollectSong);
        }
        public async Task RemoveUserCollectSongAsync(string userId, string songId)
        {
            var existingCollect = await _usersRepository.GetUserCollectSongAsync(userId, songId);

            if (existingCollect == null)
            {
                throw new ArgumentException("这首歌不在关注列表");
            }

            await _usersRepository.RemoveUserCollectSongAsync(existingCollect);
        }
         public async Task AddUserCollectAlbumAsync(string userId, string albumId)
        {

            var existingCollect = await _usersRepository.GetUserCollectAlbumAsync(userId, albumId);
            if (existingCollect != null)
            {
                throw new InvalidOperationException("专辑已经收藏过了");
            }

            var userCollectAlbum = new UserCollectAlbum
            {
                UserId = userId,
                AlbumId = albumId,
                CollectAlbumDate = DateTime.UtcNow
            };

            await _usersRepository.AddUserCollectAlbumAsync(userCollectAlbum);
        }

        public async Task RemoveUserCollectAlbumAsync(string userId, string albumId)
        {
            var existingCollect = await _usersRepository.GetUserCollectAlbumAsync(userId, albumId);

            if (existingCollect == null)
            {
                throw new ArgumentException("专辑尚未被收藏");
            }

            await _usersRepository.RemoveUserCollectAlbumAsync(existingCollect);
        }
                public async Task<List<Album>> GetUserCollectedAlbumsAsync(string userId)
        {
            return await _usersRepository.GetUserCollectedAlbumsAsync(userId);
        }
         public async Task<List<Song>> GetCollectedSongsByUserId(string userId)
        {
            return await _usersRepository.GetCollectedSongsByUserId(userId);
        }
    }
}
