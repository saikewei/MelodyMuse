<template>
  <div class="music-player">
    <div class="content">
      <el-col :span="12" class="image-container">
        <img :src="currentImage" alt="Album cover" class="album-image" />
      </el-col>
      <el-col :span="12" class="lyrics-container">
        <div class = "songName">
          <span class = "songName">{{ songName }}</span>
        </div>
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

      <input type="range" class="progress-slider" v-model.number="progress" :max="duration"  @input="seek" />

      <span class="progress-display">{{ formatTime(progress) }} / {{ formatTime(duration) }}</span>

      <audio ref="audioElement" :src="audioSrc" @timeupdate="updateProgress" @durationchange="updateDuration" :controls="false" @ended="nextSong"></audio>
    </div>

    <div class = "control-button-warpper">
        <button class="prev-song-button" @click="prevSong" @mouseenter="prevSongSrc = '/prev-hover.png'" @mouseleave="prevSongSrc = '/prev.png'">
          <img :src="prevSongSrc" alt="Previous Song" />
        </button>

        <button class="play-pause-button" @click="togglePlaying">
          <img v-if="!playing" src="/play.jpg" alt="Play" />
          <img v-else src="/pause.jpg" alt="Pause" />
        </button>

        <button class="next-song-button" @click="nextSong"  @mouseenter="nextSongSrc = '/next-hover.png'" @mouseleave="nextSongSrc = '/next.png'">
          <img :src="nextSongSrc" alt="Next Song" />
        </button>

        <button class="play-mode-button" @click="togglePlayMode">
          <img :src="playModeSrc" alt="Play Mode" />
        </button>

        <button class="play-list-button" @click="showPlayListWindow" >播放列表</button>
        <playListWindow ref="playListWindowRef" :title="windowTitle" :content="windowContent" @close="onClose">
          <!--button @click="hidePlayListWindow">Close</button-->
          <div class="sonPlayList">
            <ul v-if="songInfoList.length > 0">
              <li v-for="(songInfo, index) in songInfoList" :key="index" :class="{ highlighted: index === songIndex }" @click="playSong(index)">
                {{ songInfo.songName }}   {{ formatTime(songInfo.duration) }}
              </li>
            </ul>
          </div>
        </playListWindow>
      </div>

      <div class="volume-slider">
        <label for="volume">音量:</label>
        <input type="range" id="volume" min="0" max="1" step="0.01" v-model="volume" @input="updateVolume">
        <span>{{ Math.round(volume * 100) }}%</span>
      </div>
  </div>
</template>

<script setup>
import { ref, onMounted ,watch} from 'vue';
import api from '../api/http.js'
import { useRouter, useRoute } from 'vue-router';
import playListWindow from './playListWindow.vue';

const route = useRoute(); 

var windowTitle = '播放列表';
var windowContent = `${route.params.songList}`;

const playListWindowRef = ref(null);

var volume = ref(0.5);
const router = useRouter(); 
const prevSongSrc = ref('/prev.png');
const nextSongSrc = ref('/next.png');
const playModeSrc = ref('/circle.png');

var audioSrc = ref('');
var lyrics = ref([]);
var albumId = ref('');
var songName = ref('');
var composerId = ref('');
var currentImage = ref('');
var currentLine = ref(0);
var audioElement = ref(null);
var progress = ref(0);
var duration = ref(0);
var playing = ref(false);
var isLoading = ref(true); //当前页面需要的数据加载状态
const songList = route.params.songList.split(',');
var songInfoList = [];
const songListLength = songList.length;
var songIndex = ref(songList.indexOf(route.params.songId));
var playmode = ref('sequence');




onMounted(async () => {
  await Promise.all([pull_song_data(route.params.songId)]);
});

const updateVolume = (event) => {
  // 更新音量值
  const newVolume = event.target.value;
  volume.value = newVolume;
  // 更新音频播放器的音量
  updateAudioPlayerVolume(newVolume);
};

// 更新音频播放器的音量
const updateAudioPlayerVolume = (newVolume) => {
  audioElement.value.volume = newVolume;
  console.log(`Volume updated to: ${newVolume}`);
};


// const hidePlayListWindow = () =>{
//   playListWindowRef.value.hide();
// }
const showPlayListWindow =async () => {
  console.log(songList)
  console.log(songInfoList)
  if(songInfoList.length == 0){
    songInfoList =  await fetch_songs_info(songList);
  }
  playListWindowRef.value.show();
};

const onClose = () => {
  console.log('Floating window closed');
};

watch(songIndex.value, (newValue, oldValue) => {
  console.log(`Highlight index changed from ${oldValue} to ${newValue}`);
});

