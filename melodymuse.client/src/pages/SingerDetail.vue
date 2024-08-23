<template>
  <div class="page-container">
    <div class="profile">
      <TheHeader />
      <div class="header">
        <img class="profile-picture" :src="profilePicture" alt="Profile Picture" />
        <div class="profile-info">
          <h1>{{ artist.artistName }}</h1>
          <p style="color:darkgray">简介：{{ artist.artistIntro }},
          {{ artist.artistGenre }},
            出生于 {{ formattedBirthday }}。 <br />
          </p>
          <div class="stats">
            <span>单曲 {{ singleCount }}</span>
            <span>专辑 {{ albumCount }}</span>
          </div>
          <button class="play-button" @click="playFirstSong">▷播放歌手热门歌曲</button> 
          <button 
            class="follow-button" 
            :class="{ following: isFollowing }" 
            @click="toggleFollow">
            {{ isFollowing ? '已关注' : '+ 关注' }} {{ followersCount }}
          </button>
        </div>
      </div>
  
      <div class="songs">
        <h2 style=" margin-top: 10px;margin-bottom: 5px; margin-left: 10px;color:#284da0c1;" > 所有歌曲</h2>
        <div class="songs-table">
          <table>
            <thead>
              <tr>
                <th>歌曲</th>
                <th>专辑</th>
                <th>时长</th>
              </tr>
            </thead>
            <tbody>
              <tr v-for="(song, index) in songs" :key="index" @click="playSong(song.songId)">
                <td>{{ index + 1 }}. {{ song.songName }}</td>
                <td>{{ song.albumName }}</td>
                <td>{{ formatDuration(song.duration) }}</td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  </div>
  </template>
  
  <script>
  import profilePicture from '../assets/logo2.jpg';
  import axios from 'axios';
  import TheHeader from '@/components/TheHeader.vue';
  
  export default {
    data() {
      return {
        profilePicture,
        userId: '',//获取逻辑在下方created()中，从localStorage获取
        artistId: '',//为方便测试而设置一个值，实际需要从前一个页面（比如歌手列表）获取，获取逻辑在created中实现
        artist: {
          artistName:'',
          artistGenre: '',
          artistIntro: '',
          artistBirthday: ''
        },
        followersCount: 0, // 数字格式，方便处理加减
        isFollowing: false,
        songs:[]
        /*songs: [
          { songId: 1, songName: '圣诞星 (feat. 杨瑞代)',albumName: '圣诞星 (feat. 杨瑞代)', duration: '240' },
          { songId: 2, songName: '晴天', albumName: '叶惠美', duration: '312' },
          { songId: 3, songName: '搁浅', albumName: '七里香', duration: '260' },
          { songId: 4, songName: '青花瓷', albumName: '我很忙', duration: '189' },          
          // 其他歌曲省略
        ]*/
      };
    },
    computed: {
      singleCount() {
        return this.songs.length;  // 动态计算单曲数量
      },
      albumCount() {
        const albums = this.songs.map(song => song.albumName);
        return new Set(albums).size;  // 动态计算专辑数量（去重）
      },

      // 格式化出生日期
      formattedBirthday() {
      const birthday = new Date(this.artist.artistBirthday);
      return birthday.toLocaleDateString();
      }
    },
    components: {
      TheHeader,
    },
    methods: {
    //获取艺术家信息，前端已测试成功
    async fetchArtistData() {
      try {
        const artistResponse = await axios.get(`https://localhost:7223/api/artist/${this.artistId}`);
        this.artist = artistResponse.data;
        this.followersCount = artistResponse.data.artistFansNum;//获取当前艺术家的关注人数，，前端已测试成功
      } catch (error) {
        console.error('获取艺术家信息信息失败:', error);
      }
    },
    //获取歌单
    async fetchSongs() {
      try {
        const songsResponse = await axios.get(`https://localhost:7223/api/artist/${this.artistId}/songs`);
        this.songs = await Promise.all(
          songsResponse.data.map(async (song) => {
            const albumResponse = await axios.get(`https://localhost:7223/api/songs/${song.songId}/album`);
            return { ...song, albumName: albumResponse.data };
          })
        );
        /*map 方法遍历这个数组，并对每个 song 对象执行一个异步操作。
        return { ...song, albumName: albumResponse.data } 创建一个新的对象，
        这个对象包含了原有的 song 信息和从 albumResponse.data 获取到的 albumName。
        Promise.all 确保所有歌曲的专辑信息都被获取到并完成，然后统一返回结果。*/
      } catch (error) {
        console.error('获取歌单信息失败：', error);
      }
    },

    //点击播放热门歌曲，系统选择歌单的第一首歌，然后通过songId切换到播放页面
      playFirstSong() {
        const firstSongId = this.songs[0].songId;
        this.$router.push({ name: 'PlayerPage', params: { songId: firstSongId } });
      },

      //用户点击歌单任意歌曲，通过songId切换到播放页面
      playSong(songId) {
        this.$router.push({ name: 'PlayerPage', params: { songId: songId } });
      },

      //实现关注和取消关注
      async toggleFollow() {
    try {
      if (this.isFollowing) {
        // 如果已经关注，执行取消关注操作
        await axios.post(`https://localhost:7223/api/artist/unfollow`, {
          userId: this.userId, artistId: this.artistId
        });
        this.isFollowing = false;
        this.followersCount -= 1;
      } else {
        // 如果未关注，执行关注操作
        await axios.post(`https://localhost:7223/api/artist/follow`, {
          userId: this.userId,
          artistId: this.artistId
        });
        this.isFollowing = true;
        this.followersCount += 1;
      }
      // 更新粉丝人数到后端
      await this.updateFollowersCount();

    } catch (error) {
      console.error('Failed to toggle follow:', error);
      // 如果操作失败，恢复到原来的状态
      this.isFollowing = !this.isFollowing;
    }
  },
async updateFollowersCount() {
    try {
      await axios.post(`https://localhost:7223/api/artist/follow/increment-fans` , null, {
          params: { artistId: this.artistId}
        });
    } catch (error) {
      console.error('添加人数失败:', error);
    }
  },
    //处理歌曲时长，将秒数转换为 mm:ss 格式
    formatDuration(duration) {
      const minutes = Math.floor(duration / 60);
      const seconds = duration % 60;
      return `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
    }
  },  
  async created() {
    this.artistId = this.$route.params.artistId;  // 假设从路由参数中获取artistId，也可以换成其他方式
    if (this.artistId) {
      await this.fetchArtistData();
      await this.fetchSongs();
    } else {
      console.error('未找到 artistId');
    }
    this.userId ='001';// this.$route.params.userId
  }
  };
  </script>
  
  <style scoped>

.page-container {
  background: radial-gradient(circle, #f0f0f5, #d8d8fb); /* 从中心向外的渐变 */
  min-height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 20px;
}

.profile {
  width: 90%;
  max-width: 1200px; /* 设置最大宽度 */
  margin: 20px auto;
  background-color: #fff;
  border: 1px solid #ddd;
  box-shadow: 0 6px 12px rgba(0, 0, 0, 0.1); /* 增加阴影效果 */
}
  
  .header {
  display: flex;
  align-items: center;
  padding: 20px;
}
  
  .profile-picture {
    width: 150px;
    height: 150px;
    border-radius: 50%;
    margin-right: 20px;
  }
  
  .profile-info {
    flex: 1;
  }
  
  .stats {
    display: flex;
    gap: 10px;
    margin-top: 10px;
  }
  
  .play-button, .follow-button {
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
  
  .follow-button {
    background-color: #fff;
    color: #284da0c1;
    border: 1px solid #284da0c1;
    border-radius: 8px;
    margin-left: 2px;
  }
  
  
  .follow-button.following {
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
    max-height: 400px;
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
  