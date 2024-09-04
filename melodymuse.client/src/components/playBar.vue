<template>
    <div class="music-player-bar">
        <!--播放控件-->
        <div class="control-button-warpper">
            <button class="prev-song-button" @click="prevSong" @mouseenter="prevSongSrc = '/prev-hover.png'" @mouseleave="prevSongSrc = '/prev.png'">
                <img :src="prevSongSrc" alt="Previous Song" />
            </button>
            <button class="play-pause-button" @click="togglePlaying">
                <img v-if="!playing" src="/play.png" alt="Play" />
                <img v-else src="/pause.png" alt="Pause" />
            </button>
            <button class="next-song-button" @click="nextSong" @mouseenter="nextSongSrc = '/next-hover.png'" @mouseleave="nextSongSrc = '/next.png'">
                <img :src="nextSongSrc" alt="Next Song" />
            </button>

        </div>
        <!--进度条-->
        <div class="player-controls">
            <div class="song-name">
                <span>{{ songName }}</span>
            </div>
            <div class="player-time">
                <input type="range" class="progress-slider" v-model.number="progress" :max="duration" @input="seek" />
                <span class="progress-display">{{ formatTime(progress) }} / {{ formatTime(duration) }}</span>
            </div>
            <audio ref="audioElement" :src="audioSrc" @timeupdate="updateProgress" @durationchange="updateDuration" :controls="false" @ended="nextSong"></audio>
        </div>
        <!--音量-->
        <button class="play-mode-button" @click="togglePlayMode">
            <img :src="playModeSrc" alt="Play Mode" />
        </button>
        <div class="volume-slider">
            <!-- 垂直音量控制条 -->
            <div class="volume-control-wrapper" v-if="showVolumeControl">
                <input type="range" id="volume" min="0" max="1" step="0.01" v-model="volume" @input="updateVolume" class="volume-slider-vertical">
            </div>
            <div>
                <img src="/volume.png"
                     @click="toggleVolumeControl"
                     class="volume-icon"
                     alt="音量" />
            </div>
            <button class="play-list-button" @click="showPlayListWindow">播放列表</button>
        </div>
        <!-- 播放列表窗口 -->
        <playListWindow v-if="showPlayList" @close="closePlayListWindow" />
    </div>
</template>

