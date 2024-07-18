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
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register services
//������ط���
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IAccountRepository>(provider =>
   new AccountRepository());
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ISMSService, SMSService>();
builder.Services.AddScoped<IVerificationCodeCacheService, VerificationCodeCacheService>();
builder.Services.AddScoped<ITencentSMSService, TencentSMSService>();
builder.Services.AddScoped<IMusicPlayerRepository>(provider =>
    new MusicPlayerRepository());

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



var app = builder.Build();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
