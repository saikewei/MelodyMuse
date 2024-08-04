/*
 * 管理员上传音乐（包含歌曲及对应专辑）
 */

namespace MelodyMuse.Server.models
{
    public class AlbumCreateModel
    {
        public string AlbumName { get; set; }//专辑名
        public DateTime AlbumReleaseDate { get; set; }//专辑发行日期
        public string AlbumCompany { get; set; }//专辑发行公司
        public string AlbumProducer { get; set; }//专辑制作人
        public string ArtistId { get; set; }//专辑艺术家/制作人ID
        public IFormFile AlbumCover { get; set; }//专辑封面
    }

    public class SongUploadModel
    {
        public string SongName { get; set; }
        public decimal Duration { get; set; }
        public string SongGenre { get; set; }
        public string Lyrics { get; set; }
        public IFormFile SongFile { get; set; }
        public string AlbumId { get; set; }
        public List<string> ArtistIds { get; set; } // 用于存储多个歌手ID
    }

    public class AlbumDto
    {
        public string AlbumId { get; set; }
        public string AlbumName { get; set; }
        public DateTime? AlbumReleasedate { get; set; }
        public string AlbumCompany { get; set; }
        public string AlbumProducer { get; set; }
        public string ArtistId { get; set; }
        public string ArtistName { get; set; }
        public List<SongDto> Songs { get; set; }
    }
    public class SongDto
    {
        public string SongId { get; set; }
        public string SongName { get; set; }
        public decimal? Duration { get; set; }
        public string Lyrics { get; set; }
        public DateTime? SongDate { get; set; }
        public string SongGenre { get; set; }
    }
}
