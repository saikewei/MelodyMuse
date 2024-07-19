using MelodyMuse.Server.Models;
namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IArtistService
    {
        Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name);
    }
}
