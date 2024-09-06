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

      <button @click="goToPlayPage" class="play-all-button">
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
            <tr v-for="(song, index) in songs" :key="song.songId">
              <td :class="{'top-three': index < 3}">{{ index + 1 }}</td>
              <td>
                  <a @click="goToPlay(song.songId)" class="song-link">
                      {{ song.songName }}
                  </a>
              </td>
              <td>{{ song.artistName }}</td>
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
import RankingImage from '../assets/logo2.jpg';
import api from "../api/http.js";
import TheHeader from "../components/TheHeader.vue";

export default {
  name: "ranksongs",
  components: {
    TheHeader,
  },
  data() {
    return {
      RankingImage,
      songs: [],
    };
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
            goToPlay(song){
                this.$store.commit('addSongToList', song);

                // 更新当前播放的歌曲 ID
                this.$store.commit('setId', song);
                try {
                    // 使用 Vue Router 导航到播放页面，传递歌曲 ID 和相关的歌曲列表
                    const songList = song;
                    this.$router.push({
                        name: 'mediaplayer',
                        params: {
                            songId: song, // 当前播放的歌曲 ID
                            songList: songList  // 歌曲列表的所有 songId
                        }
                    });
                } catch (error) {
                    console.error('跳转到播放页面失败:', error);
                }
    },
    // 跳转到播放页面
    goToPlayPage() {
    if (this.songs.length > 0) {
      // 获取歌曲列表的第一首歌的 ID
      const firstSongId = this.songs[0].songId;

      // 构建完整的歌曲 ID 列表字符串，作为路径参数传递
        const songList = this.songs.map(s => s.songId);
        this.$store.commit('setListOfSongs', songList);

        // 更新当前播放的歌曲 ID
        this.$store.commit('setId', firstSongId);

      // 使用 Vue Router 导航到 mediaplayer 页面，并传递歌曲 ID 和歌曲列表
      this.$router.push({
        name: 'mediaplayer',
        params: {
          songId: firstSongId, // 第一个歌曲的 ID
            songList: firstSongId   // 所有歌曲 ID 组成的字符串
        }
      });
    } else {
      console.error('歌曲列表为空，无法播放歌曲');
    }
  },
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
.song-link {
  color: #284da0c1; /* 蓝色文字 */
  background-color: transparent; /* 透明背景 */
}

.song-link:hover {
    background-color: transparent; /* 透明背景 */
    text-decoration: underline; /* 下划线 */
    text-decoration-color: #284da0c1; /* 蓝色下划线 */
}
</style>
