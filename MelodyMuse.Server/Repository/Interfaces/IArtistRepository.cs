using MelodyMuse.Server.Models;
namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IArtistRepository
    {
        Task<IEnumerable<Artist>> GetArtistsByNameAsync(string name);
    }
}
