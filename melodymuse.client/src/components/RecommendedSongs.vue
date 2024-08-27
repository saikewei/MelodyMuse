<template>
    <div class="recommended-songs">
        <h2>RECOMMENDED SONGS</h2>
        <div class="songs-list">
            <div v-for="song in displayedSongs" :key="song.SongId" class="song-card">
                <img :src="song.coverUrl" alt="Song Cover" class="song-cover" />
                <div class="song-info">
                    <h3>{{ song.songName }}</h3>
                    <p>{{ song.artists.map(artist => artist.artistName).join(', ') }}</p>
                </div>
            </div>
        </div>
        <div class="pagination">
            <button v-if="!showAll" @click="showAllSongs">More</button>
        </div>
    </div>
</template>

<script>
    import api from '../api/http.js';

    export default {
        data() {
            return {
                songs: [], // 推荐歌曲列表
                userId: null, // 用户ID
                showAll: false, // 是否显示所有歌曲
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
            }
        }
    };
</script>

<style scoped>
  
    .songs-list {
        display: grid;
        grid-template-columns: repeat(4, 1fr); /* 每行显示4个 */
        gap: 20px;
    }

    .song-card {
        text-align: center;
        background-color: #f9f9f9;
        border-radius: 8px;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
        transition: transform 0.2s;
    }

        .song-card:hover {
            transform: translateY(-5px);
        }

    .song-cover {
        width: 100%;
        height: 150px;
        object-fit: cover;
        border-radius: 8px 8px 0 0;
    }

    .song-info {
        padding: 10px;
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
