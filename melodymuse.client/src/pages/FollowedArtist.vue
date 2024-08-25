<template>
  <div>
    <TheHeader />
    <div class="header">
      <p class="header-text">我的关注</p>
    </div>
    <div class="followed-artist-list">
      <ul>
        <li v-for="singer in followedSingers" :key="singer.artistId" class="artist-item">
          <a @click="goToArtistPage(singer.artistId)">
            {{ singer.artistName }}
          </a>
        </li>
      </ul>
    </div>
    <TheFooter />
  </div>
</template>

<script>
import TheFooter from "../components/TheFooter.vue";
import TheHeader from "../components/TheHeader.vue";
import api from '../api/http.js'

export default {
  name: "FollowedArtist",
  components: {
    TheHeader,
    TheFooter,
  },
  data() {
    return {
      followedSingers: [],
    };
  },
  methods: {
    // 获取已关注的歌手信息
    async fetchFollowedSingers() {
      try {
        const response = await api.apiClient.get(`/api/artist/user/${this.userId}/followed`);
        this.followedSingers = response.data;
        console.log('用户信息:', this.followedSingers);
      } catch (error) {
        console.error('获取已关注的歌手信息失败:', error);
      }
    },
    // 跳转到歌手详情页
    goToArtistPage(artistId) {
      this.$router.push({ name: "SingerDetail", params: { artistId: artistId } });
    },
  },
  mounted() {
    this.userId = '001';//localStorage.getItem('userId');
    this.fetchFollowedSingers();
    
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

.followed-artist-list {
  margin-top: 20px;
  padding-right: 5px; /* 留出空间给滚动条 */
  max-height: 400px; /* 限制高度以启用滚动条 */
  overflow-y: auto; /* 启用垂直滚动条 */
}

.followed-artist-list::-webkit-scrollbar {
  width: 8px; /* 滚动条的宽度 */
}

.followed-artist-list::-webkit-scrollbar-thumb {
  background-color: #c4c4c4; /* 滚动条的颜色 */
  border-radius: 10px; /* 圆角 */
}

.followed-artist-list::-webkit-scrollbar-thumb:hover {
  background-color: #999; /* 悬停时滚动条的颜色 */
}

.followed-artist-list::-webkit-scrollbar-track {
  background-color: #f0f0f0; /* 滚动条轨道的颜色 */
}

.followed-artist-list ul {
  display: flex;
  flex-wrap: wrap;
  justify-content: left;
  padding: 0;
  list-style-type: none;
}

.artist-item {
  flex: 0 1 calc(25%); /* 每行 4 个，间隔 10px */
  margin-bottom: 10px;
  margin-top: 10px;
  text-align: center;
  background-color:#fafafaf5;
  height:30px;
}

.artist-item a {
  cursor: pointer;
  color: #333;
  font-size: medium;
}

.artist-item a:hover {
  text-decoration: underline;
  background-color: transparent;
}

.artist-item a:active {
  color: #8b9dcbc1;
  background-color: transparent;
}

::-webkit-scrollbar {
  width: 6px;
}

::-webkit-scrollbar-thumb {
  background-color: #c4c4c4;
  border-radius: 10px;
}

::-webkit-scrollbar-thumb:hover {
  background-color: #999;
}
</style>
