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
import { ref, onMounted } from 'vue';


import api from '../api/http.js'
import { useRouter, useRoute } from 'vue-router';
const router = useRouter(); 


const route = useRoute(); 
const audioSrc = ref('');
const lyrics = ref([]);
const albumId = ref('');
const songName = ref('');
const composerId = ref('');
const currentImage = ref('');
const currentLine = ref(0);
const audioElement = ref(null);
const progress = ref(0);
const duration = ref(0);
const playing = ref(false);
const isLoading = ref(true); // 新增状态，用于表示数据是否正在加载


onMounted(async () => {
  try {
    // 并发获取歌曲信息
    const [songInfo] = await Promise.all([
      fetchSongInfo(route.params.songId)
    ]);
    console.log(songInfo)

    // 使用歌曲信息中的数据
    composerId.value = songInfo.composerId;
    albumId.value = songInfo.albumId;


    // 并发获取歌词和音频数据
    const [lyrics_txt, audio , image] = await Promise.all([
      fetchLyrics(route.params.songId, composerId.value),
      fetchSong(route.params.songId, composerId.value),
      fetch_img(albumId.value)
    ]);

    // 更新数据
    songName.value = songInfo.songName;
    lyrics.value = parseLyrics(lyrics_txt);
    audioSrc.value = audio;
    currentImage.value =  image;

    // 设置加载状态为 false
    isLoading.value = false;

    // 开始更新进度条
    const update = () => {
      updateProgress({ target: audioElement.value });
      requestAnimationFrame(update);
    };
    requestAnimationFrame(update);
  } catch (error) {
    console.error('Error loading data:', error);
    // 处理错误情况，例如显示错误消息
    isLoading.value = false; // 数据加载失败后也设置为 false
  }
});


async function fetch_img(albumId) {
  try {
    const response = await api.apiClientWithoutToken.get("/api/player/jpg", {
      params: { 'albumId': albumId },
      responseType: 'arraybuffer'  // 关键：将响应类型设为 arraybuffer
    });

    // 将响应的二进制数据转换为 Blob 对象
    const blob = new Blob([response.data], { type: 'image/jpeg' });  // 假设图片是 JPEG 格式

    // 创建一个指向 Blob 的 URL
    const imageUrl = URL.createObjectURL(blob);

    // 将 URL 赋值给 currentImage
    return imageUrl;
  } catch (error) {
    console.log(error);
    return null;
  }
}


async function fetchLyrics(songId,artistId){
  try{
    const formData = new FormData();
    formData.append('songId', songId);
    formData.append('artistId', artistId);
  const response = await api.apiClientWithoutToken.get("/api/player/txt",{
    params:{'songId' : songId,
    'artistId' : artistId}
  });

  if(response.status === 200)
  {
  return response.data;
  }
  else{
    return '[00:00.00] 获取歌词失败'
  }

}catch(error){
  console.log(error)
}
}

async function fetchSong(songId, artistId) {
  try {
    const response = await api.apiClientWithoutToken.get("/api/player/mp3", {
      params: { 'songId': songId, 'artistId': artistId },
      responseType: 'arraybuffer'  // 关键：将响应类型设为 arraybuffer 以获取二进制数据
    });

    if (response.status === 200) {
      // 将二进制数据转换为 Blob 对象
      const blob = new Blob([response.data], { type: 'audio/mpeg' });  // 假设音频类型是 MP3

      // 创建一个指向 Blob 的 URL
      const audioUrl = URL.createObjectURL(blob);

      // 返回音频的 URL
      return audioUrl;
    } else {
      return null;
    }
  } catch (error) {
    console.log(error);
    return null;
  }
}


// 解析歌词
function parseLyrics(lyricsText) {
  return lyricsText.split('\n').flatMap(function (line) {
    const results = [];

    let index = 0;
    while (index < line.length) {
      // 查找左括号的位置
      const leftBracketIndex = line.indexOf('[', index);
      if (leftBracketIndex === -1) break; // 如果没有找到左括号，结束循环
      
      // 查找右括号的位置
      const rightBracketIndex = line.indexOf(']', leftBracketIndex);
      if (rightBracketIndex === -1) break; // 如果没有找到右括号，结束循环
      
      // 提取时间戳
      const timeStr = line.substring(leftBracketIndex + 1, rightBracketIndex);
      const minutes = parseInt(timeStr.substring(0, 2), 10);
      const seconds = parseFloat(timeStr.substring(3));
      const timestamp = minutes * 60 + seconds;
      
      // 提取时间戳后面的文本
      const textStartIndex = rightBracketIndex + 1;
      const text = line.substring(textStartIndex).trim();
      
      results.push({ time: timestamp, text: text });

      // 更新索引
      index = textStartIndex;
    }

    return results;
  }).filter(Boolean);
}

async function fetchSongInfo(songId) {
  console.log(songId)
  try {
    //const response = await axios.get(`https://localhost:7223/api/player/${songId}`);
    const response = await api.apiClient.get(`/api/player/${songId}`);
    const songInfo = response.data;
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