<script setup>
    import { ref, onMounted, computed, watch, onBeforeUnmount } from 'vue';
    import { useStore } from 'vuex';
    import { ElMessage } from 'element-plus'
    import playListWindow from './playListWindow.vue';
    import api from '../api/http.js';

    const store = useStore();
    const volume = ref(store.state.volume);
    const showVolumeControl = ref(false);
    const prevSongSrc = ref('/prev.png');
    const nextSongSrc = ref('/next.png');
    const playModeSrc = ref('/circle.png');
    const audioSrc = ref('');
    var albumId = ref('');
    var songName = ref('');
    var composerId = ref('');
    var audioElement = ref(null);
    const showPlayList = ref(false);  // 控制播放列表窗口的显示状态

    //待连js
    var progress = ref(0);
    var duration = ref(0);
    var playing = ref(false);

    const songList = computed(() => store.getters.listOfSongs);
    const songListLength = computed(() => songList.value.length);
    var songId = computed(() => store.getters.id)

    var songInfoList = [];
    var songIndex = computed(() => store.getters.listIndex);
    var playmode = ref('sequence');

    const playListWindowRef = ref(null);

    // 显示播放列表窗口
    function showPlayListWindow() {
        showPlayList.value = true;
    }

    // 关闭播放列表窗口
    function closePlayListWindow() {
        showPlayList.value = false;
    }


    const updateVolume = (event) => {
        // 更新音量值
        const newVolume = event.target.value;
        volume.value = newVolume;
        store.commit('setVolume', newVolume);
        // 更新音频播放器的音量
        updateAudioPlayerVolume(newVolume);
    };

    // 更新音频播放器的音量
    const updateAudioPlayerVolume = (newVolume) => {
        audioElement.value.volume = newVolume;
        console.log(`Volume updated to: ${newVolume}`);
    };

    const toggleVolumeControl = () => {
        showVolumeControl.value = !showVolumeControl.value;
    };

    const handleClickOutside = (event) => {
        if (!event.target.closest('.volume-control-wrapper') && !event.target.closest('.volume-icon')) {
            showVolumeControl.value = false;
        }
    };

    watch(songList, async (newList, oldList) => {
        // 检查播放列表是否发生了变化
        if (newList !== oldList) {
            // 如果当前播放的歌曲已不在新列表中，可以选择播放新的歌曲，或者保持当前状态
            if (!newList.includes(songId.value)) {
                // 停止当前播放的歌曲并清空播放条信息
                resetCurrentSong();
                songName.value = '';
                audioSrc.value = '';
            } else {
                // 如果当前歌曲仍在列表中，可能需要重新拉取歌曲数据
                songId = computed(() => store.getters.id);
                await pull_song_data(songId.value);
            }
        }
    }, { immediate: true }); // immediate确保在组件挂载时也能触发watch

    onMounted(async () => {
        await Promise.all([pull_song_data(songId.value)]);
        document.addEventListener('click', handleClickOutside);
    });
    onBeforeUnmount(() => {
        document.removeEventListener('click', handleClickOutside);
    });

    async function pull_song_data(songId) {
        try {
            // 并发获取歌曲信息
            const [songInfo] = await Promise.all([
                fetchSongInfo(songId)
            ]);
            console.log(songInfo)

            // 使用歌曲信息中的数据
            composerId.value = songInfo.composerId;
            console.log(songInfo.albumId)
            albumId.value = songInfo.albumId;


            // 并发获取歌词和音频数据
            const [lyrics_txt, audio, image] = await Promise.all([
                fetchLyrics(songId, composerId.value),
                fetchSong(songId, composerId.value),
                fetch_img(albumId.value)
            ]);

            // 更新数据
            songName.value = songInfo.songName;
            //lyrics.value = parseLyrics(lyrics_txt);
            audioSrc.value = audio;
            //currentImage.value = image;

            // 设置加载状态为 false
            //isLoading.value = false;

            // 开始更新进度条
            const update = () => {
                updateProgress({ target: audioElement.value });
                requestAnimationFrame(update);
            };
            requestAnimationFrame(update);
        } catch (error) {
            console.error('Error loading data:', error);
            // 处理错误情况，例如显示错误消息
            //isLoading.value = false; // 数据加载失败后也设置为 false
        }
    }

    //时间格式转换
    function formatTime(seconds) {
        const minutes = Math.floor(seconds / 60);
        const remainingSeconds = Math.floor(seconds % 60);
        return `${minutes}:${remainingSeconds.toString().padStart(2, '0')}`;
    }
    //获取播放列表信息
    async function fetch_songs_info(songList) {
        var songInfoLists = [];
        for (var i = 0; i < songListLength.value; i++) {
            var songInfo = await fetchSongInfo(songList[i]);
            var usedSongInfo = {
                songName: songInfo.songName,
                duration: songInfo.songDuration,
            }
            songInfoLists.push(usedSongInfo);
        }
        return songInfoLists;
    }
    //获取歌曲信息
    async function fetchSongInfo(songId) {
        console.log(songId)
        try {
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
    //从后端获取歌词
    async function fetchLyrics(songId, artistId) {
        try {
            const response = await api.apiClient.get("/api/player/txt", {
                params: {
                    songId: songId,
                    artistId: artistId
                }
            });

            let lyrics;
            if (response.status === 200) {
                lyrics = response.data;
            } else {
                lyrics = '[00:00.00] 获取歌词失败';
            }

            // 将获取到的歌词传递到 song.js 中
            store.commit('setLyric', lyrics);

            return lyrics;

        } catch (error) {
            console.log(error);
            const errorLyrics = '[00:00.00] 获取歌词失败';

            // 在发生错误时也将错误提示传递到 song.js 中
            store.commit('setLyric', errorLyrics);

            return errorLyrics;
        }
    }
    //从后端获取音源
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
                ElMessage({
                    message: "音乐加载失败",
                    type: "error"
                })
                return null;
            }
        } catch (error) {
            console.log(error);
            ElMessage({
                message: "音乐加载失败",
                type: "error"
            })
            return null;
        }
    }
    //从后端获取图片
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

            // 将图片 URL 传给 Vuex 的 state
            store.commit('setPicUrl', imageUrl);
            // 将 URL 赋值给 currentImage
            return imageUrl;
        } catch (error) {
            console.log(error);
        }
    }

    // 更新音频播放进度
    function updateProgress(event) {
        progress.value = event.target.currentTime;

        // 根据当前播放时间更新歌词显示
        //updateLyricDisplay(event.target.currentTime);
    }

    // 更新音频总时长
    function updateDuration(event) {
        duration.value = event.target.duration;
        store.commit('setDuration', duration.value);
    }

    // 跳转到指定时间
    function seek(event) {
        console.log(event.target.value)
        var currentTime = event.target.value;
        audioElement.value.currentTime = currentTime;
        updateProgress({ target: audioElement.value });
        store.commit('setCurTime', currentTime);
    }

    //切换播放顺序
    function togglePlayMode() {
        console.log(playmode.value)
        if (playmode.value == 'sequence') {
            playmode.value = 'random';
            playModeSrc.value = '/random.png';
        }
        else if (playmode.value == 'random') {
            playmode.value = 'repeat';
            playModeSrc.value = '/repeat.png';
        }
        else if (playmode.value == 'repeat') {
            playmode.value = 'sequence';
            playModeSrc.value = '/circle.png';
        }
    }
    // 切换播放/暂停状态
    function togglePlaying() {
        if (playing.value) {
            audioElement.value.pause();
            store.commit('setIsPlay', 0);
        } else {
            audioElement.value.play();
            store.commit('setIsPlay', 1);
        }
        playing.value = !playing.value;
    }

    function getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    function resetCurrentSong() {
        //currentLine.value = 0;
        audioElement.value.currentTime = 0;
        progress.value = 0;
    }
    //点击播放列表播放歌曲
    /*async function playSong(index) {
        togglePlaying();
        var oldSongId = songList[songIndex.value];
        songIndex.value = index;
        var songId = songList[index];

        if (oldSongId == songId) {
        }
        else {
            await pull_song_data(songId);
        }
        togglePlaying();
    }*/

    // 播放下一首歌曲
    async function nextSong() {
        if (playing.value == true) {
            togglePlaying();
        }

        var oldSongId = songList[songIndex.value];
        var curIndex;
        var songId;

        if (playmode.value == 'random') {
            curIndex = getRandomInt(0, songListLength.value - 1);
            store.commit('setListIndex', curIndex);
            songIndex = computed(() => store.getters.listIndex);
            songId = songList[songIndex.value];
        }
        else if (playmode.value == 'repeat') {
            songId = oldSongId;
        }
        else if (playmode.value == 'sequence') {
            if (songIndex.value < songListLength.value - 1) {
                curIndex = songIndex.value + 1;
                store.commit('setListIndex', curIndex);
                songIndex = computed(() => store.getters.listIndex);
            }
            else if (songIndex.value == songListLength.value - 1) {
                curIndex = 0;
                store.commit('setListIndex', curIndex);
                songIndex = computed(() => store.getters.listIndex);
            }
            songId = songList[songIndex.value];
        }
        if (songId == oldSongId) {
            resetCurrentSong();
        }
        else {
            await pull_song_data(songId);
        }
        togglePlaying();

        console.log('Next song');
    }

    // 播放上一首歌曲
    async function prevSong() {
        if (playing.value == true) {
            togglePlaying();
        }
        var oldSongId = songList[songIndex.value];
        var curIndex;
        var songId;

        if (playmode.value == 'random') {
            curIndex = getRandomInt(0, songListLength.value - 1);
            store.commit('setListIndex', curIndex);
            songIndex = computed(() => store.getters.listIndex);
            songId = songList[songIndex.value];
        }
        else if (playmode.value == 'repeat') {
            songId = oldSongId;
        }
        else if (playmode.value == 'sequence') {
            if (songIndex.value < songListLength.value - 1) {
                curIndex = songIndex.value - 1;
                store.commit('setListIndex', curIndex);
                songIndex = computed(() => store.getters.listIndex);
            }
            else if (songIndex.value == songListLength.value - 1) {
                curIndex = 0;
                store.commit('setListIndex', curIndex);
                songIndex = computed(() => store.getters.listIndex);
            }
            songId = songList[songIndex.value];
        }
        if (songId == oldSongId) {
            resetCurrentSong();
        }
        else {
            await pull_song_data(songId);
        }
        togglePlaying();
        console.log('Previous song');
    }


