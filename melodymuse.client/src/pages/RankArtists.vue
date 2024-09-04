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
        <img class="ranking-image1" :src="RankingImage" alt="Ranking Image" />
      </div>
      <h1 class="ranking-title1">
        <span>音乐人推荐榜</span><strong>Top50</strong>
      </h1>

      <!-- Artist list with scrollable container -->
      <div class="ranking-table-container">
        <table class="ranking-table">
          <thead>
            <tr>
              <th>音乐人</th>
              <th>粉丝数</th>
              <th>总播放次数</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="(artist, index) in artists" :key="artist.artistId">
              <td>
                <router-link :to="{ name: 'SingerDetail', params: { artistId: artist.artistId } }" class="artist-link">
                  {{ artist.artistName }}
                </router-link>
              </td>
              <td>{{ artist.fansCount }}</td>
              <td>{{ artist.totalPlayCount }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    </div>
  </div>
</template>

<script>
import RankingImage from '../assets/logo2.jpg';
import api from "../api/http.js";
import TheHeader from "../components/TheHeader.vue";

export default {
  name: "rankartists",
  components: {
    TheHeader,
  },
  data() {
    return {
      RankingImage,
      artists: []
    };
  },
  methods: {
    // 两个排行榜互相跳转
    navigateTo(routeName) {
    this.$router.push({ name: routeName });
    },
    fetchArtists() {
      api.apiClient.get('/api/rank/ranking')
        .then(response => {
          // 假设数据已经根据rankScore排好序并选择前50名音乐人
          this.artists = response.data;
        })
        .catch(error => {
          console.error('获取音乐人信息失败:', error);
        });
      },
  },
  mounted() {
    this.fetchArtists();
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
  background-color: #f4f4f4;
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

.ranking-image1 {
  position:absolute;
  top:6%;
  left:28%;
  width: 100px;
  height: 100px;
  border-radius: 50%;
  margin-right: 20px;
}

.ranking-title1 {
  text-align: center;
  font-size: 40px;
  margin-left: 10px;
  margin-bottom: 85px;
  color: #193169c1; 
  letter-spacing: 2px; /* 调整字间距，可以根据需要增大或减小 */
}

.ranking-title1 span {
  font-weight: normal;
  font-size: -2em;
  margin-left: 40px;
  margin-right: 10px;
}

.ranking-title1 strong {
  font-weight: bold;
  font-size: 1.7em; /* 放大字体 */
  color: white; 
  font-style: italic; /* 设置为斜体 */
  text-shadow: 2px 2px 5px rgba(24, 44, 145, 0.6); /* 添加阴影 */
}

.ranking-table-container {
  max-height: 490px; /* 设置一个合适的高度 */
  overflow-y: auto;  /* 添加垂直滚动条 */
}

.ranking-table {
  width: 100%;
  border-collapse: collapse;
  background-color: white;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.ranking-table th, .ranking-table td {
  padding: 10px;
  text-align: center; /* 使列居中 */
  border-bottom: 1px solid #ddd;
}

.ranking-table th {
  background-color: #f2f2f2;
}

.ranking-table tr:hover {
  background-color: #f5f5f5;
  cursor: pointer; /* 鼠标悬停时显示为指针 */
}

.artist-link {
  color: #284da0c1; /* 蓝色文字 */
  background-color: transparent; /* 透明背景 */
}

.artist-link:hover {
    text-decoration: underline; /* 下划线 */
    text-decoration-color: #284da0c1; /* 蓝色下划线 */
}
</style>
