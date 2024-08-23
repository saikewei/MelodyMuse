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
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:5173")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Register services
//������ط���
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository>(provider =>
   new AccountRepository());
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



//����JWT����
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

// MusicPlayer services
//������ط���
builder.Services.AddScoped<IMusicPlayerService, MusicPlayerService>();

//SongEdit services
builder.Services.AddScoped<ISongEditService, SongEditService>();

var app = builder.Build();

// Enable CORS
app.UseCors("AllowSpecificOrigin");

app.UseDefaultFiles();
app.UseStaticFiles();
//����JWT����
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin"); // Apply CORS policy
app.UseAuthorization();

app.MapControllers();

app.Run();