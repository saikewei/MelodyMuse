<template>
  <div>
    <TheHeader />
    <div class="ranking-container">
      <div>
        <img class="ranking-image" :src="RankingImage" alt="Ranking Image" />
      </div>
      <h1 class="ranking-title">
        <span>热歌榜</span><strong>Top100</strong>
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
              <td>{{ song.artist }}</td>
              <td>{{ formatDuration(song.duration) }}</td>
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
  name: "ranksongs",
  components: {
    TheHeader,
    TheFooter
  },
  data() {
    return {
      RankingImage,
      songs: [
        {
          songId: '1',
          songName: 'Song of the Stars',
          artist: 'Lun',
          duration: '100',
          playCount: '100'
        },
        {
          songId: '9',
          songName: 'Whispering Winds',
          artist: 'Zephyr',
          duration: '300',
          playCount: '20'
        },
        {
          songId: '3',
          songName: 'Ocean Waves',
          artist: 'Marina',
          duration: '210',
          playCount: '120',
        },
        {
          songId: '4',
          songName: 'Mountain Echo',
          artist: 'Echo',
          duration: '340',
          playCount: '130'
        },
        {
          songId: '5',
          songName: 'Silent Night',
          artist: 'Nocturne',
          duration: '178',
          playCount: '100'
        },
        {
          songId: '6',
          songName: 'Sunrise Melody',
          artist: 'Aurora',
          duration: '264',
          playCount: '100'
        },
        {
          songId: '3',
          songName: 'Ocean Waves',
          artist: 'Marina',
          duration: '360',
          playCount: '140'
        },
        {
          songId: '4',
          songName: 'Mountain Echo',
          artist: 'Echo',
          duration: '270',
          playCount: '100'
        },
        {
          songId: '5',
          songName: 'Silent Night',
          artist: 'Nocturne',
          duration: '220',
          playCount: '100'
        },
        {
          songId: '3',
          songName: 'Ocean Waves',
          artist: 'Marina',
          duration: '360',
          playCount: '100'
        },
        {
          songId: '4',
          songName: 'Mountain Echo',
          artist: 'Echo',
          duration: '270',
          playCount: '100'
        },
        {
          songId: '5',
          songName: 'Silent Night',
          artist: 'Nocturne',
          duration: '220',
          playCount: '100'
        },]
    };
  },
  computed: {
    topSong() {
      return this.songs[0];
    }
  },
  methods: {
    fetchSongs() {
      axios.get('https://localhost:7223/api/rank/with-playcount')
        .then(response => {
          const sortedSongs = response.data.sort((a, b) => b.playCount - a.playCount);
          const top100Songs = sortedSongs.slice(0, 100);
          this.songs = top100Songs.map(song => ({
            songId: song.songId,
            songName: song.songName,
            artist: song.artistName,
            duration: song.duration,
            playCount: song.totalPlays,  
          }));
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
.ranking-container {
  width: 90%;
  left:5%;
  position: relative;
  padding: 40px;
  background-color: #d2deef;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.ranking-image {
  position:absolute;
  top:6%;
  left:30%;
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
  letter-spacing: 2px;
}

.ranking-title span {
  font-weight: normal;
  font-size: -2em;
  margin-left: 40px;
  margin-right: 10px;
}

.ranking-title strong {
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
