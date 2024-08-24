<template>
  <div>
    <TheHeader />
    <div class="ranking-container">
      <div>
        <img class="ranking-image" :src="RankingImage" alt="Ranking Image" />
      </div>
      <h1 class="ranking-title">
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
            <tr v-for="(artist, index) in artists" :key="artist.artistId" @click="goToArtistPage(artist.artistId)">
              <td>{{ artist.artistName }}</td>
              <td>{{ artist.fansCount }}</td>
              <td>{{ artist.totalPlayCount }}</td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
    <TheFooter />
  </div>
</template>

<script>
import RankingImage from '../assets/logo2.jpg';
import axios from 'axios';
import TheFooter from "../components/TheFooter.vue";
import TheHeader from "../components/TheHeader.vue";

export default {
  name: "rankartists",
  components: {
    TheHeader,
    TheFooter
  },
  data() {
    return {
      RankingImage,
      artists: []
    };
  },
  methods: {
    fetchArtists() {
      axios.get('https://localhost:7223/api/rank/ranking')
        .then(response => {
          // 假设数据已经根据rankScore排好序并选择前50名音乐人
          this.artists = response.data;
        })
        .catch(error => {
          console.error('获取音乐人信息失败:', error);
        });
      },
    goToArtistPage(artistId) {
      console.log(`Navigating to artist page with ID: ${artistId}`);
      this.$router.push({ name: 'ArtistDetail', params: { artistId: artistId } });
    }
  },
  mounted() {
    this.fetchArtists();
  }
};
</script>

<style>
.ranking-container {
  width: 90%;
  left:5%;
  position: relative;
  padding: 40px;
  background-color: #d2deef;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1); /* 给整个容器添加阴影 */
}

.ranking-image {
  position:absolute;
  top:14%;
  left:26%;
  width: 100px;
  height: 100px;
  border-radius: 50%;
  margin-right: 20px;
}

.ranking-title {
  text-align: center;
  font-size: 40px;
  margin-left: 10px;
  margin-bottom: 20px;
  color: #193169c1; 
  letter-spacing: 2px; /* 调整字间距，可以根据需要增大或减小 */
}

.ranking-title span {
  font-weight: normal;
  font-size: -2em;
  margin-left: 40px;
  margin-right: 10px;
}

.ranking-title strong {
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
</style>
