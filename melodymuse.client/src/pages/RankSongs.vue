<template>
  <div>
    <TheHeader />
    <div class="main-container">
  <div class="sidebar">
    <button @click="navigateTo('rankartists')">音乐人推荐榜</button>
    <button @click="navigateTo('ranksongs')">热歌榜</button>
  </div>
    <div class="ranking-container">
      <div>
        <img class="ranking-image2" :src="RankingImage" alt="Ranking Image" />
      </div>
      <h1 class="ranking-title2">
        <span>热歌榜</span><strong>Top50</strong>
      </h1>

      <button @click="goToPlayPage(topSong.songId)" class="play-all-button">
      ▷播放全部
      </button>

      <!-- Song list with scrollable container -->
      <div class="ranking-table-container">
        <table class="ranking-table">
          <thead>
            <tr>
              <th>排名</th>
              <th>歌名</th>
              <th>歌手</th>
              <th>时长</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(song, index) in songs" :key="song.songId" @click="goToPlayPage(song.songId)">
              <td :class="{'top-three': index < 3}">{{ index + 1 }}</td>
              <td>{{ song.songName }}</td>
              <td>{{ song.artistName }}</td>
              <td>{{ formatDuration(song.duration) }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    </div>
    <TheFooter />
  </div>
</template>

<script>
import RankingImage from '../assets/logo2.jpg';
import api from "../api/http.js";
import TheFooter from "../components/TheFooter.vue";
import TheHeader from "../components/TheHeader.vue";

export default {
  name: "ranksongs",
  components: {
    TheHeader,
    TheFooter
  },
  data() {
    return {
      RankingImage,
      songs: [],
    };
  },
  computed: {
    topSong() {
      return this.songs[0];
    }
  },
  methods: {
    // 两个排行榜互相跳转
    navigateTo(routeName) {
    this.$router.push({ name: routeName });
  },
    fetchSongs() {
      api.apiClient.get('/api/rank/top-songs')
        .then(response => {
          console.log(response.data); // 检查 API 返回的数据
          this.songs = response.data; // 直接将后端返回的排序好的数据赋值给 songs
        })
        .catch(error => {
          console.error('获取歌曲信息失败:', error);
        });
    },
    // 格式化时长,将秒数转换为分钟:秒数
    formatDuration(seconds) {
      const minutes = Math.floor(seconds / 60);
      const remainingSeconds = seconds % 60;
      return `${minutes}:${remainingSeconds < 10 ? '0' : ''}${remainingSeconds}`;
    },
    // 跳转到播放页面
    goToPlayPage(songId) {
      console.log(`Navigating to play page for song ID: ${songId}`);
      this.$router.push({ name: 'PlayerPage', params: { songId: songId } });
    }
  },
  mounted() {
    this.fetchSongs();
  }
};
</script>

<style>
.main-container {
  display: flex;
  width:100%;
}

.sidebar {
  width: 150px;
  background-color: white;
  padding: 20px 10px;
  display: flex;
  flex-direction: column;
  align-items: center;
}

.sidebar button {
  margin-bottom: 20px;
  padding: 10px 0px;
  background-color: white;
  color: #193169;
  border-color: #d2deef;
  border-radius: 5px;
  cursor: pointer;
  width: 100%;
  text-align: center;
}

.sidebar button:hover {
  background-color: #fff;
}

.ranking-container {
  flex: 1;
  padding: 40px;
  background-color: #d2deef;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  position: relative;
  width: 100%;
}

.ranking-image2 {
  position:absolute;
  top:6%;
  left:32%;
  width: 100px;
  height: 100px;
  border-radius: 50%;
  margin-right: 20px;
}

.ranking-title2 {
  text-align: center;
  font-size: 40px;
  margin-left: 10px;
  margin-bottom: 20px;
  color: #193169c1; 
  letter-spacing: 2px;
}

.ranking-title2 span {
  font-weight: normal;
  font-size: -2em;
  margin-left: 40px;
  margin-right: 10px;
}

.ranking-title2 strong {
  font-weight: bold;
  font-size: 1.7em;
  color: white;
  font-style: italic;
  text-shadow: 2px 2px 5px rgba(24, 44, 145, 0.6);
}

.play-all-button {
  display: block;
  margin: 0 auto 30px;
  padding: 10px 20px;
  background-color: #284da0c1;
  color: #fff;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.play-all-button:hover {
  background-color: #284da0c1;
}

.ranking-table-container {
  max-height: 490px;
  overflow-y: auto;
}

.ranking-table {
  width: 100%;
  border-collapse: collapse;
  background-color: white;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
  text-align: center; /* 列属性居中 */
}

.ranking-table th, .ranking-table td {
  padding: 10px;
  text-align: center;
  border-bottom: 1px solid #ddd;
}

.ranking-table th {
  background-color: #f2f2f2;
}

.ranking-table tr:hover {
  background-color: #f5f5f5;
  cursor: pointer;
}

.top-three {
  font-weight: bold;
  text-shadow: 2px 2px 5px rgba(24, 44, 145, 0.2);
  color: #284da0c1;
  font-size: 1.5em;
}
</style>
