using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Repository;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MelodyMuse.Server.Configure;
using System.Text;
using MelodyMuse.Server.OuterServices.Interfaces;
using MelodyMuse.Server.OuterServices;
using Microsoft.Extensions.Options;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://orcl.tongji.store") // 前端应用的URL
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials(); // 如果你需要发送带有凭据的请求，如Cookies等
        });
});


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAnyOrigin",
//        builder => builder.AllowAnyOrigin()
//                          .AllowAnyHeader()
//                          .AllowAnyMethod());
//});


// Register services
//������ط���
builder.Services.AddScoped<IRankingService, RankingService>();
builder.Services.AddScoped<IRankingRepository>(provider =>
   new RankingRepository());
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository>(provider => new AccountRepository());
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ISMSService, SMSService>();
builder.Services.AddScoped<IVerificationCodeCacheService, VerificationCodeCacheService>();
builder.Services.AddScoped<ITencentSMSService, TencentSMSService>();
// MusicPlayer services
//������ط���
builder.Services.AddScoped<IMusicPlayerService, MusicPlayerService>();
builder.Services.AddScoped<IMusicPlayerRepository>(provider =>
    new MusicPlayerRepository());
builder.Services.AddScoped<ISongEditRepository>(provider =>
    new SongEditRepository());
builder.Services.AddScoped<ISonglistRepository>(provider =>
    new SonglistRepository());
builder.Services.AddScoped<IStatisticRepository>(provider =>
    new StatisticRepository());

//MusicSubmit services

builder.Services.AddScoped<ICreateAlbumService, CreateAlbumService>();
builder.Services.AddScoped<IUploadSongService, UploadSongService>();

builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IArtistRepository, ArtistRepository>(provider =>
    new ArtistRepository());

builder.Services.AddScoped<IAlbumService, AlbumService>();
builder.Services.AddScoped<IAlbumRepository, AlbumRepository>(provider =>
    new AlbumRepository());

builder.Services.AddScoped<ISongRepository, SongRepository>(provider =>
    new SongRepository());

// 注册服务并提供连接字符串
builder.Services.AddScoped<ISongRepository>(provider => new SongRepository());

// 其他服务注册
builder.Services.AddScoped<ISongService, SongService>();

//用户服务
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepository>(provider =>
    new UsersRepository());

//歌曲推荐
builder.Services.AddScoped<IRecommendService, RecommendService>();
builder.Services.AddScoped<IRecommendRepository>(provider =>
    new RecommendRepository());

//艺术家相关
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IArtistRepository>(provider => new ArtistRepository());
//����JWT����
builder.Services.AddScoped<IMusicPlayerRepository>(provider => new MusicPlayerRepository());
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<ISearchRepository>(provider => new SearchRepository());

// Configure JWT authentication
var key = Encoding.ASCII.GetBytes(JWTConfigure.serect_key);
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


// 加载配置文件
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
// 注册 FtpSettings 到 DI 容器
builder.Services.Configure<FtpSettings>(builder.Configuration.GetSection("FtpSettings"));


// MusicPlayer services
builder.Services.AddScoped<IMusicPlayerService, MusicPlayerService>();

//SongEdit services
builder.Services.AddScoped<ISongEditService, SongEditService>();

//Songlist services
builder.Services.AddScoped<ISonglistService, SonglistService>();

//Statistic services
builder.Services.AddScoped<IStatisticService, StatisticService>();


var app = builder.Build();


//app.UseCors("AllowAnyOrigin");  
app.UseCors("AllowSpecificOrigin");
app.UseDefaultFiles();
app.UseStaticFiles();




// Enable JWT authentication
app.UseAuthentication();
app.UseAuthorization();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



//app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();