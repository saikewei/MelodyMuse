<template>
    <div class="search-result-page">
        <TheHeader @search="handleSearch" />
        <h1>搜索结果</h1>
        <div v-if="loading">加载中...</div>
        <div v-else>
            <SearchResultList :results="searchResults" :searchType="searchType" @updateCategory="handleCategoryUpdate" />
            <TheFooter />
        </div>
    </div>
</template>

<script>
    import TheFooter from "../components/TheFooter.vue";
    import TheHeader from "../components/TheHeader.vue";
    import axios from 'axios';
    import SearchResultList from '../components/SearchResultList.vue';
    import { mapGetters, mapActions } from 'vuex';

    export default {
        components: {
            TheHeader,
            SearchResultList,
            TheFooter
        },
        data() {
            return {
                loading: false, // 用于控制加载状态的标志
            };
        },
        computed: {
            ...mapGetters('search', ['searchType', 'searchResults']) // 从Vuex中获取搜索类型和结果
        },
        created() {
            this.performSearch(); // 页面加载时执行搜索
        },
        watch: {
            '$route.query': {
                handler() {
                    this.updateSearchType(this.$route.query.type || 'songs'); // 根据URL参数更新搜索类型
                    this.performSearch(); // 每当查询参数变化时执行搜索
                },
                immediate: true // 确保组件加载时立即执行handler
            }
        },
        methods: {
            ...mapActions('search', ['updateSearchType', 'setSearchResults']), // Vuex actions，用于更新搜索类型和结果
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
                            const artistResponse = await axios.get(`https://localhost:7223/api/search/artists`, {
                                params: { query: encodeURIComponent(query) }
                            });
                            results = artistResponse.data.map(artist => ({ ...artist, type: 'artist' }));
                        } else if (this.searchType === 'song') {
                            const songResponse = await axios.get(`https://localhost:7223/api/search/songs`, {
                                params: { query: encodeURIComponent(query) }
                            });
                            results = songResponse.data.map(song => ({ ...song, type: 'song' }));
                        }

                        this.setSearchResults(results); // 设置搜索结果到Vuex store
                    } catch (error) {
                        console.error('API Error:', error);
                        this.setSearchResults([]); // 出现错误时清空结果
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
    }

    h1 {
        margin: 20px 0;
    }
</style>
