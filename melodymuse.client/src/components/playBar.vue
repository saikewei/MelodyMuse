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
                <div class="text">
                    <a @click="goToPlay(songId)" class="song-link">{{ songName }}</a>
                </div>
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
        <playListWindow v-if="showPlayList" @close="showPlayListWindow" />
    </div>
</template>

<script setup>
    import { ref, onMounted, computed, watch, onBeforeUnmount } from 'vue';
    import { useStore } from 'vuex';
    import { ElMessage } from 'element-plus'
    import { useRouter } from 'vue-router';
    import playListWindow from './playListWindow.vue';
    import api from '../api/http.js';

    const store = useStore();
    const router = useRouter();
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
    var progress = computed(() => store.getters.curTime);
    var duration = computed(() => store.getters.duration);
    var playing = computed(() => store.getters.isPlay);

    const songList = computed(() => store.getters.listOfSongs);
    const songListLength = computed(() => songList.value.length);
    var songId = computed(() => store.getters.id)

    var songInfoList = [];
    var songIndex = computed(() => store.getters.listIndex);
    var playmode = ref('sequence');

    const playListWindowRef = ref(null);

    // 显示播放列表窗口
    function showPlayListWindow() {
        if (!showPlayList.value)
            showPlayList.value = true;
        else
            showPlayList.value = false;
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
    watch([songId, songIndex], async ([newId, newIndex], [oldId, oldIndex]) => {
        // 检查 songId 是否发生变化
        if (newId !== oldId) {
            playSong(songId.value); // 当 songId 发生变化时播放当前索引的歌曲
            //audioElement.value.currentTime = store.getters.curTime;
        }

        // 检查 songIndex 是否发生变化
        if (newIndex !== oldIndex) {
            playSong(songId.value); // 当 songIndex 发生变化时播放相应的歌曲
            //audioElement.value.currentTime = store.getters.curTime;
        }
    }, { immediate: true });


    onMounted(async () => {
        // 加载上次播放的状态
        await Promise.all([pull_song_data(songId.value)]);
        document.addEventListener('click', handleClickOutside);
        playing = computed(() => store.getters.isPlay);
        togglePlaying()/*
        // 恢复播放时间
        audioElement.value.currentTime = store.state.curTime;

        // 如果之前是播放状态，继续播放
        if (store.getters.isPlay) {
            audioElement.value.play();
        }*/
    });
    onBeforeUnmount(() => {
        document.removeEventListener('click', handleClickOutside);
    });


    //点击播放列表播放歌曲
    async function playSong(index) {
        togglePlaying();
        await pull_song_data(songId.value);
        togglePlaying();
    }

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
            store.commit('setTitle', songInfo.songName);
            store.commit('setLyric', lyrics_txt);
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
        store.commit('setSongInfoLists', songInfoLists);
        return songInfoLists;
    }
    //获取歌曲信息
    async function fetchSongInfo(songId) {
        console.log(songId)
        try {
            const response = await api.apiClient.get(`/api/player/${songId}`);
            const songInfo = response.data;
            store.commit('setComposerId', songInfo.composerId);
            console.log('播放条', songInfo.composerId);
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
    async function fetchSong(songId, artistId, retryCount = 10) {
        try {
            console.log('songId', songId);
            const response = await api.apiClient.get("/api/player/mp3", {
                params: { 'songId': songId, 'artistId': artistId },
                responseType: 'arraybuffer' // 获取二进制数据
            });

            if (response.status === 200) {
                // 将二进制数据转换为 Blob 对象
                const blob = new Blob([response.data], { type: 'audio/mpeg' }); // 假设音频类型是 MP3
                // 创建一个指向 Blob 的 URL
                const audioUrl = URL.createObjectURL(blob);
                // 返回音频的 URL
                return audioUrl;
            } else {
                // 状态码不为200时，重试
                if (retryCount > 0) {
                    console.log(`音乐加载失败，重试中... 剩余重试次数: ${retryCount}`);
                    await new Promise((resolve) => setTimeout(resolve, 1000)); // 等待2秒
                    return fetchSong(songId, artistId, retryCount - 1); // 重试请求
                } else {
                    ElMessage({
                        message: "音乐加载失败",
                        type: "error"
                    });
                    return null;
                }
            }
        } catch (error) {
            console.log(error);
            if (retryCount > 0) {
                console.log(`请求错误，重试中... 剩余重试次数: ${retryCount}`);
                await new Promise((resolve) => setTimeout(resolve, 1000)); // 等待2秒
                return fetchSong(songId, artistId, retryCount - 1); // 重试请求
            } else {
                ElMessage({
                    message: "音乐加载失败",
                    type: "error"
                });
                return null;
            }
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
        store.commit('setCurTime', event.target.currentTime);
        // 根据当前播放时间更新歌词显示
        //updateLyricDisplay(event.target.currentTime);
    }

    // 更新音频总时长
    function updateDuration(event) {
        store.commit('setDuration', event.target.duration);
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
        console.log(playmode.value)
    }
    // 切换播放/暂停状态
    function togglePlaying() {
        if (playing.value) {
            audioElement.value.pause();
            store.commit('setIsPlay', 0);
        } else {
            // 重新设置当前进度，并从该时间点开始播放
            //audioElement.value.currentTime = store.getters.curTime;
            audioElement.value.play();
            store.commit('setIsPlay', 1);
        }
    }


    function getRandomInt(min, max) {
        return Math.floor(Math.random() * (max - min + 1)) + min;
    }

    function resetCurrentSong() {
        //currentLine.value = 0;
        audioElement.value.currentTime = 0;
        store.commit('setCurTime', 0);
    }

    // 播放下一首歌曲
    async function nextSong() {
        if (playing.value == true) {
            togglePlaying();
        }

        var oldSongId = songList.value[songIndex.value];
        var curIndex;
        var songId;

        console.log(playmode.value);

        if (playmode.value == 'random') {
            curIndex = getRandomInt(0, songListLength.value - 1);
            store.commit('setListIndex', curIndex);
            songId = songList.value[songIndex.value];
            store.commit('setId', songId);
        }
        else if (playmode.value == 'repeat') {
            songId = oldSongId;
            resetCurrentSong();
        }
        else if (playmode.value == 'sequence') {
            if (songIndex.value < songListLength.value - 1) {
                console.log(songIndex.value);
                curIndex = songIndex.value + 1;
                store.commit('setListIndex', curIndex);
                console.log(songIndex.value);
            }
            else if (songIndex.value == songListLength.value - 1) {
                curIndex = 0;
                store.commit('setListIndex', curIndex);

                console.log(songIndex.value);
            }
            songId = songList.value[songIndex.value];
            console.log('原本：', oldSongId);
            console.log('下一首：', songId);
            store.commit('setId', songId);
        }/*
        if (songId == oldSongId) {
            resetCurrentSong();
        }
        else {
            await pull_song_data(songId);
        }*/
        togglePlaying();

        console.log('Next song');
    }

    // 播放上一首歌曲
    async function prevSong() {
        if (playing.value == true) {
            togglePlaying();
        }

        var oldSongId = songList.value[songIndex.value];
        var curIndex;
        var songId;

        console.log(playmode.value);

        if (playmode.value == 'random') {
            curIndex = getRandomInt(0, songListLength.value - 1);
            store.commit('setListIndex', curIndex);
            songId = songList.value[songIndex.value];
            store.commit('setId', songId);
        }
        else if (playmode.value == 'repeat') {
            songId = oldSongId;
        }
        else if (playmode.value == 'sequence') {
            if (songIndex.value > 0) {
                console.log(songIndex.value);
                curIndex = songIndex.value - 1;
                store.commit('setListIndex', curIndex);
                console.log(songIndex.value);
            }
            else if (songIndex.value == 0) {
                curIndex = songListLength.value - 1;
                store.commit('setListIndex', curIndex);

                console.log(songIndex.value);
            }
            songId = songList.value[songIndex.value];
            console.log('原本：', oldSongId);
            console.log('下一首：', songId);
            store.commit('setId', songId);
        }/*
        if (songId == oldSongId) {
            resetCurrentSong();
        }
        else {
            await pull_song_data(songId);
        }*/
        togglePlaying();

        console.log('Prev song');
    }
    function goToPlay(song) {
        //this.$store.commit('addSongToList', song);

        // 更新当前播放的歌曲 ID
        //this.$store.commit('setId', song);
        try {
            // 使用 Vue Router 导航到播放页面，传递歌曲 ID 和相关的歌曲列表
            const songList = song;
            router.push({
                name: 'mediaplayer',
                params: {
                    songId: song, // 当前播放的歌曲 ID
                    songList: songList  // 歌曲列表的所有 songId
                }
            });
        } catch (error) {
            console.error('跳转到播放页面失败:', error);
        }
    }

</script>

<style scoped>
    .music-player-bar {
        display: flex;
        align-items: center;
        justify-content: flex-start;
        width: 100%;
        height: 70px;
        padding: 0px 45px;
        box-sizing: border-box;
        background-color: #ffffff;
        box-shadow: 0 4px 8px #cacaca;
        position: fixed;
        bottom: 0;
        left: 0;
        pointer-events: auto; /* 确保播放条可交互 */
        z-index: 8000;
    }
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
    .main-content {
        pointer-events: auto; /* 确保页面其他部分正常交互 */
    }

    .player-controls {
        display: flex;
        flex-direction: column;
        justify-content: center;
        position: fixed;
        width: 100%; /* 控制器宽度与父级相同 */
        height: 70px; /* 控制器高度 */
        padding: 10px 0;
        align-items: center;
        margin-left: -100px;
    }

    .song-name {
        margin-right: 460px;
<<<<<<< Updated upstream
        width:200px;
=======
        width: 200px;
>>>>>>> Stashed changes
    }

    .player-time {
        display: flex;
        flex-direction: row;
    }

    .play-pause-button,
    .prev-song-button,
    .next-song-button {
        background-color: transparent;
<<<<<<< Updated upstream
        position:fixed;
=======
        position: fixed;
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        left:180px;
=======
        left: 180px;
>>>>>>> Stashed changes
    }

    .next-song-button {
        transform: scale(0.08);
        transition: transform 0.3s ease; /* 添加平滑过渡效果 */
        width: 30px;
        height: 10px;
        margin-left: 10px;
<<<<<<< Updated upstream
        left:300px;
=======
        left: 300px;
>>>>>>> Stashed changes
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
        width: 28px;
        height: 13px;
        margin-left: 300px; /* 增加右侧的外边距 */
        display: flex;
        align-items: center;
        justify-content: center;
        position: fixed;
        bottom: 25px;
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
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
    .play-list-button {
        display: flex;
        align-items: center;
        justify-content: center;
        position: fixed;
        bottom: 22px;
        right: 230px;
    }
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
    .song-link {
        color: #808080;
        text-decoration: none;
        cursor: pointer;
    }

        .song-link:hover {
            text-decoration: underline;
            background-color: transparent;
        }
</style>
