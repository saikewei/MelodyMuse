using FluentFTP;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;
using System.Text;

namespace MelodyMuse.Server.Services
{
    public class UploadSongService : IUploadSongService
    {
        private readonly ISongRepository _songRepository;
        private readonly IAlbumRepository _albumRepository;
        private readonly IArtistRepository _artistRepository;
        private readonly string _ftpServer = "101.126.23.58";
        private readonly string _ftpUsername = "ftpuser"; 
        private readonly string _ftpPassword = "tongjiORCL2024"; 

        public UploadSongService(ISongRepository songRepository, IAlbumRepository albumRepository, IArtistRepository artistRepository)
        {
            _songRepository = songRepository;
            _albumRepository = albumRepository;
            _artistRepository = artistRepository;
            
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
            using (var ftp = new AsyncFtpClient(_ftpServer, _ftpUsername, _ftpPassword))
            {
                var token = new CancellationToken();
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
    }
}



