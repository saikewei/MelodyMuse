<template>
    <div class="recommended-songs">
        <h2>Recommended Songs</h2>
        <div class="songs-list">
            <div v-for="song in songs" :key="song.SongId" class="song-card">
                <img :src="song.coverUrl" alt="Song Cover" class="song-cover" />
                <div class="song-info">
                    <h3>{{ song.songName }}</h3>
                    <p>{{ song.artists.map(artist => artist.artistName).join(', ') }}</p>
                </div>
            </div>
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
            };
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
        }
    };
</script>

<style scoped>
    .recommended-songs {
        padding: 20px;
    }

    .songs-list {
        display: flex;
        flex-wrap: wrap;
        gap: 20px;
    }

    .song-card {
        width: 150px;
        text-align: center;
    }

    .song-cover {
        width: 100%;
        height: auto;
        border-radius: 8px;
    }

    .song-info {
        margin-top: 10px;
    }
</style>