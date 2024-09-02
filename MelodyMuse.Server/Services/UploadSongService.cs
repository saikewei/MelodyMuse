using FluentFTP;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Text;
using TencentCloud.Cpdp.V20190820.Models;



namespace MelodyMuse.Server.Services
{
    public class UploadSongService : IUploadSongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly IUsersRepository _userRepository;
        private readonly IMusicPlayerRepository _musicplayerRepository;
        private readonly FtpSettings _ftpSettings;
        //private readonly string _ftpServer = "101.126.23.58";
        //private readonly string _ftpUsername = "ftpuser"; 
        //private readonly string _ftpPassword = "tongjiORCL2024"; 

        public UploadSongService(ISongRepository songRepository, IAlbumRepository albumRepository, IArtistRepository artistRepository, IUsersRepository userRepository, IMusicPlayerRepository musicplayerRepository, IOptions<FtpSettings> ftpSettings)
        {
            _songRepository = songRepository;
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
            _userRepository = userRepository;
            _musicplayerRepository = musicplayerRepository;
            _ftpSettings = ftpSettings.Value; // 从配置中读取 FTP 设置
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

            //生成歌曲ID
            var songId = Guid.NewGuid().ToString().Substring(0, 10);
            
            var artists = new List<Artist>();
            var token = new CancellationToken();
            //using (var ftp = new AsyncFtpClient(_ftpServer, _ftpUsername, _ftpPassword))
            using (var ftp = new AsyncFtpClient(_ftpSettings.Server, _ftpSettings.Username, _ftpSettings.Password))
            {
                ftp.Config.DataConnectionType = FtpDataConnectionType.AutoActive;

                await ftp.Connect(token);

                foreach (var artistId in songUploadDto.ArtistIds)
                {
                    //获取相关歌手信息
                    var artist = await _artistRepository.GetArtistByIdAsync(artistId);
                    if (artist != null)
                    {
                        artists.Add(artist);

                        // 创建歌曲在FTP上的存储路径：/songs/{artistId}/{songId}/
                        var artistFolderPath = $"/songs/{artistId}";
                        var songFolderPath = $"{artistFolderPath}/{songId}";

                        if (!await ftp.DirectoryExists(artistFolderPath, token))
                        {
                            await ftp.CreateDirectory(artistFolderPath, token);
                        }

                        if (!await ftp.DirectoryExists(songFolderPath, token))
                        {
                            await ftp.CreateDirectory(songFolderPath, token);
                        }

                        // 上传歌曲文件到FTP服务器的songId文件夹中
                        var fileName = $"{songId}{Path.GetExtension(songUploadDto.SongFile.FileName)}";
                        var ftpFilePath = $"{songFolderPath}/{fileName}";

                        using (var memoryStream = new MemoryStream())
                        {
                            await songUploadDto.SongFile.CopyToAsync(memoryStream);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                            await ftp.UploadStream(memoryStream, ftpFilePath, token: token);
                        }

                        // 生成歌词文件的路径和内容
                        var lyricsFileName = $"{songId}.txt";
                        var lyricsFilePath = $"{songFolderPath}/{lyricsFileName}";

                        using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(songUploadDto.Lyrics)))
                        {
                            await ftp.UploadStream(memoryStream, lyricsFilePath, token: token);
                        }
                    }
                }

