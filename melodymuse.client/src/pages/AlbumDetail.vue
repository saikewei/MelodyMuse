<template>
    <div>
    <TheHeader />
    <div class="page-container">
      <div class="profile">
        <div class="header">
          <img class="album-cover" :src="albumCover" alt="Album Cover" />
          <div class="album-info">
            <h1>{{ album.albumName }}</h1>
            <p style="color:darkgray;font-size: small;">
              发行日期：{{ formattedReleaseDate }} <br />
              发行公司：{{ album.albumCompany }} <br />
              制作人：{{ album.albumProducer }}
            </p>
            <div class="stats">
              <span>歌曲 {{ songCount }}</span>
            </div>
            <button class="play-button" @click="playFirstSong">▷播放专辑</button>
          </div>
        </div>
  
        <div class="songs">
          <h2 style="margin-top: 10px; margin-bottom: 5px; margin-left: 10px; color:#284da0c1;">专辑歌曲</h2>
          <div class="songs-table">
            <table>
              <thead>
                <tr>
                  <th>歌曲</th>
                  <th>歌手</th>
                  <th>时长</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(song, index) in album.songs" :key="index" @click="playSong(song.songId)">
                  <td>{{ index + 1 }}. {{ song.songName }}</td>
                  <td>{{ song.artistName }}</td>
                  <td>{{ formatDuration(song.duration) }}</td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
    <TheFooter />
    </div>
  </template>
  
  <script>
  import albumCover from '../assets/logo2.jpg';
  import TheFooter from "../components/TheFooter.vue";
  import TheHeader from '../components/TheHeader.vue';
  import api from '../api/http.js';
  
  export default {
    data() {
      return {
        albumCover,
        userId: '',
        albumId: '',
        album: {
          albumName: '杜宣达',
          albumReleasedate: '2002-09-13T03:47:21',
          albumCompany: '蓝天乐律',
          albumProducer: '杜宣达',
          songs: [
          {
            "songId": "0c35751f-0",
            "songName": "缓缓",
            "duration": 236,
            "songDate": null,
            "songGenre": null
        },
        {
            "songId": "1b5ed95e-d",
            "songName": "天若有情",
            "duration": 205,
            "songDate": null,
            "songGenre": null
        },
        ],
        },
      };
    },
    computed: {
      songCount() {
        return this.album.songs.length;
      },
      formattedReleaseDate() {
        const releaseDate = new Date(this.album.albumReleasedate);
        return releaseDate.toLocaleDateString();
      }
    },
    components: {
      TheHeader,
      TheFooter,
    },
    methods: {
      async fetchAlbumData() {
        try {
          const albumResponse = await api.apiClient.get(`/api/album/${this.albumId}`);
          this.album = albumResponse.data;
          await this.fetchArtistName();
        } catch (error) {
          console.error('获取专辑信息失败:', error);
        }
      },
      async fetchArtistName() {
        try {
          const artistResponse = await api.apiClient.get(`/api/artist/${this.album.artistId}`);
          const artistName = artistResponse.data.artistName;
          this.album.songs.forEach(song => {
            song.artistName = artistName;
          });
        } catch (error) {
          console.error('获取歌手信息失败:', error);
        }
      },
      playFirstSong() {
        const firstSongId = this.album.songs[0].songId;
        this.$router.push({ name: 'PlayerPage', params: { songId: firstSongId } });
      },
      playSong(songId) {
        this.$router.push({ name: 'PlayerPage', params: { songId: songId } });
      },
      formatDuration(duration) {
        const minutes = Math.floor(duration / 60);
        const seconds = duration % 60;
        return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
      }
    },
    async created() {
      this.albumId = this.$route.params.albumId;
      if (this.albumId) {
        await this.fetchAlbumData();
      } else {
        console.error('未找到 albumId');
      }
      this.userId = '001'; // this.$route.params.userId
    }
  };
  </script>
  
  <style scoped>
  .page-container {
    background: linear-gradient(to top, #f0f0f5, #d8dff8);
    min-height: 94vh;
    display: flex;
    justify-content: center;
    align-items: center;
    padding: 20px;
  }
  
  .profile {
    width: 90%;
    max-width: 1200px;
    max-height:680px;
    margin: 20px auto;
    background-color: #fff;
    border: 1px solid #ddd;
    box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1);
  }
  
  .header {
    display: flex;
    align-items: center;
    padding: 20px;
  }
  
  .album-cover {
    width: 150px;
    height: 150px;
    border-radius: 8px;
    margin-right: 30px;
  }
  
  .album-info {
    flex: 1;
  }
  
  .stats {
    display: flex;
    gap: 10px;
    margin-top: 10px;
  }
  
  .play-button {
    margin-top: 20px;
    padding: 10px 20px;
    border: none;
    cursor: pointer;
    background-color: #284da0c1;
    color: white;
    border-radius: 8px;
    margin-right: 2px;
  }
  
  .songs {
    margin-top: 20px;
    padding: 20px;
    border-top: 1px solid #ddd;

  }
  
  .songs-table {
    max-height: 320px;
    overflow-y: auto;
  }
  
  table {
    width: 100%;
    border-collapse: collapse;
  }
  
  thead {
    background-color: #f4f4f4;
  }
  
  th, td {
    padding: 10px;
    text-align: left;
    border-bottom: 1px solid #ddd;
  }
  </style>
  