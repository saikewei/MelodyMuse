<template>
  <div class="music-player">
    <div class="content">
      <el-col :span="12" class="image-container">
        <img :src="currentImage" alt="Album cover" class="album-image" />
      </el-col>
      <el-col :span="12" class="lyrics-container">
        <div class="lyrics-wrapper">
          <div
            v-for="(line, index) in lyrics"
            :key="index"
            :class="['lyrics-line', { 'current-line': index === currentLine }]"
            :style="getLineStyle(index)"
          >
            {{ line.text }}
          </div>
        </div>
      </el-col>
    </div>
    <div class="player-controls">
      <button class="play-pause-button" @click="togglePlaying">
        <svg v-if="!playing" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M8 5v14l11-7z"/></svg>
        <svg v-else xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M6 19h4V5H6v14zm8-14v14h4V5h-4z"/></svg>
      </button>
      <input type="range" class="progress-slider" v-model="progress" :max="duration" @input="seek" />
      <button class="prev-song-button" @click="prevSong">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M16 6L6 16M6 6l10 10"/></svg>
      </button>
      <button class="next-song-button" @click="nextSong">
        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24"><path d="M8 6L18 16M18 6l-10 10"/></svg>
      </button>
      <audio ref="audioElement" :src="audioSrc" @timeupdate="updateProgress" @durationchange="updateDuration" :controls="false"></audio>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted, watch } from 'vue';
import axios from 'axios';
import { useRouter, useRoute } from 'vue-router';

const router = useRouter(); // 获取 Vue Router 实例
const route = useRoute(); 

const songId=route.params.songId;

const audioSrc = ref('test.mp3'); // 替换为实际音频文件路径

const currentImage = ref('/th.jpg');

fetchSongInfo(songId);

// 解析歌词
const parseLyrics = (lyricsText) => {
  return lyricsText.split('\n').flatMap(line => {
    const matches = line.matchAll(/\[(\d{2}:\d{2}\.\d{3})\]/g);
    const results = [];
    for (const match of matches) {
      const time = match[1];
      const minutes = parseInt(time.substring(0, 2), 10);
      const seconds = parseFloat(time.substring(3));
      const timestamp = minutes * 60 + seconds;
      // 提取时间戳后面的文本
      const text = line.replace(/\[.*?\]/g, '').trim();
      results.push({ time: timestamp, text: text });
    }
    return results;
  }).filter(Boolean);
};

const lyrics = '';

const currentLine = ref(0);
const audioElement = ref(null);
const progress = ref(0); // 当前播放位置
const duration = ref(0); // 音频总长度
const playing = ref(false);

onMounted(() => {
  const update = () => {
    updateProgress({ target: audioElement.value });
    requestAnimationFrame(update);
  };
  requestAnimationFrame(update);
});

async function fetchSongInfo(songId) {
  console.log(songId)
  try {
    const response = await axios.get(`http://localhost:7223/api/player/${songId}`);
    const songInfo = response.data;


    console.log('Song Info:', songInfo);

    return songInfo;
  } catch (error) {
    console.error('Error fetching song info:', error);
    if (error.response && error.response.status === 401) {
      console.error('Unauthorized access. Please log in again.');
    }
    throw error;
  }
}


function getLineStyle(index) {
  const distance = Math.abs(currentLine.value - index);
  const maxDistance = 2;
  const scale = Math.max(1 - distance * 0.15, 0.85);
  const opacity = Math.max(1 - distance * 0.3, 0.5);
  return {
    transform: `scale(${scale})`,
    opacity: opacity,
    display: distance > maxDistance ? 'none' : 'block',
  };
}

// 更新音频播放进度
function updateProgress(event) {
  progress.value = event.target.currentTime;

  // 根据当前播放时间更新歌词显示
  updateLyricDisplay(event.target.currentTime);
}

// 更新音频总时长
function updateDuration(event) {
  duration.value = event.target.duration;
}

// 跳转到指定时间
function seek(value) {
  audioElement.value.currentTime = value;
  updateProgress({ target: audioElement.value });
}


// 更新歌词显示
// 更新歌词显示
function updateLyricDisplay(currentTime) {
  let lineIndex = 0;
  for (let i = 0; i < lyrics.value.length; i++) {
    if (lyrics.value[i].time >= currentTime) {
      lineIndex = i;
      break;
    }
  }
  currentLine.value = lineIndex;
}


// 切换播放/暂停状态
function togglePlaying() {
  if (playing.value) {
    audioElement.value.pause();
  } else {
    audioElement.value.play();
  }
  playing.value = !playing.value;
}

// 播放下一首歌曲
function nextSong() {
  // 这里你可以添加逻辑来切换到下一首歌曲
  console.log('Next song');
}

// 播放上一首歌曲
function prevSong() {
  // 这里你可以添加逻辑来切换到上一首歌曲
  console.log('Previous song');
}
</script>

<style scoped>
.music-player {
  display: flex;
  flex-direction: column; /* 修改为column布局 */
  align-items: center;
  justify-content: space-between;
  height: 80vh;
}

.content {
  display: flex;
  width: 100%;
  height: 80%; /* 占据80%的高度 */
}

.image-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%; /* 占据内容容器的全部高度 */
  padding: 20px;
}

.album-image {
  width: 100%;
  height: 100%; /* 图片高度占据容器的高度 */
  object-fit: cover; /* 让图片覆盖整个容器 */
  border-radius: 10px;
}

.lyrics-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100%; /* 占据内容容器的全部高度 */
  overflow: hidden; /* 防止歌词超出容器 */
}

.lyrics-wrapper {
  text-align: center;
  width: 100%;
  display: flex;
  flex-direction: column;
  justify-content: center;
}

.lyrics-line {
  transition: transform 0.3s ease, opacity 0.3s ease; /* 添加过渡效果 */
}


.current-line {
  font-size: 1.5em; /* 当前歌词行字体稍大 */
  font-weight: bold;
  color: #42b983; /* 使用你喜欢的颜色 */
}

.player-controls {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  width: 100%; /* 控制器宽度与父级相同 */
  height: 20%; /* 控制器高度 */
  padding: 10px 0;
}

/* 隐藏默认的播放器控件 */
audio {
  visibility: hidden;
  position: absolute;
}

.play-pause-button,
.prev-song-button,
.next-song-button {
  background-color: transparent;
  border: none;
  cursor: pointer;
  margin: 0 10px;
}

.progress-slider {
  width: 100%;
  margin: 0 20px;
}

svg {
  width: 24px;
  height: 24px;
  fill: currentColor;
}
</style>