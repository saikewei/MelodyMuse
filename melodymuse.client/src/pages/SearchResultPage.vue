<template>
    <div class="search-result-page">
        <TheHeader @search="handleSearch" />
        <div v-if="loading">加载中...</div>
        <div v-else>
            <SearchResultList :results="searchResults" :searchType="searchType" @updateCategory="handleCategoryUpdate" />
        </div>
    </div>
</template>

<script>
    import TheHeader from "../components/TheHeader.vue";
    //import playBar from "../components/playBar.vue";
    import SearchResultList from '../components/SearchResultList.vue';
    import { mapGetters, mapActions } from 'vuex';
    import api from '../api/http.js'


    export default {
        components: {
            TheHeader,
            SearchResultList,
        },
        data() {
            return {
                loading: false, // 控制加载状态
            };
        },
        computed: {
            ...mapGetters('search', ['searchType', 'searchResults']) // 从 Vuex 中获取搜索类型和结果
        },
        created() {
            this.performSearch(); // 页面加载时执行搜索
        },
        watch: {
            '$route.query': {
                handler() {
                    if (this.$route.query.query) {
                        this.updateSearchType(this.$route.query.type || 'artist'); // 根据 URL 参数更新搜索类型
                        this.performSearch(); // 查询参数变化时执行搜索
                    }
                },
                immediate: true // 确保组件加载时立即执行 handler
            }
        },
        methods: {
            ...mapActions('search', ['updateSearchType', 'updateSearchResults']), // Vuex actions，用于更新搜索类型和结果
            async handleSearch({ query, category }) {
                this.updateSearchType(category); // 更新搜索类型
                this.$router.push({
                    path: '/searchResultPage',
                    query: { query, type: category }
                });
                await this.performSearch(); // 重新执行搜索
            },
            async handleCategoryUpdate(category) {
                this.updateSearchType(category); // 更新搜索类型
                this.$router.push({
                    query: {
                        ...this.$route.query,
                        type: category
                    }
                });
                await this.performSearch(); // 重新执行搜索
            },
            async performSearch() {
                const query = this.$route.query.query;
                if (query) {
                    this.loading = true;
                    try {
                        let results = [];
                        if (this.searchType === 'artist') {
                            const artistResponse = await api.apiClient.get(`/api/search/artists`, {
                                params: { query: encodeURIComponent(query) }
                            });
                            results = artistResponse.data.map(artist => ({ ...artist, type: 'artist' }));
                        } else if (this.searchType === 'song') {
                            const songResponse = await api.apiClient.get(`/api/search/songs`, {
                                params: { query: encodeURIComponent(query) }
                            });
                            results = songResponse.data.map(song => ({ ...song, type: 'song' }));
                        }

                        this.updateSearchResults(results); // 设置搜索结果到 Vuex store
                    } catch (error) {
                        console.error('API Error:', error);
                        this.updateSearchResults([]); // 出现错误时清空结果
                    } finally {
                        this.loading = false;
                    }
                }
            }
        }
    }
</script>

<style scoped>
    .search-result-page {
        padding: 20px;
        text-align: center;
        margin-bottom: 50px;
    }

    h1 {
        margin: 20px 0;
    }
</style>
