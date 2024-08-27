<template>
    <div class="recommended-songs">
        <h2>推荐以下歌曲</h2>
        <hr class="separator-line" />
        <div class="songs-list">
            <div v-for="song in displayedSongs" :key="song.SongId" class="song-card">
                <a :href="'/song/' + song.SongId" class="song-link">
                    <img :src="song.coverUrl" alt="Song Cover" class="song-cover" />
                    <div class="song-info">
                        <h3>{{ song.songName }}</h3>
                        <p>{{ song.artists.map(artist => artist.artistName).join(', ') }}</p>
                    </div>
                </a>
                <!-- 新增操作按钮 -->
                <div class="song-actions">
                    <el-tooltip content="播放歌曲" placement="bottom">
                        <img :src="song.playing ? playClickedIcon : song.playHover ? playHoverIcon : playIcon"
                             @mouseover="song.playHover = true"
                             @mouseleave="song.playHover = false"
                             @click="togglePlayIcon(song)"
                             class="play-icon"
                             alt="播放歌曲" />
                    </el-tooltip>
                    <el-tooltip content="收藏歌曲" placement="bottom">
                        <img :src="song.liked ? likeClickedIcon : song.likeHover ? likeHoverIcon : likeIcon"
                             @mouseover="song.likeHover = true"
                             @mouseleave="song.likeHover = false"
                             @click="toggleLikeIcon(song)"
                             class="icon"
                             alt="收藏歌曲" />
                    </el-tooltip>
                    <el-tooltip content="添加到播放列表" placement="bottom">
                        <img :src="song.added ? addClickedIcon : song.addHover ? addHoverIcon : addIcon"
                             @mouseover="song.addHover = true"
                             @mouseleave="song.addHover = false"
                             @click="toggleAddIcon(song)"
                             class="icon"
                             alt="添加到播放列表" />
                    </el-tooltip>
                </div>
            </div>
        </div>
        <div class="pagination">
            <button v-if="!showAll" @click="showAllSongs">查看更多</button>
        </div>
    </div>
</template>

<script>
    import api from '../api/http.js';
    import playIcon from '../assets/pics/play.png';
    import playClickedIcon from '../assets/pics/play-click.png';
    import playHoverIcon from '../assets/pics/play-cover.png';
    import likeIcon from '../assets/pics/like.png';
    import likeHoverIcon from '../assets/pics/like-cover.png';
    import likeClickedIcon from '../assets/pics/like-click.png';
    import addIcon from '../assets/pics/add.png';
    import addHoverIcon from '../assets/pics/add-cover.png';
    import addClickedIcon from '../assets/pics/add-click.png';

    export default {
        data() {
            return {
                songs: [], // 推荐歌曲列表
                userId: null, // 用户ID
                showAll: false, // 是否显示所有歌曲
                playIcon,
                playClickedIcon,
                playHoverIcon,
                likeIcon,
                likeHoverIcon,
                likeClickedIcon,
                addIcon,
                addHoverIcon,
                addClickedIcon,
            };
        },
        computed: {
            displayedSongs() {
                return this.showAll ? this.songs : this.songs.slice(0, 8);
            }
        },
        async created() {
            try {
                // 获取用户ID
                const userResponse = await api.apiClient.get('/api/users/getNow');
                this.userId = userResponse.data.userId;

                // 根据用户ID获取推荐歌曲
                const response = await api.apiClient.get(`/api/recommend/${this.userId}`);
                this.songs = response.data;

            } catch (error) {
                console.error('Error fetching recommended songs:', error);
            }
        },
        methods: {
            showAllSongs() {
                this.showAll = true;
            },
            togglePlayIcon(song) {
                song.playing = !song.playing;
            },
            toggleLikeIcon(song) {
                song.liked = !song.liked;
            },
            toggleAddIcon(song) {
                song.added = !song.added;
            }
        }
    };
</script>

<style scoped>
    .recommended-songs {
        margin: 20px;
    }

    h2 {
        margin-bottom: 10px;
        font-size: 24px;
    }

    .separator-line {
        border: none;
        border-top: 4px solid rgba(64, 108, 194, 0.9); /* 实线分隔 */
        margin-bottom: 20px;
    }

    .songs-list {
        display: grid;
        grid-template-columns: repeat(4, minmax(0, 1fr)); /* 每行显示4个，减小间距 */
        gap: 20px;
    }

    .song-card {
        text-align: center;
        background-color: #f9f9f9;
        border-radius: 8px;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        transition: transform 0.2s;
        display: flex;
        flex-direction: column;
        align-items: center; /* 垂直居中 */
    }

        .song-card:hover {
            transform: translateY(-5px);
        }

    .song-link {
        text-decoration: none;
        color: inherit; /* 保持原始颜色 */
        display: flex;
        flex-direction: column;
        align-items: center;
    }

        .song-link:hover .song-info h3,
        .song-link:hover .song-info p {
            text-decoration: underline; /* 悬停时添加下划线 */
            color: inherit; /* 保持原始颜色 */
        }

    .song-cover {
        width: 100%;
        height: 150px; /* 保持正方形 */
        object-fit: cover;
        border-radius: 8px 8px 0 0;
    }

    .song-info {
        padding: 10px;
        text-align: center;
    }

    .song-actions {
        display: flex;
        justify-content: center;
        margin-top: 10px;
    }

    .play-icon {
        width: 30px;
        height: 25px;
        margin-right: 8px;
        cursor: pointer;
        vertical-align: middle;
        margin-bottom: 3px;
    }

    .icon {
        width: 28px;
        height: 25px;
        margin-right: 8px;
        cursor: pointer;
        vertical-align: middle;
    }

    .pagination {
        display: flex;
        justify-content: center;
        align-items: center;
        margin-top: 20px;
    }

        .pagination button {
            padding: 8px 16px;
            margin-left: 10px;
            border-radius: 20px;
            border: none;
            background-color: rgba(64, 108, 194, 0.9);
            color: white;
            cursor: pointer;
        }

            .pagination button:disabled {
                background-color: #ccc;
                cursor: not-allowed;
            }

        .pagination span {
            font-weight: bold;
        }
</style>