async function pull_song_data(songId){
  try {
    // 并发获取歌曲信息
    const [songInfo] = await Promise.all([
      fetchSongInfo(songId)
    ]);
    console.log(songInfo)

    // 使用歌曲信息中的数据
    composerId.value = songInfo.composerId;
    albumId.value = songInfo.albumId;


    // 并发获取歌词和音频数据
    const [lyrics_txt, audio , image] = await Promise.all([
      fetchLyrics(songId, composerId.value),
      fetchSong(songId, composerId.value),
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
}

async function fetch_songs_info(songList){
  var songInfoLists=[];
  for(var i=0;i<songList.length;i++){
    var songInfo = await fetchSongInfo(songList[i]);
    var usedSongInfo ={
      songName: songInfo.songName,
      duration: songInfo.songDuration,
    }
    songInfoLists.push(usedSongInfo);
  }
  return songInfoLists;
}

function formatTime(seconds) {
  const minutes = Math.floor(seconds / 60);
  const remainingSeconds = Math.floor(seconds % 60);
  return `${minutes}:${remainingSeconds.toString().padStart(2, '0')}`;
}

async function fetch_img(albumId) {
  try {
    const response = await api.apiClient.get("/api/player/jpg", {
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
  const response = await api.apiClient.get("/api/player/txt",{
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
    const response = await api.apiClient.get("/api/player/mp3", {
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
function seek(event) {
  console.log(event.target.value)
  var currentTime = event.target.value;
  audioElement.value.currentTime = currentTime;
  updateProgress({ target: audioElement.value });
}


// 更新歌词显示
function updateLyricDisplay(currentTime) {
  let lineIndex = 0;
  for (let i = 0; i < lyrics.value.length; i++) {
    if (lyrics.value[i+1].time >= currentTime) {
      lineIndex = i;
      break;
    }
  }
  currentLine.value = lineIndex;
}

function togglePlayMode(){
  console.log(playmode.value)
  if(playmode.value =='sequence'){
    playmode.value = 'random';
    playModeSrc.value = '/random.png';
  }
  else if(playmode.value =='random'){
    playmode.value ='repeat';
    playModeSrc.value = '/repeat.png';
  }
  else if(playmode.value =='repeat'){
    playmode.value ='sequence';
    playModeSrc.value = '/circle.png';
  }
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

function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function resetCurrentSong(){
  currentLine.value = 0;
  audioElement.value.currentTime = 0;
  progress.value = 0;
}

async function playSong(index){
  togglePlaying();
  var oldSongId = songList[songIndex.value];
  songIndex.value = index;
  var songId = songList[index];

  if(oldSongId == songId){
  }
  else{
    await pull_song_data(songId);
  }
  togglePlaying();
}

// 播放下一首歌曲
async function nextSong() {
  if(playing.value==true){
    togglePlaying();
  }

  var oldSongId = songList[songIndex.value];
  var songId ;

  if(playmode.value == 'random'){
    songIndex.value = getRandomInt(0, songListLength-1);
    songId = songList[songIndex.value];
  }
  else if(playmode.value =='repeat'){
    songId = oldSongId;
  }
  else if(playmode.value =='sequence'){
    if(songIndex.value<songListLength-1){
      songIndex.value++;
    }
    else if(songIndex.value==songListLength-1){
      songIndex.value = 0;
    }
    songId = songList[songIndex.value];
  }
  if(songId == oldSongId){
    resetCurrentSong();
  }
  else{
    await pull_song_data(songId);
  }
  togglePlaying();

  console.log('Next song');
}

// 播放上一首歌曲
async function prevSong() {
  if(playing.value==true){
    togglePlaying();
  }
  var oldSongId = songList[songIndex.value];
  var songId ;

  if(playmode.value == 'random'){
    songIndex.value = getRandomInt(0, songListLength-1);
    songId = songList[songIndex.value];
  }
  else if(playmode.value =='repeat'){
    songId = oldSongId;
  }
  else if(playmode.value =='sequence'){
    if(songIndex.value>0){
      songIndex.value--;
    }
    else if(songIndex.value==0){
      songIndex.value = songListLength-1;
    }
    songId = songList[songIndex.value];
  }
  if(songId == oldSongId){
    resetCurrentSong();
  }
  else{
    await pull_song_data(songId);
  }
  togglePlaying();
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
  margin: 0 30px;
}

svg {
  width: 24px;
  height: 24px;
  fill: currentColor;
}

.progress-display {
  margin-left: 10px;
  font-size: 0.8em;
  color: #999;
}

.control-button-warpper {
  display: flex; /* 使用 flexbox 布局 */
  justify-content: center; /* 水平居中 */
  align-items: center; /* 垂直居中 */
}

.prev-song-button{
  transform: scale(0.1); /* 缩小到原大小的一半 */
  transition: transform 0.3s ease; /* 添加平滑过渡效果 */
  width: 30px;
  height: 10px;
}

.next-song-button{
  transform: scale(0.1);
  transition: transform 0.3s ease; /* 添加平滑过渡效果 */
  width: 30px;
  height: 10px;
}

.play-pause-button{
  transform: scale(0.1);
  transition: transform 0.3s ease; /* 添加平滑过渡效果 */
  width: 30px;
  height: 10px;
}

.play-mode-button{
  transform: scale(0.1);
  transition: transform 0.3s ease; /* 添加平滑过渡效果 */
  width: 30px;
  height: 10px;
  margin-left: 300px; /* 增加右侧的外边距 */
}

.play-mode-button :hover{
  transform: scale(0.9); 
}

.songName{
  font-size: 1.5em;
  font-weight: bold;
  color: #06dcfd;
  margin-bottom: 100px;
}

.play-list-button{
  transition: transform 0.3s ease; /* 添加平滑过渡效果 */
  width: 70px;
  height: 20px;
  margin-top: 25px;
  margin-left: 50px; /* 增加右侧的外边距 */
}

.sonPlayList {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  justify-content: flex-start;
  /* 去掉内边距 */
  padding: 0;
}

.sonPlayList ul {
  padding: 0;
  margin: 0;
}

.sonPlayList li {
  list-style: none; /* 去掉默认的列表样式 */
  margin-bottom: 5px; /* 每个列表项之间的间距 */
}

.highlighted {
  background-color: rgb(0, 255, 26); /* 你可以选择你喜欢的颜色 */
}

.volume-slider {
  position: absolute;
  display: flex;
  margin-top: 485px;
}


</style>