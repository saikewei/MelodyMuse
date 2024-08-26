<template>
    <div class="recommended-songs">
        <h2>Recommended Songs</h2>
        <div class="songs-list">
            <div v-for="song in songs" :key="song.SongId" class="song-card">
                <img :src="song.coverUrl" alt="Song Cover" class="song-cover" />
                <div class="song-info">
                    <h3>{{ song.SongName }}</h3>
                    <p>{{ song.Artists.map(artist => artist.ArtistName).join(', ') }}</p>
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
                songs: []
            };
        },
        async created() {
            try {
                // 获取用户ID，假设你可以从Vuex store中获取
                const userId = this.$store.getters['user/userId'];

                // 发起请求以获取推荐歌曲
                const response = await api.apiClient.get(`/api/recommend/${userId}`);

                // 设置歌曲数据
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