                await ftp.Disconnect(token);
            }





            // 创建歌曲实体对象
            var song = new Song
            {
                SongId = songId,
                SongName = songUploadDto.SongName,
                Duration = songUploadDto.Duration,
                SongGenre = songUploadDto.SongGenre,
                Lyrics = songUploadDto.Lyrics,
                SongDate = album.AlbumReleasedate,
                ComposerId = album.ArtistId,
                Status = 1,//表示已发布
            };

            // 将歌曲信息保存到数据库
            var createSongAsync = await _songRepository.CreateSongAsync(song);
            if (!createSongAsync)
            {
                return false;
            }

            // 更新SongMakeupAlbum关系
            var songMakeupAlbumCreated = await _songRepository.songMakeupAlbumAsync(song.SongId, album.AlbumId);
            if (!songMakeupAlbumCreated)
            {
                return false; // 如果关系保存失败，返回错误
            }

            // 更新ArtistSingSong关系
            foreach (var artist in artists)
            {
                var artistSingSongCreated = await _artistRepository.artistSingSongAsync(song.SongId, artist.ArtistId);
                if (!artistSingSongCreated)
                {
                    return false; // 如果关系保存失败，返回错误
                }
            }

            
            return true;
        }
        public async Task<bool> CreateSongAsync(SongCreateModel song)
        {
            var Song = new Song
            {
                SongId = song.SongId,
                SongName = song.SongName,
                Duration = song.Duration,
                SongGenre = song.SongGenre,
                ComposerId = song.ArtistId,
                Status = 1,//表示已发布
                Lyrics =null,
                SongDate = null,
            };

            // 将歌曲信息保存到数据库
            var createSongAsync = await _songRepository.CreateSongAsync(Song);
            if (!createSongAsync)
            {
                return false;
            }

            var artistSingSongCreated = await _artistRepository.artistSingSongAsync(song.SongId, song.ArtistId);
            if (!artistSingSongCreated)
            {
                return false; // 如果关系保存失败，返回错误
            }


            return true;
        }


        public async Task<bool> UserUploadSongAsync(SongUploadByUserModel songUploadByUserModel, string userId)
        {
            // 检查Artists表中是否存在用户信息
            if (!await _artistRepository.IsUserInArtistAsync("user"+userId))
            {
                var userInfo = await _userRepository.GetUserById(userId);
                if (userInfo != null)
                {
                    // 如果用户不存在于Artist表中，则尝试将其注册为歌手
                    if (!await _artistRepository.UserRegisterSinger(userInfo))
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            // 生成歌曲ID
            var songId = Guid.NewGuid().ToString().Substring(0, 10);

            var token = new CancellationToken();
            //using (var ftp = new AsyncFtpClient(_ftpServer, _ftpUsername, _ftpPassword))
            using (var ftp = new AsyncFtpClient(_ftpSettings.Server, _ftpSettings.Username, _ftpSettings.Password))
            {
                ftp.Config.DataConnectionType = FtpDataConnectionType.AutoActive;

                await ftp.Connect(token);
                //获取相关歌手信息
                var artist = await _artistRepository.GetArtistByIdAsync("user"+userId);
                
                if (artist != null)
                {

                    var artistId = "user" + userId;
                    // 创建歌曲在FTP上的存储路径：/songs/{artistId}/{songId}/
                    var artistFolderPath = $"/songs/{artistId}";
                    var songFolderPath = $"{artistFolderPath}/{songId}";

                    if (!await ftp.DirectoryExists(artistFolderPath, token))
                    {
                        await ftp.CreateDirectory(artistFolderPath, token);
                    }

                    if (!await ftp.DirectoryExists(songFolderPath, token))
                    {
                        await ftp.CreateDirectory(songFolderPath, token);
                    }

                    // 上传歌曲文件到FTP服务器的songId文件夹中
                    var fileName = $"{songId}{Path.GetExtension(songUploadByUserModel.SongFile.FileName)}";
                    var ftpFilePath = $"{songFolderPath}/{fileName}";

                    using (var memoryStream = new MemoryStream())
                    {
                        await songUploadByUserModel.SongFile.CopyToAsync(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        await ftp.UploadStream(memoryStream, ftpFilePath, token: token);
                    }

                    // 生成歌词文件的路径和内容
                    var lyricsFileName = $"{songId}.txt";
                    var lyricsFilePath = $"{songFolderPath}/{lyricsFileName}";

                    using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(songUploadByUserModel.Lyrics)))
                    {
                        await ftp.UploadStream(memoryStream, lyricsFilePath, token: token);
                    }
                }



                await ftp.Disconnect(token);
            }

            // 创建歌曲实体对象
            var song = new Song
            {
                SongId = songId,
                SongName = songUploadByUserModel.SongName,
                Duration = songUploadByUserModel.Duration,
                SongGenre = songUploadByUserModel.SongGenre,
                Lyrics = songUploadByUserModel.Lyrics,
                SongDate = DateTime.Now,
                ComposerId = "user"+userId,
                Status = 0, // 表示未发布，需要管理员审核
            };

            // 将歌曲信息保存到数据库
            if (!await _songRepository.CreateSongAsync(song))
            {
                return false;
            }

            // 保存歌手和歌曲的关联信息
            if (!await _artistRepository.artistSingSongAsync(song.SongId, "user" + userId))
            {
                return false;
            }

            //更新Upload表
            var upload = new Upload
            {
                SongId = songId,
                UserId = userId,
                UploadDate = DateTime.Now
            };
            
            if (!await _userRepository.UserUpload(upload))
            {
                return false;
            }

            return true;
        }
        public async Task<bool> UserDeleteSongAsync(string userId,string songId)
        {
            
            //检查是否存在这首歌
            var existingSong = await _songRepository.GetSongByIdAsync(songId);
            if (existingSong == null)
            {
                throw new Exception("Song not found"); 
            }

            //删除Upload里的记录
            if(! await _userRepository.DeleteUploadRecord(userId, songId))
            {
                
                throw new Exception("删除Upload记录失败");
            }

            //删除User_Collect_Song表里的记录
            if (!await _userRepository.DeleteCollectSongRecord(songId))
            {
                
                throw new Exception("删除User_Collect_Song记录失败");
            }

            //删除Artist_Sing_Song表里的记录
            if (!await _artistRepository.DeleteartistSingSongAsync(songId,"user"+ userId))
            {
                throw new Exception("删除Song_Makeup_SongList记录失败");
            }

            
            //删除Song_Makeup_SongList里的记录
            if(!await _songRepository.DeleteMakeupSongListRecord(songId))
            {
                throw new Exception("删除Song_Makeup_SongList记录失败");
            }

            //删除Song_Play_Count里的记录
            if (!await _musicplayerRepository.DeleteCountPlaysRecord(songId))
            {
                throw new Exception("删除Song_Play_Count记录失败");
            }
            
            //删除Song里的记录
            if(!await _songRepository.DeleteSongRecord(songId))
            {
                throw new Exception("删除Song记录失败");
            }

            //删除ftp服务器上的相关文件
            var token = new CancellationToken();
            //using (var ftp = new AsyncFtpClient(_ftpServer, _ftpUsername, _ftpPassword))
            using (var ftp = new AsyncFtpClient(_ftpSettings.Server, _ftpSettings.Username, _ftpSettings.Password))
            {
                ftp.Config.DataConnectionType = FtpDataConnectionType.AutoActive;

                try
                {
                    // 连接到 FTP 服务器
                    await ftp.Connect(token);

                    // 获取艺术家信息
                    var artist = await _artistRepository.GetArtistByIdAsync("user" + userId);

                    if (artist != null)
                    {
                        var artistId = "user" + userId;
                        // 创建歌曲在 FTP 上的存储路径：/songs/{artistId}/{songId}/
                        var artistFolderPath = $"/songs/{artistId}";
                        var songFolderPath = $"{artistFolderPath}/{songId}";

                        // 检查歌曲文件夹是否存在
                        if (await ftp.DirectoryExists(songFolderPath, token))
                        {
                            // 删除歌曲文件夹及其内容
                            await ftp.DeleteDirectory(songFolderPath, token);
                            Console.WriteLine($"歌曲文件夹 {songFolderPath} 已成功删除。");
                        }
                        else
                        {
                            Console.WriteLine($"歌曲文件夹 {songFolderPath} 不存在。");
                        }
                    }
                    else
                    {
                        Console.WriteLine("未找到对应的艺术家。");
                    }
                }
                catch (Exception ex)
                {
                    // 处理连接或删除过程中的异常
                    Console.WriteLine($"发生错误: {ex.Message}");
                }
                finally
                {
                    // 确保在结束时断开与 FTP 服务器的连接
                    await ftp.Disconnect(token);
                }
            }
            return true;
        }
    }

}


