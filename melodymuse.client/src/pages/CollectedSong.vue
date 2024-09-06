<template>
    <div>
      <TheHeader />
      <div class="header">
        <p class="header-text">我的收藏</p>
        <div class="buttons">
          <button 
            @click="navigateTo('CollectedSong')"
            :class="{'active': currentTab === 'song'}">
            歌曲
          </button>
          <button 
            @click="navigateTo('CollectedAlbum')"
            :class="{'active': currentTab === 'album'}">
            专辑
          </button>
        </div>
      </div>
      <div class="collected-songs-list">
        <table>
          <thead>
            <tr>
              <th class="song-column">歌曲</th>
              <th class="artist-column">歌手</th>
              <th class="album-column">专辑</th>
              <th class="duration-column">时长</th>
              <th class="action-column">操作</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(song, index) in collectedSongs" :key="index">
              <td>
                <el-tooltip content="播放歌曲" placement="bottom">
                  <img :src="song.playing ? playClickedIcon : song.playHover ? playHoverIcon : playIcon"
                       @mouseover="song.playHover = true"
                       @mouseleave="song.playHover = false"
                       @click.stop="togglePlayIcon(song.songId)"
                       class="play-icon"
                       alt="播放歌曲" />
                </el-tooltip>
                {{ index + 1 }}. {{ song.songName }}
              </td>
              <td>{{ song.artistName }}</td>
              <td>{{ song.albumName }}</td>
              <td>{{ formatDuration(song.duration) }}</td>
              <td>
                <el-tooltip content="取消收藏" placement="bottom">
                  <img :src="song.liked ? likeClickedIcon : song.likeHover ? likeHoverIcon : likeIcon"
                       @mouseover="song.likeHover = true"
                       @mouseleave="song.likeHover = false"
                       @click="toggleLikeIcon(song)"
                       class="like-icon"
                       alt="取消收藏" />
                </el-tooltip>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </template>
  
  <script>
  import TheHeader from "../components/TheHeader.vue";
  import api from '../api/http.js';
  import axios from 'axios';
  import playIcon from '../assets/pics/play.png'; 
  import playClickedIcon from '../assets/pics/play-click.png'; 
  import playHoverIcon from '../assets/pics/play-cover.png'; 
  import likeIcon from '../assets/pics/like.png'; 
  import likeHoverIcon from '../assets/pics/like-cover.png'; 
  import likeClickedIcon from '../assets/pics/like-click.png'; 
  
  export default {
    name: "CollectedSong",
    components: {
      TheHeader,
    },
    data() {
      return {
        collectedSongs: [
      
        ],
        playIcon,
        playClickedIcon,
        playHoverIcon,
        likeIcon,
        likeHoverIcon,
        likeClickedIcon,
        currentTab: 'song', // 当前页面为歌曲
      };
    },
    methods: {
        togglePlayIcon(song) {
            console.log(song);
            this.$router.push({ path: `/mediaplayer/${song.songId}/${song.songId}` })
        },
        // 页面跳转
    navigateTo(page) {
      if (page === 'CollectedSong') {
        this.currentTab = 'song';
      } else {
        this.currentTab = 'album';
        this.$router.push({ name: 'CollectedAlbum' });
      }
    },
      // 获取收藏的歌曲信息
      async fetchCollectedSongs() {
        try {
          const response = await api.apiClient.get(`/api/users/collectsong`);
         // const response = await axios.get(`https://localhost:7223/api/songs/user/${this.userId}/collected`);
          const songs = response.data.map(song => ({ ...song, liked: true }));
          // 获取每首歌的专辑名称并更新数据
        for (let song of songs) {
          const albumResponse = await api.apiClient.get(`/api/songs/${song.songId}/album`);
          //const albumResponse = await axios.get(`https://localhost:7223/api/songs/${song.songId}/album`);
          song.albumName = albumResponse.data.albumName;
        }

        this.collectedSongs = songs;
          console.log('收藏的歌曲:', this.collectedSongs);
        } catch (error) {
          console.error('获取收藏的歌曲信息失败:', error);
        }
      },

      // 切换收藏状态
      async toggleLikeIcon(song) {
      song.liked = !song.liked;
      try {
        await api.apiClient.delete(`/api/users/remove`, {
        //await axios.delete(`https://localhost:7223/api/users/remove`, {
          data: {
            userId: this.userId,
            songId: song.songId
          }
        });
        await this.fetchCollectedSongs();
      } catch (error) {
        console.error('取消收藏失败,请重试', error);
      }
    },
      // 将秒转换为分钟和秒
      formatDuration(duration) {
        const minutes = Math.floor(duration / 60);
        const seconds = duration % 60;
        return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
      },
    },
    mounted() {
      this.userId = '001'; // localStorage.getItem('userId');
      this.fetchCollectedSongs();
    },
  };
  </script>
  
  <style scoped>
  .header {
    height: 220px;
    background-color: #284da0c1;
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    margin-bottom: 20px;
  }
  
  .header-text {
    color: aliceblue;
    font-size: 30px;
    margin-top: 45px;
    text-align: center;
  }
  
  .buttons {
    margin-top: 10px;
  }
  
  .buttons button {
    padding: 10px 20px;
    margin: 10px 10px;
    border: none;
    border-radius: 20px;
    background-color: transparent;
    color: white;
    cursor: pointer;
  }
  
  .buttons button.active {
    background-color: white;
    color: black;
  }
  .collected-songs-list {
    margin-top: 20px;
    padding-right: 5px;
    max-height: 400px;
    overflow-y: auto;
  }
  
  .collected-songs-list::-webkit-scrollbar {
    width: 8px;
  }
  
  .collected-songs-list::-webkit-scrollbar-thumb {
    background-color: #c4c4c4;
    border-radius: 10px;
  }
  
  .collected-songs-list::-webkit-scrollbar-thumb:hover {
    background-color: #999;
  }
  
  .collected-songs-list::-webkit-scrollbar-track {
    background-color: #f0f0f0;
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
    width: 34px;
  }
  
  .like-icon {
    width: 34px;
  }
  .song-column {
    width: 40%;
  }
  
  .artist-column {
    width: 20%;
  }
  
  .album-column {
    width: 20%;
  }

  .duration-column {
    width: 10%;
  }
  
  .action-column {
    width: 10%;
  }
  </style>
  