</script>

<style scoped>
    .music-player-bar {
        display: flex;
        align-items: center;
        justify-content: space-between;
        width: 100%;
        height: 70px;
        padding: 0px 45px;
        box-sizing: border-box;
        background-color: #ffffff;
        box-shadow: 0 4px 8px #cacaca;
        position: fixed;
        bottom: 0;
        left: 0;
        z-index: 0;
    }

    .player-controls {
        display: flex;
        flex-direction: column;
        justify-content: center;
        position: fixed;
        width: 100%; /* 控制器宽度与父级相同 */
        height: 100%; /* 控制器高度 */
        padding: 10px 0;
        align-items: center;
        margin-left: -100px;
    }

    .song-name {
        margin-right: 550px;
    }

    .player-time {
        display: flex;
        flex-direction: row;
    }

    .play-pause-button,
    .prev-song-button,
    .next-song-button {
        background-color: transparent;
        position:fixed;
        border: none;
        cursor: pointer;
        margin: 0 10px;
        align-items: center;
        margin-bottom: 15px;
        z-index: 2000;
    }

    .control-button-warpper {
        display: flex; /* 使用 flexbox 布局 */
        justify-content: center; /* 水平居中 */
        align-items: center; /* 垂直居中 */
    }

    .prev-song-button {
        transform: scale(0.08); /* 缩小到原大小的一半 */
        transition: transform 0.3s ease; /* 添加平滑过渡效果 */
        width: 30px;
        height: 10px;
        margin-right: 12px;
        left:180px;
    }

    .next-song-button {
        transform: scale(0.08);
        transition: transform 0.3s ease; /* 添加平滑过渡效果 */
        width: 30px;
        height: 10px;
        margin-left: 10px;
        left:300px;
    }

    .play-pause-button {
        transform: scale(0.08);
        transition: transform 0.3s ease; /* 添加平滑过渡效果 */
        width: 30px;
        height: 10px;
        left: 240px;
    }

    .play-mode-button {
        transform: scale(0.08);
        transition: transform 0.3s ease; /* 添加平滑过渡效果 */
        width: 20px;
        height: 16px;
        margin-left: 300px; /* 增加右侧的外边距 */
        display: flex;
        align-items: center;
        justify-content: center;
        position: fixed;
        bottom: 22px;
        right: 400px;
    }

    .progress-slider {
        width: 600px;
        margin: 0 30px;
    }

    .volume-slider {
        display: flex;
        flex-direction: column;
        align-items: center;
        position: absolute;
        bottom: 7px;
        right: 325px;
        width: 50px;
    }

    .volume-control-wrapper {
        display: flex;
        align-items: center;
        justify-content: center;
        margin-bottom: 60px; /* 控制音量条和图标之间的间距 */
    }

    .volume-slider-vertical {
        width: 130px; /* 控制音量条的长度 */
        transform: rotate(-90deg); /* 将音量条垂直旋转 */
        transform-origin: center;
        background: #1db954;
        border-radius: 5px;
        cursor: pointer;
    }

    .volume-icon {
        width: 38px;
        height: 37px;
        cursor: pointer;
    }

        .volume-icon:hover {
            width: 40px;
            height: 39px;
        }
    .play-list-button {
        display: flex;
        align-items: center;
        justify-content: center;
        position: fixed;
        bottom: 22px;
        right: 230px;
    }
</style>
