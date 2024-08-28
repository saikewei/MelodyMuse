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
            <button 
            class="like-button" 
            :class="{ liked: isLiked }" 
            @click="toggleLike">
            {{ isLiked ? '已收藏' : '♡ 收藏' }} 
          </button>
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
                  <!-- 添加按钮 -->
                  <th>操作</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="(song, index) in album.songs" :key="index" @click="playSong(song.songId)">

                  <td>
                    <!-- 添加播放按钮 -->
                    <el-tooltip content="播放歌曲" placement="bottom">
                      <img :src="song.playing ? playClickedIcon : song.playHover ? playHoverIcon : playIcon"
                           @mouseover="song.playHover = true"
                           @mouseleave="song.playHover = false"
                           @click="togglePlayIcon(song)"
                           class="play-icon"
                           alt="播放歌曲" />
                    </el-tooltip>
                    {{ index + 1 }}. {{ song.songName }}</td>
               <!-- togglePlayIcon(song)换成playSong(song.songId) -->

                  <td>{{ song.artistName }}</td>
                  <td>{{ formatDuration(song.duration) }}</td>

                  <!-- 添加收藏按钮 -->
                  <td>
                    <el-tooltip content="收藏歌曲" placement="bottom">
                      <img :src="song.liked ? likeClickedIcon : song.likeHover ? likeHoverIcon : likeIcon"
                           @mouseover="song.likeHover = true"
                           @mouseleave="song.likeHover = false"
                           @click="toggleLikeIcon(song)"
                           class="like-icon"
                           alt="收藏歌曲" />
                    </el-tooltip>
                  </td>
             </tr>
           </tbody>
         </table>
       </div>
     </div>
   </div>
 </div>
      <TheFooter  />
 </div>
</template>
  
  <script>
  import albumCover from '../assets/logo2.jpg';
  import TheFooter from "../components/TheFooter.vue";
  import TheHeader from '../components/TheHeader.vue';
  import api from '../api/http.js';
  import playIcon from '../assets/pics/play.png'; // 添加按钮图片路径↓
  import playClickedIcon from '../assets/pics/play-click.png'; 
  import playHoverIcon from '../assets/pics/play-cover.png'; 
  import likeIcon from '../assets/pics/like.png'; 
  import likeHoverIcon from '../assets/pics/like-cover.png'; 
  import likeClickedIcon from '../assets/pics/like-click.png'; 
  import addIcon from '../assets/pics/add.png'; 
  import addHoverIcon from '../assets/pics/add-cover.png'; 
  import addClickedIcon from '../assets/pics/add-click.png'; // 添加↑
  
  export default {
    data() {
      return {
        albumCover,
        userId: '',
        albumId: '',
        album: {
          albumName: '',
          albumReleasedate: '',
          albumCompany: '',
          albumProducer: '',
          
          songs: [
          /*
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
        {
            "songId": "1b5ed95e-g",
            "songName": "天情",
            "duration": 215,
            "songDate": null,
            "songGenre": null
        },
        */
        ],
        isLiked: false,
        },
         // 添加
        playIcon,
        playClickedIcon,
        playHoverIcon,
        likeIcon,
        likeHoverIcon,
        likeClickedIcon,
        addIcon,
        addHoverIcon,
        addClickedIcon,
      };
    },
    computed: {
      //歌曲计数
      songCount() {
        return this.album.songs.length;
      },
      //把发行日期转化为规范格式
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
      //点击播放专辑按钮，自动播放第一首歌曲
      playFirstSong() {
        const firstSongId = this.album.songs[0].songId;
        this.$router.push({ name: 'PlayerPage', params: { songId: firstSongId } });
      },
      //用户点击歌单任意歌曲，通过songId切换到播放页面
      playSong(songId) {
        this.$router.push({ name: 'PlayerPage', params: { songId: songId } });
      },

      //收藏方法
      async toggleLikeIcon(song) {
        try {
          if (song.liked) {
            // 如果已收藏，发送请求删除收藏
            await api.apiClient.delete(`/api/users/remove`, {
              data: {
              userId: this.userId,
              songId: song.songId
              }
            });
            song.liked = false;
          } else {
            // 如果未收藏，发送请求添加收藏
            await api.apiClient.post(`/api/users/add`, {
              userId: this.userId,
              songId: song.songId
            });
            song.liked = true;
          }
        } catch (error) {
          console.error('操作失败,请重试', error);
          song.liked = !song.liked; // 收藏失败，恢复到之前的状态
        }
      },

      // 在专辑列表内播放，暂停，跳转音乐的方法（目前暂未实现列表内播放，但前端仍可保留），涉及歌曲URL
      togglePlayIcon(song){
      try{
        if (this.currentPlayingSongId === song.songId && !this.audio.paused) {
          // 如果当前正在播放同一首歌，则暂停音乐
          this.audio.pause();
          song.playing = false; // 当前歌曲状态设为未播放
          this.currentPlayingSongId = null; // 清空当前播放的歌曲 ID
        } else {
          // 如果当前没有播放音乐或播放不同的音乐
          if (this.audio) {
            this.audio.pause(); // 暂停当前播放的音乐
            // 
            const previousSong = this.album.songs.find(s => s.songId === this.currentPlayingSongId);
            if (previousSong) {
              previousSong.playing = false;
            }
           }
          const songUrl = `/api/player/file?songId=${song.songId}`;
          this.audio = new Audio(song.songUrl); 
          this.audio.play();
          this.currentPlayingSongId = song.songId;
        }
        song.playing = !song.playing; // 切换播放状态
      }catch (error) {
          console.error('播放失败,请重试', error);
          song.playing = false; 
          this.currentPlayingSongId = null; // 清空当前播放的歌曲 ID
        }
      },
      
      // 将毫秒转换为分钟和秒
      formatDuration(duration) {
        const minutes = Math.floor(duration / 60);
        const seconds = duration % 60;
        return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
      },
    // 收藏与取消收藏专辑的方法
    async toggleLike() {
      try {
        if (this.isLiked) {
          await api.apiClient.delete(`/api/users/removealbum`, {
            data: {
          userId: this.userId,
          albumId: this.albumId
        }
      });
          this.isLiked = false;//取消收藏
        } else {
          await api.apiClient.post(`/api/users/addalbum`, {
            userId: this.userId,
            albumId: this.albumId
          });
          this.isLiked = true;//收藏
        }
        this.$forceUpdate(); // 强制更新视图
      } catch (error) {
        console.error('切换收藏状态失败:', error);
        this.isLiked = !this.isLiked;
        this.$forceUpdate(); // 强制更新视图
      }
    },
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
  
  .play-button, .like-button {
  margin-top: 20px;
  padding: 10px 20px;
  border: none;
  cursor: pointer;
}

.play-button {
  background-color: #284da0c1; 
  color: white;
  border-radius: 8px;
  margin-right: 2px;
}

.like-button {
  background-color: #fff;
  color: #284da0c1;
  border: 1px solid #284da0c1;
  border-radius: 8px;
  margin-left: 2px;
}

.like-button.liked {
  border: 1px solid darkgray;
  background-color: darkgray;
  color: white;
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
  .play-icon {
  width: 34px; /* 设置按钮的宽度 */
}
  .like-icon {
  width: 34px; /* 设置按钮的宽度 */
}
  </style>
  