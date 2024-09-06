<template>
    <div class="results-container">
        <div class="tabs">
            <button @click="setCategory('song')" :class="{ active: category === 'song' }">单曲</button>
            <button @click="setCategory('artist')" :class="{ active: category === 'artist' }">歌手</button>
        </div>

        <div v-if="filteredResults.length" class="search-summary">
            搜索“{{ query }}”，找到
            <span class="result-count">{{ resultCount }}</span>
            {{ searchType === 'song' ? '首单曲' : '位歌手' }}
        </div>

        <el-table v-if="filteredResults.length" :data="filteredResults" style="width: 100%">
            <!-- 歌手类别显示 -->
            <el-table-column v-if="category === 'artist'"
                             label="歌手"
                             width="500">
                <template #default="scope">
                    <a @click="goToArtistPage(scope.row.artistId)" class="artist-link">{{ scope.row.artistName }}</a>
                </template>
            </el-table-column>
            <el-table-column v-if="category === 'artist'"
                             prop="artistIntro"
                             label="简介"
                             width="500">
            </el-table-column>
            <el-table-column v-if="category === 'artist'"
                             label="操作"
                             width="200">
                <template #default="scope">
                    <el-button @click="followArtist(scope.row)" type="primary" class=" artistButton">+ 关注 </el-button>
                </template>
            </el-table-column>

            <!-- 单曲类别显示 -->
            <el-table-column v-if="category === 'song'" label="歌曲" width="500">
                <template #default="scope">
                    <el-tooltip content="播放歌曲" placement="bottom">
                        <img :src="scope.row.playing ? playClickedIcon : scope.row.playHover ? playHoverIcon : playIcon"
                             @mouseover="scope.row.playHover = true"
                             @mouseleave="scope.row.playHover = false"
                             @click="togglePlayIcon(scope.row)"
                             class="play-icon"
                             alt="播放歌曲" />
                    </el-tooltip>
                    <a @click.prevent="gotoPlay(scope.row.songId)" class="song-link">{{ scope.row.songName }}</a>
                </template>
            </el-table-column>

            <el-table-column v-if="category === 'song'"
                             label="演唱"
                             width="250">
                <template #default="scope">
                    <span v-for="(artist, index) in scope.row.artist" :key="index">
                        <a @click="goToArtistPage(artist.artistId)" class="artist-link">{{ artist.artistName}}</a>
                        <span v-if="index < scope.row.artist.length - 1">, </span>
                    </span>
                </template>
            </el-table-column>
            <el-table-column v-if="category === 'song'"
                             prop="duration"
                             label="时长"
                             width="250">
                <template #default="scope">
                    {{ formatDuration(scope.row.duration) }}
                </template>
            </el-table-column>
            <el-table-column v-if="category === 'song'"
                             label="操作"
                             width="200">
                <template #default="scope">
                    <el-tooltip content="收藏歌曲" placement="bottom">
                        <img :src="scope.row.liked ? likeClickedIcon : scope.row.likeHover ? likeHoverIcon : likeIcon"
                             @mouseover="scope.row.likeHover = true"
                             @mouseleave="scope.row.likeHover = false"
                             @click="toggleLikeIcon(scope.row)"
                             class="icon"
                             alt="收藏歌曲" />
                    </el-tooltip>
                    <el-tooltip content="添加到歌单" placement="bottom">
                        <img :src="scope.row.added ? addClickedIcon : scope.row.addHover ? addHoverIcon : addIcon"
                             @mouseover="scope.row.addHover = true"
                             @mouseleave="scope.row.addHover = false"
                             @click="toggleAddIcon(scope.row)"
                             class="icon"
                             alt="添加到歌单" />
                    </el-tooltip>
                </template>
            </el-table-column>
        </el-table>

        <div v-else class="no-results">
            抱歉，没有找到相关结果
        </div>
        <el-dialog v-model="dialogAddVisible" width="500px" v-if="dialogAddVisible">
            <AddToSongList :songId="currentSongId" :dialogVisible="dialogAddVisible" @update:dialogVisible="handleDialogClose" />
        </el-dialog>
    </div>
</template>

