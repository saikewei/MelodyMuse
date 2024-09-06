<template>
    <div>
        <TheHeader />
        <div class="header">
            <p class="header-text" v-html="currentTitleSpan"></p>
            <div class="buttons">
                <button @click="selectTab('rankartists')"
                        :class="{'active': currentTab === 'rankartists'}">
                    歌手榜
                </button>
                <button @click="selectTab('ranksongs')"
                        :class="{'active': currentTab === 'ranksongs'}">
                    热歌榜
                </button>
            </div>
        </div>
        <div class="ranking-container">
            <!-- Content based on selected tab -->
            <div v-if="currentTab === 'rankartists'" class="ranking-table-container">
                <table class="ranking-table">
                    <thead>
                        <tr>
                            <th>排名</th>
                            <th>音乐人</th>
                            <th>粉丝数</th>
                            <th>总播放次数</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(artist, index) in artists" :key="artist.artistId">
                            <td :class="{'top-three': index < 3}">{{ index + 1 }}</td>
                            <td>
                                <router-link :to="{ name: 'SingerDetail', params: { artistId: artist.artistId } }" class="artist-link">
                                    {{ artist.artistName }}
                                </router-link>
                            </td>
                            <td>{{ artist.fansCount }}</td>
                            <td>{{ artist.totalPlayCount }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>

            <div v-if="currentTab === 'ranksongs'" class="ranking-table-container">
                <button v-if="songs.length > 0" @click="goToPlayPage" class="play-all-button">
                    ▷播放全部
                </button>
                <table class="ranking-table">
                    <thead>
                        <tr>
                            <th>排名</th>
                            <th>歌名</th>
                            <th>歌手</th>
                            <th>时长</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr v-for="(song, index) in songs" :key="song.songId">
                            <td :class="{'top-three': index < 3}">{{ index + 1 }}</td>
                            <td>
                                <a @click="goToPlay(song.songId)" class="song-link">
                                    {{ song.songName }}
                                </a>
                            </td>
                            <td>{{ song.artistName }}</td>
                            <td>{{ formatDuration(song.duration) }}</td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</template>






<script>
    import RankingImage from '../assets/logo2.jpg';
    import api from "../api/http.js";
    import TheHeader from "../components/TheHeader.vue";

    export default {
        name: "RankingPage",
        components: {
            TheHeader,
        },
        data() {
            return {
                RankingImage,
                currentTab: 'rankartists',
                artists: [],
                songs: [],
            };
        },
        computed: {
            currentImageClass() {
                return this.currentTab === 'rankartists' ? 'ranking-image1' : 'ranking-image2';
            },
            currentTitleSpan() {
                return this.currentTab === 'rankartists' ?
                    '歌手榜<span><strong>TOP50</strong></span>' :
                    '歌曲榜<span><strong>TOP50</strong></span>';
            }
        },
        methods: {
            selectTab(tab) {
                this.currentTab = tab;
                if (tab === 'rankartists') {
                    this.fetchArtists();
                } else if (tab === 'ranksongs') {
                    this.fetchSongs();
                }
            },
            fetchArtists() {
                api.apiClient.get('/api/rank/ranking')
                    .then(response => {
                        this.artists = response.data;
                    })
                    .catch(error => {
                        console.error('获取音乐人信息失败:', error);
                    });
            },
            fetchSongs() {
                api.apiClient.get('/api/rank/top-songs')
                    .then(response => {
                        this.songs = response.data;
                    })
                    .catch(error => {
                        console.error('获取歌曲信息失败:', error);
                    });
            },
            formatDuration(seconds) {
                const minutes = Math.floor(seconds / 60);
                const remainingSeconds = seconds % 60;
                return `${minutes}:${remainingSeconds < 10 ? '0' : ''}${remainingSeconds}`;
            },
            goToPlay(song) {
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
            // 跳转到播放页面
            goToPlayPage() {
                if (this.songs.length > 0) {
                    // 获取歌曲列表的第一首歌的 ID
                    const firstSongId = this.songs[0].songId;

                    // 构建完整的歌曲 ID 列表字符串，作为路径参数传递
                    const songList = this.songs.map(s => s.songId);
                    this.$store.commit('setListOfSongs', songList);

                    // 更新当前播放的歌曲 ID
                    this.$store.commit('setId', firstSongId);

                    // 使用 Vue Router 导航到 mediaplayer 页面，并传递歌曲 ID 和歌曲列表
                    this.$router.push({
                        name: 'mediaplayer',
                        params: {
                            songId: firstSongId, // 第一个歌曲的 ID
                            songList: firstSongId   // 所有歌曲 ID 组成的字符串
                        }
                    });
                } else {
                    console.error('歌曲列表为空，无法播放歌曲');
                }
            },
        },
        mounted() {
            this.fetchArtists();
        }
    };
</script>



<style scoped>
    .header {
        height: 220px;
        background-color: #284da0c1;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        margin-bottom: 20px;
    }

    .header-text {
        color: aliceblue;
        font-size: 30px;
        margin-top: 45px;
        text-align: center;
    }

    .buttons {
        margin-top: 10px;
    }

        .buttons button {
            padding: 10px 20px;
            margin: 10px 10px;
            border: none;
            border-radius: 20px;
            background-color: transparent;
            color: white;
            cursor: pointer;
        }

            .buttons button.active {
                background-color: white;
                color: black;
            }

    .ranking-container {
        padding: 40px;
        background-color: white; /* Changed to white */
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        width: 100%;
    }

    .ranking-image1, .ranking-image2 {
        position: absolute;
        top: 6%;
        width: 100px;
        height: 100px;
        border-radius: 50%;
        margin-right: 20px;
    }

    .ranking-image1 {
        left: 32%;
    }

    .ranking-image2 {
        left: 32%;
    }

    .ranking-title1, .ranking-title2 {
        text-align: center;
        font-size: 40px;
        margin-left: 10px;
        margin-bottom: 85px;
        color: #193169c1;
        letter-spacing: 2px;
    }

        .ranking-title1 span, .ranking-title2 span {
            font-weight: normal;
            font-size: -2em;
            margin-left: 40px;
            margin-right: 10px;
        }

        .ranking-title1 strong, .ranking-title2 strong {
            font-weight: bold;
            font-size: 1.7em;
            color: white;
            font-style: italic;
            text-shadow: 2px 2px 5px rgba(24, 44, 145, 0.6);
        }

    .play-all-button {
        display: block;
        margin: 0 auto 30px;
        padding: 10px 20px;
        background-color: #284da0c1;
        color: #fff;
        border: none;
        border-radius: 5px;
        cursor: pointer;
    }

        .play-all-button:hover {
            background-color: #284da0c1;
        }

    .ranking-table-container {
        max-height: 490px;
        overflow-y: auto;
        position: relative;
    }

    .ranking-table {
        width: 100%;
        border-collapse: collapse;
        background-color: white;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        text-align: center;
    }

        .ranking-table th, .ranking-table td {
            padding: 10px;
            text-align: center;
            border-bottom: 1px solid #ddd;
        }

        .ranking-table th {
            background-color: #f2f2f2;
        }

        .ranking-table tr:hover {
            background-color: #f5f5f5;
            cursor: pointer;
        }

    .top-three {
        font-weight: bold;
        text-shadow: 2px 2px 5px rgba(24, 44, 145, 0.2);
        color: #284da0c1;
        font-size: 1.5em;
    }

    .artist-link, .song-link {
        color: #284da0c1;
        background-color: transparent;
    }

        .artist-link:hover, .song-link:hover {
            background-color: transparent;
            text-decoration: underline;
        }
</style>



