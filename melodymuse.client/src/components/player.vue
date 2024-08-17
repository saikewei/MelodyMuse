<template>
    <div class="music-player">
      <el-row>
        <el-col :span="8">
          <img :src="currentSong.cover" alt="Album Cover" class="album-cover" />
        </el-col>
        <el-col :span="16">
          <div class="song-info">
            <h2>{{ currentSong.title }}</h2>
            <h3>{{ currentSong.artist }}</h3>
            <audio ref="audio" :src="currentSong.url" @timeupdate="updateLyrics" controls></audio>
          </div>
          <div class="lyrics">
            <p v-for="(line, index) in parsedLyrics" :key="index" :class="{ active: index === currentLine }">
              {{ line.text }}
            </p>
          </div>
        </el-col>
      </el-row>
    </div>
  </template>
  
  <script>
  export default {
    data() {
      return {
        currentSong: {
          title: '我用什么把你留住',
          artist: 'Flower',
          cover: require('@../components/test.jpg'),
          url: require('@../components/test.mp3'),
          lyrics: `
            [00:00.00] Line 1 lyrics
            [00:10.00] Line 2 lyrics
            [00:20.00] Line 3 lyrics
            [00:30.00] Line 4 lyrics
          `,
        },
        currentLine: 0,
      };
    },
    computed: {
      parsedLyrics() {
        return this.currentSong.lyrics.split('\n').map((line) => {
          const parts = line.match(/\[(\d{2}):(\d{2})\.(\d{2})\](.*)/);
          return parts
            ? {
                time: parseInt(parts[1]) * 60 + parseInt(parts[2]) + parseInt(parts[3]) / 100,
                text: parts[4],
              }
            : { time: 0, text: '' };
        });
      },
    },
    methods: {
      updateLyrics() {
        const currentTime = this.$refs.audio.currentTime;
        for (let i = 0; i < this.parsedLyrics.length; i++) {
          if (currentTime < this.parsedLyrics[i].time) {
            this.currentLine = i - 1;
            break;
          }
        }
      },
    },
  };
  </script>
  
  <style scoped>
  .music-player {
    display: flex;
    align-items: center;
  }
  
  .album-cover {
    width: 100%;
    height: auto;
  }
  
  .song-info {
    margin-left: 20px;
  }
  
  .lyrics {
    margin-top: 20px;
    overflow-y: auto;
    height: 200px;
  }
  
  .lyrics p {
    margin: 5px 0;
    font-size: 16px;
  }
  
  .lyrics .active {
    color: #409eff;
  }
  </style>