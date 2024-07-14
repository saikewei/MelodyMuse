<template>
  <div id="app" class="container">
    <transition name="fade" mode="out-in">
      <div v-if="showPlayer" key="player">
        <Player :track="currentTrack" :audio="audio" :lyrics="currentLyrics" @go-back="togglePlayerView" />
      </div>
      <div v-else key="home">
        <el-row>
          <el-col :span="24">
            <h1 class="text-center mb-4">Music Player</h1>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="24" class="content">
            <el-table :data="musicFiles" style="width: 80%">
              <el-table-column prop="title" label="歌曲标题" align="left" header-align="left">
                <template #default="scope">
                  <el-button type="link" @click="playMusic(scope.row)" style="font-size: 1.2rem;">
                    {{ scope.row.title }}
                  </el-button>
                </template>
              </el-table-column>
              <el-table-column prop="duration" label="时长" align="left" header-align="left" />
              <el-table-column prop="artist" label="歌手" align="left" header-align="left" />
              <el-table-column prop="album" label="专辑" align="left" header-align="left" />
            </el-table>
          </el-col>
        </el-row>
      </div>
    </transition>
    <div id="custom-player" class="player">
      <div class="left-section" @click="togglePlayerView">
        <div class="cover" :style="{ backgroundImage: currentTrack.cover_url ? `url(${currentTrack.cover_url})` : 'none' }">
          <div v-if="!currentTrack.cover_url" class="placeholder"></div>
        </div>
        <div class="track-info">
          <div>{{ currentTrack.title }}</div>
          <div>{{ currentTrack.artist }}</div>
        </div>
      </div>
      <div class="right-section">
        <div class="controls">
          <button @click="prevTrack">⏮️</button>
          <button @click="togglePlay">
            <el-icon v-if="isPlaying">
              <IconFillPause />
            </el-icon>
            <el-icon v-else>
              <IconFillPlay />
            </el-icon>
          </button>
          <button @click="nextTrack">⏭️</button>
        </div>
        <div class="progress">
          <input type="range" min="0" :max="duration" v-model="currentTime" @input="seek" />
          <div>{{ formatTime(currentTime) }} / {{ formatTime(duration) }}</div>
        </div>
        <div class="volume">
          <label for="volume">Volume:</label>
          <input type="range" id="volume" min="0" max="1" step="0.01" v-model="volume" @input="setVolume" />
        </div>
        <div class="speed">
          <label for="speed">Speed:</label>
          <select id="speed" v-model="speed" @change="setSpeed">
            <option value="0.5">0.5x</option>
            <option value="1">1x</option>
            <option value="1.5">1.5x</option>
            <option value="2">2x</option>
          </select>
        </div>
      </div>
    </div>
    <audio ref="audio" @timeupdate="updateTime" @ended="nextTrack"></audio>
  </div>
</template>

<script>
import axios from 'axios';
import { defineComponent } from 'vue';
import { ElTable, ElTableColumn, ElRow, ElCol, ElButton, ElIcon } from 'element-plus';
import Player from './Player.vue';

