using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MelodyMuse.Server.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Playlist> Playlists { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<SongPlayCount> SongPlayCounts { get; set; }

    public virtual DbSet<Songlist> Songlists { get; set; }

    public virtual DbSet<Upload> Uploads { get; set; }

    public virtual DbSet<UploadCreatorSong> UploadCreatorSongs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCollectAlbum> UserCollectAlbums { get; set; }

    public virtual DbSet<UserCollectSong> UserCollectSongs { get; set; }

    public virtual DbSet<UserCollectSonglist> UserCollectSonglists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=C##Main_Schema;Password=tongjiorcl2024;Data Source=101.126.23.58:1521/orcl");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##MAIN_SCHEMA")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("SYS_C007532");

            entity.ToTable("ALBUM");

            entity.Property(e => e.AlbumId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ALBUM_ID");
            entity.Property(e => e.AlbumCompany)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ALBUM_COMPANY");
            entity.Property(e => e.AlbumName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ALBUM_NAME");
            entity.Property(e => e.AlbumProducer)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ALBUM_PRODUCER");
            entity.Property(e => e.AlbumReleasedate)
                .HasColumnType("DATE")
                .HasColumnName("ALBUM_RELEASEDATE");
            entity.Property(e => e.ArtistId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ARTIST_ID");

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("SYS_C007533");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("SYS_C007529");

            entity.ToTable("ARTIST");

            entity.Property(e => e.ArtistId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ARTIST_ID");
            entity.Property(e => e.ArtistBirthday)
                .HasColumnType("DATE")
                .HasColumnName("ARTIST_BIRTHDAY");
            entity.Property(e => e.ArtistFansNum)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("ARTIST_FANS_NUM");
            entity.Property(e => e.ArtistGenre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ARTIST_GENRE");
            entity.Property(e => e.ArtistIntro)
                .IsUnicode(false)
                .HasColumnName("ARTIST_INTRO");
            entity.Property(e => e.ArtistName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ARTIST_NAME");
        });

        modelBuilder.Entity<Playlist>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SongId }).HasName("SYS_C007563");

            entity.ToTable("PLAYLIST");

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.SongId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SONG_ID");
            entity.Property(e => e.PlayOrder)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("PLAY_ORDER");

            entity.HasOne(d => d.Song).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007565");

            entity.HasOne(d => d.User).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007564");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("SYS_C007569");

            entity.ToTable("POST");

            entity.Property(e => e.PostId)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("POST_ID");
            entity.Property(e => e.Text)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("TEXT");
            entity.Property(e => e.Title)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("TITLE");
            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("SYS_C007570");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.HasKey(e => e.SongId).HasName("SYS_C007530");

            entity.ToTable("SONG");

            entity.Property(e => e.SongId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SONG_ID");
            entity.Property(e => e.ArtistId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ARTIST_ID");
            entity.Property(e => e.Duration)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("DURATION");
            entity.Property(e => e.Lyrics)
                .IsUnicode(false)
                .HasColumnName("LYRICS");
            entity.Property(e => e.SongDate)
                .HasColumnType("DATE")
                .HasColumnName("SONG_DATE");
            entity.Property(e => e.SongGenre)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SONG_GENRE");
            entity.Property(e => e.SongName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SONG_NAME");

            entity.HasOne(d => d.Artist).WithMany(p => p.SongsNavigation)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("SYS_C007531");

            entity.HasMany(d => d.Albums).WithMany(p => p.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "SongMakeupAlbum",
                    r => r.HasOne<Album>().WithMany()
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C007556"),
                    l => l.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C007555"),
                    j =>
                    {
                        j.HasKey("SongId", "AlbumId").HasName("SYS_C007554");
                        j.ToTable("SONG_MAKEUP_ALBUM");
                        j.IndexerProperty<string>("SongId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("SONG_ID");
                        j.IndexerProperty<string>("AlbumId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("ALBUM_ID");
                    });

            entity.HasMany(d => d.Artists).WithMany(p => p.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "ArtistSingSong",
                    r => r.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C007550"),
                    l => l.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C007549"),
                    j =>
                    {
                        j.HasKey("SongId", "ArtistId").HasName("SYS_C007548");
                        j.ToTable("ARTIST_SING_SONG");
                        j.IndexerProperty<string>("SongId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("SONG_ID");
                        j.IndexerProperty<string>("ArtistId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("ARTIST_ID");
                    });

            entity.HasMany(d => d.Songlists).WithMany(p => p.Songs)
                .UsingEntity<Dictionary<string, object>>(
                    "SongMakeupSonglist",
                    r => r.HasOne<Songlist>().WithMany()
                        .HasForeignKey("SonglistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C007559"),
                    l => l.HasOne<Song>().WithMany()
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C007558"),
                    j =>
                    {
                        j.HasKey("SongId", "SonglistId").HasName("SYS_C007557");
                        j.ToTable("SONG_MAKEUP_SONGLIST");
                        j.IndexerProperty<string>("SongId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("SONG_ID");
                        j.IndexerProperty<string>("SonglistId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("SONGLIST_ID");
                    });
        });

        modelBuilder.Entity<SongPlayCount>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SongId }).HasName("SYS_C007560");

            entity.ToTable("SONG_PLAY_COUNT");

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.SongId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SONG_ID");
            entity.Property(e => e.Count)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("COUNT");

            entity.HasOne(d => d.Song).WithMany(p => p.SongPlayCounts)
                .HasForeignKey(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007562");

            entity.HasOne(d => d.User).WithMany(p => p.SongPlayCounts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007561");
        });

        modelBuilder.Entity<Songlist>(entity =>
        {
            entity.HasKey(e => e.SonglistId).HasName("SYS_C007534");

            entity.ToTable("SONGLIST");

            entity.Property(e => e.SonglistId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SONGLIST_ID");
            entity.Property(e => e.SonglistDate)
                .HasColumnType("DATE")
                .HasColumnName("SONGLIST_DATE");
            entity.Property(e => e.SonglistIspublic)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("SONGLIST_ISPUBLIC");
            entity.Property(e => e.SonglistName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("SONGLIST_NAME");
            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Songlists)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("SYS_C007535");
        });

        modelBuilder.Entity<Upload>(entity =>
        {
            entity.HasKey(e => new { e.SongId, e.UserId }).HasName("SYS_C007571");

            entity.ToTable("UPLOAD");

            entity.Property(e => e.SongId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SONG_ID");
            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.UploadDate)
                .HasColumnType("DATE")
                .HasColumnName("UPLOAD_DATE");

            entity.HasOne(d => d.Song).WithMany(p => p.Uploads)
                .HasForeignKey(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007572");

            entity.HasOne(d => d.User).WithMany(p => p.Uploads)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007573");
        });

        modelBuilder.Entity<UploadCreatorSong>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SongId }).HasName("SYS_C007566");

            entity.ToTable("UPLOAD_CREATOR_SONG");

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.SongId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SONG_ID");
            entity.Property(e => e.UploadData)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("UPLOAD_DATA");

            entity.HasOne(d => d.Song).WithMany(p => p.UploadCreatorSongs)
                .HasForeignKey(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007568");

            entity.HasOne(d => d.User).WithMany(p => p.UploadCreatorSongs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007567");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("SYS_C007528");

            entity.ToTable("USERS");

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.UserAge)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("USER_AGE");
            entity.Property(e => e.UserBirthday)
                .HasColumnType("DATE")
                .HasColumnName("USER_BIRTHDAY");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("USER_EMAIL");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");
            entity.Property(e => e.UserPhone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("USER_PHONE");
            entity.Property(e => e.UserSex)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_SEX");
            entity.Property(e => e.UserStatus)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("USER_STATUS");

            entity.HasMany(d => d.Artists).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserFollowArtist",
                    r => r.HasOne<Artist>().WithMany()
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C007547"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("SYS_C007546"),
                    j =>
                    {
                        j.HasKey("UserId", "ArtistId").HasName("SYS_C007545");
                        j.ToTable("USER_FOLLOW_ARTIST");
                        j.IndexerProperty<string>("UserId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("USER_ID");
                        j.IndexerProperty<string>("ArtistId")
                            .HasMaxLength(10)
                            .IsUnicode(false)
                            .HasColumnName("ARTIST_ID");
                    });
        });

        modelBuilder.Entity<UserCollectAlbum>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.AlbumId }).HasName("SYS_C007536");

            entity.ToTable("USER_COLLECT_ALBUM");

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.AlbumId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("ALBUM_ID");
            entity.Property(e => e.CollectAlbumDate)
                .HasColumnType("DATE")
                .HasColumnName("COLLECT_ALBUM_DATE");

            entity.HasOne(d => d.Album).WithMany(p => p.UserCollectAlbums)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007538");

            entity.HasOne(d => d.User).WithMany(p => p.UserCollectAlbums)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007537");
        });

        modelBuilder.Entity<UserCollectSong>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SongId }).HasName("SYS_C007542");

            entity.ToTable("USER_COLLECT_SONG");

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.SongId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SONG_ID");
            entity.Property(e => e.CollectSongDate)
                .HasColumnType("DATE")
                .HasColumnName("COLLECT_SONG_DATE");

            entity.HasOne(d => d.Song).WithMany(p => p.UserCollectSongs)
                .HasForeignKey(d => d.SongId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007544");

            entity.HasOne(d => d.User).WithMany(p => p.UserCollectSongs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007543");
        });

        modelBuilder.Entity<UserCollectSonglist>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.SonglistId }).HasName("SYS_C007539");

            entity.ToTable("USER_COLLECT_SONGLIST");

            entity.Property(e => e.UserId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("USER_ID");
            entity.Property(e => e.SonglistId)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SONGLIST_ID");
            entity.Property(e => e.CollectSonglistDate)
                .HasColumnType("DATE")
                .HasColumnName("COLLECT_SONGLIST_DATE");

            entity.HasOne(d => d.Songlist).WithMany(p => p.UserCollectSonglists)
                .HasForeignKey(d => d.SonglistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007541");

            entity.HasOne(d => d.User).WithMany(p => p.UserCollectSonglists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C007540");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
