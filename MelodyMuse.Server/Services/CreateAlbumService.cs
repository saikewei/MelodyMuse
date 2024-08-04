using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Repository.Interfaces;


namespace MelodyMuse.Server.Services
{
    public class CreateAlbumService : ICreateAlbumService
    {
        
        
            private readonly IAlbumRepository _albumRepository;


            public CreateAlbumService(IAlbumRepository albumRepository)
            {
                _albumRepository = albumRepository;
            }

            public string GenerateShortId(int length)
            {
               const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
               var random = new Random();
               return new string(Enumerable.Repeat(chars, length)
               .Select(s => s[random.Next(s.Length)]).ToArray());
            }

            public async Task<bool> CreateAlbumAsync(AlbumCreateModel albumCreateDto)
            {
                   // 生成专辑ID
                   var albumId = GenerateShortId(10);
             
                   // 指定存储专辑封面的文件夹路径
                   var albumCoverFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Covers");
                    if (!Directory.Exists(albumCoverFolderPath))
                    {
                        Directory.CreateDirectory(albumCoverFolderPath); // 如果文件夹不存在，则创建
                    }

                    // 以专辑ID命名封面文件
                    var fileExtension = Path.GetExtension(albumCreateDto.AlbumCover.FileName);
                    var albumCoverPath = Path.Combine(albumCoverFolderPath, $"{albumId}{fileExtension}");

                    // 保存专辑封面文件
                    using (var stream = new FileStream(albumCoverPath, FileMode.Create))
                    {
                        await albumCreateDto.AlbumCover.CopyToAsync(stream);
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