<script>
    import { mapGetters } from 'vuex';
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

    export default {
        props: {
            results: {
                type: Array,
                required: true
            },
            searchType: {
                type: String,
                required: true
            }
        },
        data() {
            return {
                query: '', // 初始查询值
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
                currentSongId: 0,
            };
        },
        components: {
            AddToSongList
        },
        created() {
            this.query = this.$route.query.query || ''; // 从路由中获取查询参数
        },
        computed: {
            ...mapGetters('search', ['searchType']), // 从Vuex获取当前的搜索类型
            category() {
                return this.searchType; // 根据Vuex中的搜索类型确定当前类别
            },
            resultCount() {
                return this.results.length;
            },
            summary() {
                if (this.query && this.resultCount > 0) {
                    return `搜索“${this.query}”，找到${this.resultCount}${this.searchType === 'song' ? '首单曲' : '位歌手'}`;
                }
                return null;
            },
            filteredResults() {
                return this.results.map(result => {
                    if (this.category === 'artist') {
                        return {
                            artistName: result.artistName,
                            artistId: result.artistId,
                            artistIntro: result.artistIntro
                        };
                    } else {
                        return {
                            songName: result.songName,
                            artist: result.artists, // 将所有演唱者名字连接成一个字符串
                            songId: result.songId,
                            duration: result.duration,
                            playing: false,
                            liked: false,
                            added: false,
                            likeHover: false,
                            addHover: false
                        };
                    }
                });
            }
        },
        methods: {
            setCategory(category) {
                this.$emit('updateCategory', category); // 触发父组件更新类别
            },
            formatDuration(seconds) {
                const minutes = Math.floor(seconds / 60);
                const remainingSeconds = seconds % 60;
                return `${String(minutes).padStart(2, '0')}:${String(remainingSeconds).padStart(2, '0')}`;
            },
<<<<<<< Updated upstream
            followArtist(artist) {
                console.log('关注歌手:', artist.artistName);
=======
            async followArtist(artist) {
                try {
                    // 调用关注API
                    await api.apiClient.post(`/api/artist/follow`, {
                        userId: '001',
                        artistId: artist.artistId
                    });
                    this.artistFollowStatus[artist.artistId] = true; // 更新关注状态
                } catch (error) {
                    ElMessage.error('关注失败:' + error);
                }
                console.log(artist);
>>>>>>> Stashed changes
            },
            addSong(song) {
                console.log('添加歌曲:', song.songName);
            },
            togglePlayIcon(song) {
                this.$store.commit('addSongToList', song.songId);

                // 更新当前播放的歌曲 ID
                this.$store.commit('setId', song.songId);
<<<<<<< Updated upstream
            },
            gotoPlay(song) {
                this.$store.commit('addSongToList', song);

                // 更新当前播放的歌曲 ID
                this.$store.commit('setId', song);
                try {
                    // 使用 Vue Router 导航到播放页面，传递歌曲 ID 和相关的歌曲列表
                    const songList = song;
                    this.$router.push({
                        name: 'mediaplayer',
                        params: {
                            songId: song, // 当前播放的歌曲 ID
                            songList: songList  // 歌曲列表的所有 songId
                        }
                    });
                } catch (error) {
                    console.error('跳转到播放页面失败:', error);
                }
            },
=======
            },
            async checkIsFollowed(artistId) {
                try {
                    const response = await api.apiClient.get(`/api/artist/FollowStatus/${artistId}`);
                    this.artistFollowStatus[artistId] = true;
                    console.log(response.status);
                } catch (error) {
                    console.error('获取关注状态失败:', error);
                    this.artistFollowStatus[artistId] = false;
                }
            },
>>>>>>> Stashed changes
            toggleLikeIcon(song) {
                song.liked = !song.liked;
            },
            toggleAddIcon(song) {
                console.log(song);
                this.currentSongId = song.songId;
                this.dialogAddVisible = true;
            },
            handleDialogClose(isVisible) {
                this.dialogAddVisible = isVisible;
            },
<<<<<<< Updated upstream
=======
            gotoPlay(song) {
                this.$store.commit('addSongToList', song);

                // 更新当前播放的歌曲 ID
                this.$store.commit('setId', song);
                try {
                    // 使用 Vue Router 导航到播放页面，传递歌曲 ID 和相关的歌曲列表
                    const songList = song;
                        name: 'mediaplayer',
                        params: {
                            songId: song, // 当前播放的歌曲 ID
                            songList: songList  // 歌曲列表的所有 songId
                        }
                    });
                } catch (error) {
                    console.error('跳转到播放页面失败:', error);
                }
            },
>>>>>>> Stashed changes
            // 跳转到艺术家详情
            goToArtistPage(artistId) {
                this.$router.push({ name: "SingerDetail", params: { artistId: artistId } });
            },
        }
    }
</script>

<style scoped>
    .results-container {
        margin: 20px auto;
        width: 1200px;
        text-align: center;
        justify-content: center;
    }

    .el-table th, .el-table td {
        text-align: center;
    }

    .tabs {
        display: flex;
        justify-content: center;
        margin-bottom: 10px;
    }

        .tabs button {
            margin: 0 50px;
            padding: 10px 15px;
            background-color: #f0f0f0;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            width: 100px;
        }

            .tabs button.active {
                background-color: rgba(64, 108, 194, 0.9);
                color: white;
                font-weight: bold;
            }

            .tabs button:hover {
                background-color: rgba(226,233,247,0.9);
                color: black;
                font-weight: normal;
            }

    .search-summary {
        margin-bottom: 10px;
        margin-left: 0;
        font-size: 15px;
        color: #666;
        text-align: left;
        position: relative;
        left: 0;
    }


    .result-count {
        color: #CC2C1F;
    }

    .no-results {
        padding: 20px;
        text-align: center;
        font-size: 16px;
        color: #666;
    }

    .artistButton {
        background-color: rgba(64, 108, 194, 0.9);
        color: white;
    }

        .artistButton:hover {
            background-color: #95ADE0;
        }

        .artistButton:active {
            background-color: #385FAB;
        }

    .artist-link {
        color: #284da0c1;
        text-decoration: none;
        cursor: pointer;
    }

        .artist-link:hover {
            text-decoration: underline;
            background-color: transparent;
        }

    .song-link {
        color: #284da0c1;
        text-decoration: none;
        cursor: pointer;
    }

        .song-link:hover {
            text-decoration: underline;
            background-color: transparent;
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
</style>