﻿using MelodyMuse.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ISonglistService
{
    Task<IEnumerable<(string SonglistId, string SonglistName, int SongCount)>> GetUserSonglistsAsync(string userId);
    Task<IEnumerable<Song>> GetSongsInSonglistAsync(string songlistId);
}
