<template>
    <el-dialog v-model="dialogAddVisible" width="500px" v-if="dialogAddVisible">
        <AddToSongList :songId="currentSongId" :dialogVisible="dialogAddVisible" @update:dialogVisible="handleDialogClose" />
    </el-dialog>
    <div class="recommended-songs">
        <!-- 常听流派推荐歌曲 -->
        <h2>根据常听流派推荐以下歌曲</h2>
        <hr class="separator-line" />
        <div class="songs-list">
            <div v-for="song in displayedGenreSongs" :key="song.SongId" class="song-card">
            
                    <img :src="song.coverUrl" alt="Song Cover" class="song-cover" />
                    <div class="song-info">
                        <h3>{{ song.songName }}</h3>
                        <p>{{ song.artists.map(artist => artist.artistName).join(', ') }}</p>
                    </div>
              
                <!-- 操作按钮 -->
                <div class="song-actions">
                    <el-tooltip content="播放歌曲" placement="bottom">
                        <img :src="song.playHover ? playHoverIcon : playIcon"
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
                    <el-tooltip content="添加到歌单" placement="bottom">
                        <img :src="song.addHover ? addHoverIcon : addIcon"
                             @mouseover="song.addHover = true"
                             @mouseleave="song.addHover = false"
                             @click="toggleAddIcon(song)"
                             class="icon"
                             alt="添加到歌单" />
                    </el-tooltip>

                </div>
            </div>
        </div>

        <div class="pagination">
            <button v-if="!showAllGenre" @click="showAllGenreSongs">查看更多推荐</button>
        </div>

        <!-- 常听歌手推荐歌曲 -->
        <h2>根据常听歌手推荐以下歌曲</h2>
        <hr class="separator-line" />
        <div class="songs-list">
            <div v-for="song in displayedArtistSongs" :key="song.SongId" class="song-card">
                
                    <img :src="song.coverUrl" alt="Song Cover" class="song-cover" />
                    <div class="song-info">
                        <h3>{{ song.songName }}</h3>
                        <p>{{ song.artists.map(artist => artist.artistName).join(', ') }}</p>
                    </div>
               
                <!-- 操作按钮 -->
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

        <!-- 分页控制 -->
        <div class="pagination">
            <button v-if="!showAllArtist" @click="showAllArtistSongs">查看更多推荐</button>
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
    import AddToSongList from './AddToSongList.vue';
    import { useRouter } from 'vue-router';

    export default {
        data() {
            return {
                genreSongs: [], // 流派推荐歌曲列表
                artistSongs: [], // 歌手推荐歌曲列表
                showAllGenre: false, // 是否显示所有流派推荐的歌曲
                showAllArtist: false, // 是否显示所有歌手推荐的歌曲
                // 图标
                playIcon,
                playClickedIcon,
                playHoverIcon,
                likeIcon,
                likeHoverIcon,
                likeClickedIcon,
                addIcon,
                addHoverIcon,
                addClickedIcon,
                dialogAddVisible: false,
            };
        },
        computed: {
            displayedGenreSongs() {
                return this.showAllGenre ? this.genreSongs : this.genreSongs.slice(0, 8);
            },
            displayedArtistSongs() {
                return this.showAllArtist ? this.artistSongs : this.artistSongs.slice(0, 8);
            }
        },
        async created() {
            try {
                // 获取流派推荐歌曲
                const genreResponse = await api.apiClient.get(`/api/recommend/bygenre`);
                this.genreSongs = genreResponse.data.result;
                // 获取每个专辑的封面图
                for (const song of this.genreSongs) {
                    const albumId = song.albumId;
                    if (albumId) {
                        try {
                            const genreJpgResponse = await api.apiClient.get('/api/player/jpg', {
                                params: { albumId },
                                responseType: 'blob'
                            });
                            song.coverUrl = URL.createObjectURL(genreJpgResponse.data);
                        } catch (error) {
                            console.error(`Error fetching cover for album ID ${albumId}:`, error);
                            song.coverUrl = null;
                        }
                    }
                }

                // 获取歌手推荐歌曲
                const artistResponse = await api.apiClient.get(`/api/recommend/byartist`);
                this.artistSongs = artistResponse.data.result;
                // 获取每个专辑的封面图
                for (const song of this.artistSongs) {
                    const albumId = song.albumId;
                    if (albumId) {
                        try {
                            const artistJpgResponse = await api.apiClient.get('/api/player/jpg', {
                                params: { albumId },
                                responseType: 'blob'
                            });
                            song.coverUrl = URL.createObjectURL(artistJpgResponse.data);
                        } catch (error) {
                            console.error(`Error fetching cover for album ID ${albumId}:`, error);
                            song.coverUrl = null;
                        }
                    }
                }

            } catch (error) {
                console.error('Error fetching recommended songs:', error);
            }
        },
        components: {
            AddToSongList,
        },
        methods: {
            handleDialogClose(isVisible) {
                this.dialogAddVisible = isVisible;
            },
            showAllGenreSongs() {
                this.showAllGenre = true;
            },
            showAllArtistSongs() {
                this.showAllArtist = true;
            },
            togglePlayIcon(song) {
                console.log(song);
                this.$router.push({ path: `/mediaplayer/${song.songId}/${song.songId}` })
            },
            async toggleLikeIcon(song) {
                try {
                    if (song.liked) {
                        // 如果已收藏，发送请求删除收藏
                        await api.apiClient.delete(`/api/users/remove`, {
                            data: {
                                userId: '001',
                                songId: song.songId
                            }
                        });
                        song.liked = false;
                    } else {
                        // 如果未收藏，发送请求添加收藏
                        await api.apiClient.post(`/api/users/add`, {
                            userId: '001',
                            songId: song.songId
                        });
                        song.liked = true;
                    }
                } catch (error) {
                    console.error('操作失败,请重试', error);
                    song.liked = !song.liked; // 收藏失败，恢复到之前的状态
                }
            },
            toggleAddIcon(song) {
                this.currentSongId = song.songId;
                this.dialogAddVisible = true;
            }
        }
    };
</script>

<style scoped>
    .recommended-songs {
        margin: 40px;
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
        width: 280px; /* Fixed width */
        height: 200px; /* Fixed height to maintain a square shape */
        object-fit: cover; /* Ensures the image covers the area without distortion */
        border-radius: 4px 4px 0 0;
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
            background-color: rgba(64, 108, 194, 0.9); /* 按钮颜色 */
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