<template>
  <div class="player-page">
    <div class="content">
      <div class="record">
        <img :src="track.cover_url || 'default-cover.jpg'" class="album-cover">
      </div>
      <div class="lyrics-container" @mousewheel.prevent="handleMouseWheel" @click="handleLyricsClick" ref="lyricsContainer">
        <p v-for="(line, index) in lyrics" :key="index" :class="{ active: index === currentLine }">
          {{ line.text }}
        </p>
      </div>
    </div>
  </div>
 </template>



<script>
export default {
 props: ['track', 'audio', 'lyrics'],
 data() {
   return {
     currentLine: 0,
     isPlaying: false,
     lastWheelTime: Date.now(),
     mouseInLyrics: false,
     autoScroll: true,
     mouseWheelTimeout: null,
     userScrolling: false, // 新增标志位，用来标记用户是否正在滚动
   };
 },
 mounted() {
   this.audio.addEventListener('play', this.onPlay);
   this.audio.addEventListener('pause', this.onPause);
   this.audio.addEventListener('timeupdate', this.onTimeUpdate);
   // 监听鼠标滚轮事件
   this.$refs.lyricsContainer.addEventListener('wheel', this.handleMouseWheel);
 },
 beforeDestroy() {
   this.audio.removeEventListener('play', this.onPlay);
   this.audio.removeEventListener('pause', this.onPause);
   this.audio.removeEventListener('timeupdate', this.onTimeUpdate);
   clearTimeout(this.mouseWheelTimeout); // 清除可能存在的定时器
   // 移除鼠标滚轮事件监听
   this.$refs.lyricsContainer.removeEventListener('wheel', this.handleMouseWheel);
 },
 methods: {
   onPlay() {
     this.isPlaying = true;
   },
   onPause() {
     this.isPlaying = false;
   },
   onTimeUpdate() {
     if (!this.autoScroll || this.userScrolling) return;
     const currentTime = this.audio.currentTime;
     const nextLine = this.lyrics.findIndex(line => line.time > currentTime);
     this.currentLine = nextLine === -1 ? this.lyrics.length - 1 : nextLine - 1;
     this.scrollToActiveLine();
   },
   handleMouseWheel() {
     this.userScrolling = true; // 用户开始滚动
     this.autoScroll = false; // 停止自动滚动
 
     clearTimeout(this.mouseWheelTimeout);
     this.mouseWheelTimeout = setTimeout(() => {
       this.userScrolling = false; // 用户停止滚动
       if (Date.now() - this.lastWheelTime > 2000) {
         this.autoScroll = true; // 超过2秒无操作，恢复自动滚动
         this.scrollToActiveLine();
       }
     }, 2000);
 
     this.lastWheelTime = Date.now();
   },
   scrollToActiveLine() {
     const lyricsContainer = this.$refs.lyricsContainer;
     if (lyricsContainer && !this.userScrolling) { // 只有当用户没有在滚动时才执行滚动
       const activeLineElement = lyricsContainer.children[this.currentLine];
       if (activeLineElement) {
         const scrollTop = activeLineElement.offsetTop - lyricsContainer.offsetTop - (lyricsContainer.clientHeight / 2) + (activeLineElement.clientHeight / 2);
         requestAnimationFrame(() => {
           lyricsContainer.scrollTop = scrollTop;
         });
       }
     }
   },
   handleLyricsClick(event) {
     // 获取点击的歌词行
     const lineIndex = Array.from(this.$refs.lyricsContainer.children).findIndex(p => p.contains(event.target));
     if (lineIndex === -1) return;
 
     // 设置音频的当前播放时间
     const clickedLine = this.lyrics[lineIndex];
     if (clickedLine && clickedLine.time) {
       this.audio.currentTime = clickedLine.time;
       this.currentLine = lineIndex; // 更新当前行
       this.scrollToActiveLine(); // 滚动到激活的歌词行
     }
   },
   
 },
 watch: {
   isPlaying(newVal) {
     if (newVal && this.mouseInLyrics && Date.now() - this.lastWheelTime > 3000) {
       this.autoScroll = true;
       this.scrollToActiveLine();
     }
   },
 },
};
</script>
  
 <style scoped>
 .player-page {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 20px;
  height: 100vh;
 }
  
 .content {
  display: flex;
  align-items: center;
  justify-content: center;
  width: 80%;
 }
  
 .record {
  margin-right: 20px;
  flex-shrink: 0;
  cursor: pointer;
  margin-bottom: 50px;
  margin-right: 20px; /* 保持与歌词的间距 */
 flex-shrink: 0; /* 防止在缩放时改变大小 */
 }
  
 .album-cover {
  width: 300px;
  height: 300px;
  object-fit: cover;
  border-radius: 15px;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
 }
  
 .lyrics-container {
 height: 200px; 
 overflow-y: auto;
 margin-top: 20px;
 text-align: left;
 flex-grow: 1; /* 让歌词容器占据剩余空间 */
 align-self: stretch; /* 让歌词容器填满其父容器的垂直空间 */
 transition: transform 0.3s ease-out;
}
 
.lyrics-container p {
 margin: 0;
 padding: 5px;
 transition: opacity 0.3s ease-out;
}
 
.lyrics-container p.active {
 font-weight: bold;
 font-size: 1.2em;
}

</style>