export default defineComponent({
  name: 'MusicPlayer',
  components: {
    ElTable,
    ElTableColumn,
    ElRow,
    ElCol,
    ElButton,
    ElIcon,
    Player
  },
  data() {
    return {
      musicFiles: [],
      currentTrack: {},
      currentLyrics: [],
      isPlaying: false,
      currentTime: 0,
      duration: 0,
      audio: null,
      showPlayer: false,
      volume: 0.5, 
      speed: 1 
    };
  },
  mounted() {
    this.fetchMusicFiles();
    this.audio = this.$refs.audio;
  },
  methods: {
    fetchMusicFiles() {
      axios.get('https://localhost:7086/api/music')
        .then(response => {
          if (Array.isArray(response.data)) {
            this.musicFiles = response.data;
          } else {
            console.error('API response is not an array:', response.data);
          }
        })
        .catch(error => {
          console.error('Failed to fetch music files:', error);
        });
    },
    playMusic(music) {
      this.currentTrack = music;
      this.audio.src = `https://localhost:7086/api/music/file/${music.fileName}`;
      this.audio.play();
      this.isPlaying = true;

      this.fetchLyrics(music.lyricsUrl);
      this.fetchCover(music.coverUrl);
    },
    fetchLyrics(fileName) {
      axios.get(`https://localhost:7086/api/music/lyrics/${fileName}`)
        .then(response => {
          this.currentLyrics = this.parseLyrics(response.data);
        })
        .catch(error => {
          console.error('Failed to fetch lyrics:', error);
        });
    },
    fetchCover(fileName) {
      axios.get(`https://localhost:7086/api/music/covers/${fileName}`, { responseType: 'blob' })
        .then(response => {
          const url = URL.createObjectURL(response.data);
          this.currentTrack.cover_url = url;
        })
        .catch(error => {
          console.error('Failed to fetch cover:', error);
        });
    },
    parseLyrics(lyrics) {
      const lines = lyrics.split('\n');
      return lines.map(line => {
        const match = line.match(/\[(\d+:\d+\.\d+)\] (.+)/);
        if (match) {
          const [minutes, seconds] = match[1].split(':').map(Number);
          return {
            time: minutes * 60 + seconds,
            text: match[2]
          };
        }
        return null;
      }).filter(line => line !== null);
    },
    togglePlay() {
      if (this.isPlaying) {
        this.audio.pause();
      } else {
        this.audio.play();
      }
      this.isPlaying = !this.isPlaying;
    },
    updateTime() {
      this.currentTime = this.audio.currentTime;
      this.duration = this.audio.duration;
    },
    seek(event) {
      this.audio.currentTime = event.target.value;
    },
    formatTime(seconds) {
      const minutes = Math.floor(seconds / 60);
      const secs = Math.floor(seconds % 60);
      return `${minutes}:${secs < 10 ? '0' : ''}${secs}`;
    },
    prevTrack() {
      const currentIndex = this.musicFiles.indexOf(this.currentTrack);
      if (currentIndex > 0) {
        this.playMusic(this.musicFiles[currentIndex - 1]);
      }
    },
    nextTrack() {
      const currentIndex = this.musicFiles.indexOf(this.currentTrack);
      if (currentIndex < this.musicFiles.length - 1) {
        this.playMusic(this.musicFiles[currentIndex + 1]);
      }
    },
    togglePlayerView() {
      this.showPlayer = !this.showPlayer;
    },
    setVolume() {
      this.audio.volume = this.volume;
    },
    setSpeed() {
      this.audio.playbackRate = this.speed;
    }
  }
});
</script>

<style scoped>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  margin-top: 60px;
}

.text-center {
  text-align: center;
}

.mb-4 {
  margin-bottom: 1.5rem;
}

.el-table th,
.el-table td {
  font-size: 1.2rem;
}

.blue-icon {
  color: blue;
}

.content {
  display: flex;
  justify-content: center;
  align-items: center;
  padding-bottom: 100px;
}

.el-table {
  width: 80%;
  margin: 0 auto;
}

.player {
  position: fixed;
  bottom: 0;
  width: 80%;
  left: 50%;
  transform: translateX(-50%);
  background-color: white;
  padding: 10px 0;
  box-shadow: 0 -2px 5px rgba(0, 0, 0, 0.1);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.left-section {
  display: flex;
  align-items: center;
  margin-left: 20px;
}

.right-section {
  display: flex;
  flex-direction: column;
  align-items: center;
  margin-right: 20px;
}

.player .cover {
  width: 50px;
  height: 50px;
  margin-right: 10px;
  background-size: cover;
  background-position: center;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: #ccc;
}

.player .placeholder {
  width: 100%;
  height: 100%;
  background-color: #ccc;
}

.player .track-info {
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.player .controls {
  display: flex;
  align-items: center;
  margin-bottom: 10px;
}

.player .controls button {
  margin: 0 10px;
  background: none;
  border: none;
  font-size: 1.5rem;
  cursor: pointer;
}

.player .progress {
  display: flex;
  align-items: center;
  width: 100%;
  max-width: 500px;
  padding: 0 20px;
}

.player .progress input[type="range"] {
  flex-grow: 1;
  margin: 0 10px;
  width: 250px;
}

.volume, .speed {
  display: flex;
  align-items: center;
  margin-bottom: 5px;
}

.volume label, .speed label {
  margin-right: 5px;
}

.volume input[type="range"] {
  width: 100px;
  margin-left: 10px;
}

.speed select {
  margin-left: 10px;
  padding: 2px;
  border-radius: 5px;
  border: 1px solid #ced4da;
}

.speed select:focus {
  outline: none;
  border-color: #007bff;
  box-shadow: 0 0 5px rgba(0, 123, 255, 0.25);
}

</style>
