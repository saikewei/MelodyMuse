<template>
    <transition name="slide-fade">
        <div class="the-aside">
            <h2 class="title">— 播放列表 —</h2>
            <ul class="menus">
                <!-- 如果 songInfoLists 为空，显示加载提示 -->
                <li v-if="songInfoLists.length === 0">加载中...</li>
                <!-- 循环显示歌曲列表 -->
                <li v-else v-for="(item, index) in songInfoLists" :key="index"
                    :class="{'is-play': id === item.id}" @click="toplay(item, index)">
                    {{ item.songName }} <!-- 显示歌曲名称 -->
                </li>
            </ul>
        </div>
    </transition>
</template>

<script>
    import { mapGetters } from 'vuex';
    import api from '../api/http.js';

    export default {
        name: 'the-aside',
        computed: {
            ...mapGetters([
                'listOfSongs',  // 当前歌曲ID列表
                'id',           // 当前播放的歌曲ID
                'songInfoLists' // 歌曲详细信息列表
            ])
        },
        mounted() {
            // 初始化时获取歌曲信息
            this.getInfo();
        },
        watch: {
            // 监听 listOfSongs 变化，重新获取歌曲信息
            listOfSongs: {
                handler(newVal, oldVal) {
                    if (newVal !== oldVal) {
                        this.getInfo();  // 当 listOfSongs 变化时刷新歌曲信息
                    }
                },
                immediate: true // 确保组件挂载时立即执行 handler
            },
            songInfoLists: {
                handler(newVal, oldVal) {
                    if (newVal !== oldVal) {
                        this.getInfo();  // 当 listOfSongs 变化时刷新歌曲信息
                    }
                },
                immediate: true // 确保组件挂载时立即执行 handler
            }
        },

        methods: {
            // 播放
            toplay(item, index) {
                // 更新当前播放的歌曲ID
                this.$store.commit('setId', item.id);
                // 更新当前播放的歌曲索引
                this.$store.commit('setListIndex', index);
                // 其他播放相关逻辑
            },
            // 异步获取歌曲信息
            async fetchSongInfo(songId) {
                try {
                    const response = await api.apiClient.get(`/api/player/${songId}`);
                    return response.data;  // 假设返回的 JSON 中包含 `songName` 和其他信息
                } catch (error) {
                    console.error('Error fetching song info:', error);
                    return null;  // 如果有错误，返回 null
                }
            },
            // 获取歌曲信息列表
            async getInfo() {
                let songInfoLists = [];

                console.log('当前 listOfSongs: ', JSON.stringify(this.listOfSongs.slice()));

                // 获取歌曲ID列表
                let songList = this.listOfSongs.slice();

                // 遍历歌曲ID列表并获取每首歌的信息
                for (let i = 0; i < songList.length; i++) {
                    let songId = songList[i];
                    let songInfo = await this.fetchSongInfo(songId);  // 使用 async/await 获取每首歌的信息

                    if (songInfo) {  // 只有在成功获取信息时才添加到列表中
                        let usedSongInfo = {
                            id: songId,  // 添加 ID 以便识别当前播放的歌曲
                            songName: songInfo.songName || '未知歌曲',
                            duration: songInfo.songDuration || '未知时长',
                        };
                        songInfoLists.push(usedSongInfo);
                    }
                }

                // 将获取到的歌曲信息存入 Vuex 状态管理
                this.$store.commit('setSongInfoLists', songInfoLists);
            },
        }
    }
</script>

<style scoped>
    /* 播放列表的容器 */
    .the-aside {
        position: fixed;
        right: 100px; /* 控制组件在页面上的位置 */
        bottom: 80px; /* 控制组件在页面上的位置 */
        width: 300px; /* 固定宽度 */
        height: 400px; /* 固定高度 */
        background-color: rgba(64, 108, 194, 0.9);
        overflow-y: auto; /* 使内容超出时可以纵向滚动 */
        color: white; /* 设置文本颜色为白色 */
        padding: 10px;
        border-radius: 8px; /* 设置圆角 */
    }

    /* 歌曲列表的样式 */
    .menus {
        list-style: none; /* 去掉默认的列表样式 */
        padding: 0;
        margin: 0;
    }

        /* 单个歌曲项的样式 */
        .menus li {
            padding: 10px;
            cursor: pointer;
            background-color: rgba(255, 255, 255, 0.1); /* 使背景稍微透明 */
            margin-bottom: 5px; /* 每个歌曲项之间留出空间 */
            border-radius: 4px;
            transition: background-color 0.3s; /* 添加过渡效果 */
        }

            /* 当前播放歌曲的样式 */
            .menus li.is-play {
                background-color: rgba(255, 255, 255, 0.3); /* 当前播放的歌曲背景色更亮 */
                font-weight: bold;
            }

            /* 鼠标悬停歌曲项时的效果 */
            .menus li:hover {
                background-color: rgba(255, 255, 255, 0.2);
            }
</style>
