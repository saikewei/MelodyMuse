<template>
    <TheHeader />
    <div class="music-player">
        <div class="content">
            <el-col :span="12" class="image-container">
                <img :src="currentImage ? currentImage : defaultImage" alt="Album cover" class="album-image" />
            </el-col>
            <el-col :span="12" class="lyrics-container">
                <div class="songName">
                    <span>{{ songName }}</span>
                </div>
                <div class="lyrics-wrapper">
                    <div v-for="(line, index) in lyrics"
                         :key="index"
                         :class="['lyrics-line', { 'current-line': index === currentLine }]"
                         :style="getLineStyle(index)">
                        {{ line.text }}
                    </div>
                </div>
            </el-col>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted, computed, watch } from 'vue';
    import { useStore } from 'vuex';
    import { useRoute } from 'vue-router';
    import TheHeader from '../components/TheHeader.vue';
    import api from '../api/http.js';

    const store = useStore();
    const route = useRoute();

    const songName = computed(() => store.getters.title);
    const currentImage = computed(() => store.getters.picUrl);
    const lyrics = ref([]);
    const curTime = computed(() => store.getters.curTime);
    const defaultImage = ref("/defaultImage.jpg");
    const currentLine = ref(0);
    var songId = computed(() => store.getters.id); 

    //获取歌曲信息
    async function fetchSongInfo(songId) {
        console.log('player', songId);
        try {
            const response = await api.apiClient.get(`/api/player/${songId}`);
            const songInfo = response.data;
            console.log('歌曲信息', songInfo.composerId);
            return songInfo.composerId;
        } catch (error) {
            console.error('Error fetching song info:', error);
            if (error.response && error.response.status === 401) {
                console.error('Unauthorized access. Please log in again.');
            }
            throw error;
        }
    }

    // 获取歌词的函数（需与后端交互）
    async function getLyrics(songId) {
        const artistId = await fetchSongInfo(songId);
        console.log('artist', fetchSongInfo(songId));
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

    // 在组件挂载时获取歌词
    onMounted(async () => {
        //console.log('尝试从js获取',ID.value);
        const fetchedLyrics = await getLyrics(songId.value);
        lyrics.value = parseLyrics(fetchedLyrics);
    });

    // 监听歌词的变化，重新渲染页面
    // 监听 songId 的变化，重新获取歌词
    watch(songId, async (newSongId, oldSongId) => {
        if (newSongId !== oldSongId) {
            const fetchedLyrics = await getLyrics(newSongId);
            lyrics.value = parseLyrics(fetchedLyrics);
            currentLine.value = 0;  // 重置当前行
            updateLyricDisplay(curTime.value);  // 重新显示歌词
        }
    });


    // 监听当前播放时间，更新歌词显示
    watch(curTime, (newTime) => {
        updateLyricDisplay(newTime);
    });

    // 解析歌词
    function parseLyrics(lyricsText) {
        return lyricsText.split('\n').map(line => {
            const timeMatch = line.match(/\[(\d{2}):(\d{2})\.(\d{2,3})\]/);
            if (!timeMatch) return { time: 0, text: line };

            const minutes = parseInt(timeMatch[1]);
            const seconds = parseInt(timeMatch[2]);
            const milliseconds = parseInt(timeMatch[3]);

            const time = minutes * 60 + seconds + milliseconds / 1000;
            const text = line.replace(/\[.*\]/, '').trim();

            return { time, text };
        }).filter(item => item.text);
    }

    // 获取当前行的样式
    function getLineStyle(index) {
        const distance = Math.abs(currentLine.value - index);
        const maxDistance = 3;
        const scale = Math.max(1 - distance * 0.15, 0.85);
        const opacity = Math.max(1 - distance * 0.3, 0.5);
        return {
            transform: `scale(${scale})`,
            opacity: opacity,
            display: distance > maxDistance ? 'none' : 'block',
        };
    }

    // 根据当前播放时间更新歌词
    function updateLyricDisplay(currentTime) {
        for (let i = 0; i < lyrics.value.length - 1; i++) {
            if (currentTime >= lyrics.value[i].time && currentTime < lyrics.value[i + 1].time) {
                currentLine.value = i;
                break;
            }
        }
        if (currentTime >= lyrics.value[lyrics.value.length - 1].time) {
            currentLine.value = lyrics.value.length - 1;
        }
    }
</script>



<style scoped>
    .music-player {
        background: linear-gradient(to top, #f0f0f5, #d8dff8); /* 从中心向外的渐变 */
        min-height: 94vh;
        display: flex;
        justify-content: center;
        align-items: center;
        padding: 20px;
    }

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


    .songName {
        font-size: 2.0em;
        font-weight: bold;
        color: #110ad9;
        height: 20%;
    }


    .lyrics-wrapper {
        text-align: center;
        width: 100%;
        flex-direction: column;
        justify-content: center;
        height: 60%;
    }

    .lyrics-line {
        transition: transform 0.3s ease, opacity 0.3s ease; /* 添加过渡效果 */
        margin-top: 10px;
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

    .prev-song-button {
        transform: scale(0.1); /* 缩小到原大小的一半 */
        transition: transform 0.3s ease; /* 添加平滑过渡效果 */
        width: 30px;
        height: 10px;
    }

    .next-song-button {
        transform: scale(0.1);
        transition: transform 0.3s ease; /* 添加平滑过渡效果 */
        width: 30px;
        height: 10px;
    }

    .play-pause-button {
        transform: scale(0.1);
        transition: transform 0.3s ease; /* 添加平滑过渡效果 */
        width: 30px;
        height: 10px;
    }


    .volume-slider {
        width: 200px;
        height: 0px;
        margin-left: 30px;
    }

    .play-mode-button {
        transform: scale(0.1);
        transition: transform 0.3s ease; /* 添加平滑过渡效果 */
        width: 30px;
        height: 10px;
        margin-left: 300px; /* 增加右侧的外边距 */
    }

        .play-mode-button :hover {
            transform: scale(0.9);
        }

    .play-list-button {
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
</style>