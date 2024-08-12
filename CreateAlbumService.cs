using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Repository.Interfaces;
using FluentFTP;
using MelodyMuse.Server.Repository;
using System.Text;


namespace MelodyMuse.Server.Services
{
    public class CreateAlbumService : ICreateAlbumService
    {
        
        
            private readonly IAlbumRepository _albumRepository;
            private readonly string _ftpServer = "101.126.23.58";
            private readonly string _ftpUsername = "ftpuser";
            private readonly string _ftpPassword = "tongjiORCL2024";


        public CreateAlbumService(IAlbumRepository albumRepository)
        {
                _albumRepository = albumRepository;
        }

        public async Task<bool> CreateAlbumAsync(AlbumCreateModel albumCreateDto)
        {
            // 生成专辑ID
            var albumId = Guid.NewGuid().ToString().Substring(0, 10);

            // 指定FTP专辑封面的文件夹路径
            var coverFolderPath = $"/albumCover/{albumId}";

            // 以专辑ID命名封面文件
            var fileExtension = Path.GetExtension(albumCreateDto.AlbumCover.FileName);
            var albumCoverFileName = $"{albumId}{fileExtension}";

            using (var ftp = new AsyncFtpClient(_ftpServer, _ftpUsername, _ftpPassword))
            {
                var token = new CancellationToken();
                await ftp.Connect(token);

                if (!await ftp.DirectoryExists(coverFolderPath, token))
                {
                    await ftp.CreateDirectory(coverFolderPath, token);
                }

                // 上传封面文件到FTP服务器的albumCover文件夹中
                using (var memoryStream = new MemoryStream())
                {
                    await albumCreateDto.AlbumCover.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    var ftpFilePath = $"{coverFolderPath}/{albumCoverFileName}";
                    await ftp.UploadStream(memoryStream, ftpFilePath, token: token);
                }

                await ftp.Disconnect(token);
            }


            // 创建专辑实体对象
            var album = new Album
            {
                AlbumId = albumId,
                AlbumName = albumCreateDto.AlbumName,
                AlbumReleasedate = albumCreateDto.AlbumReleaseDate,
                AlbumCompany = albumCreateDto.AlbumCompany,
                AlbumProducer = albumCreateDto.AlbumProducer,
                ArtistId = albumCreateDto.ArtistId, // 使用AlbumCreateDto中的ArtistId

            };


            // 将专辑信息保存到数据库
            bool result = await _albumRepository.CreateAlbumAsync(album);
            if (!result)
            {
                throw new Exception("Failed to save album to the database.");
            }
            return result;


        }
    }



}

