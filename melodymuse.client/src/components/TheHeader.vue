<template>
    <div class="header-container" @click="handleOutsideClick">
        <nav class="the-header">
            <div class="header-logo" @click="goHome">
                <img :src="Images.logoUrl" alt="Home Icon" style="height: 40px; width: 40px;">
                <img :src="Images.nameUr1" alt="Home Icon" style="height: 40px; width: 90px;">
            </div>
            <ul class="navbar">
                <li :class="{ active: item.name === activeName }" v-for="item in navMsg" :key="item.path" @click="goPage(item.path, item.name)">
                    {{ item.name }}
                </li>
            </ul>
            <div class="navbar-search">
                <select v-model="searchType" @change="handleSearchTypeChange">
                    <option value="song">搜索单曲</option>
                    <option value="artist">搜索歌手</option>
                </select>
                <input type="text" v-model="inputQuery" placeholder="Search..." @focus="showResultsPopup" @input="performSearch" ref="searchInput" />
                <button @click="goSearchPage">Search</button>
                <div v-if="showPopup && filteredResults.length" class="search-results-popup" @click.stop>
                    <SearchResults :results="filteredResults" :searchType="searchType" />
                </div>
            </div>
        </nav>
    </div>
</template>

<script>
    import axios from 'axios';
    import { mapGetters, mapActions } from 'vuex';
    import logoUrl from '../assets/logo2.jpg';
    import nameUr1 from '../assets/name1.jpg';
    import { navMsg } from '../assets/data/header';
    import SearchResults from './SearchResults.vue';

    export default {
        components: {
            SearchResults
        },
        data() {
            return {
                Images: {
                    logoUrl,
                    nameUr1
                },
                navMsg: navMsg,
                inputQuery: '', // 实时更新弹出框用
                actualQuery: '', // 点击Search按钮时用
                searchResults: [],
                searchType: 'song', // 初始设置
                actualType: 'song',
                showPopup: false  // 控制弹出框显示
            };
        },
        computed: {
            ...mapGetters('configure', ['activeName']),
            ...mapGetters('search', ['searchType']),
            filteredResults() {
                return this.searchResults.filter(result => result.type === this.searchType);
            }
        },
        created() {
            this.inputQuery = this.$route.query.query || '';
            this.searchType = this.$route.query.type || 'song';
            this.updateSearchType(this.searchType); // 更新 Vuex 中的搜索类型
            if (this.inputQuery) {
                this.performSearch();
            }
        },
        mounted() {
            document.addEventListener('click', this.handleOutsideClick);
        },
        beforeUnmount() {
            document.removeEventListener('click', this.handleOutsideClick);
        },
        watch: {
            '$route.query': {
                handler(newQuery) {
                    this.searchQuery = newQuery.query || '';
                    this.searchType = newQuery.type || 'artists';
                    this.performSearch();
                },
                immediate: true
            }
        },
        methods: {
            ...mapActions('search', ['updateSearchResults', 'updateSearchType']),
            async performSearch() {
                if (this.inputQuery.trim() === '') {
                    this.searchResults = [];
                    this.showPopup = false;
                    return;
                }

                try {
                    let results = [];

                    if (this.searchType === 'artist') {
                        const artistResponse = await axios.get(`https://localhost:7223/api/search/artists`, {
                            params: { query: encodeURIComponent(this.inputQuery) }
                        });
                        results = artistResponse.data.map(artist => ({ ...artist, type: 'artist' }));
                    } else if (this.searchType === 'song') {
                        const songResponse = await axios.get(`https://localhost:7223/api/search/songs`, {
                            params: { query: encodeURIComponent(this.inputQuery) }
                        });
                        results = songResponse.data.map(song => ({ ...song, type: 'song' }));
                    }

                    //this.updateSearchResults(results);
                    this.searchResults = results;

                    // 检查鼠标是否在搜索框内，如果不在则隐藏弹出框
                    if (this.$refs.searchInput && this.$refs.searchInput.contains(document.activeElement)) {
                        this.showPopup = this.searchResults.length > 0;
                    } else {
                        this.showPopup = false;
                    }
                } catch (error) {
                    console.error('API Error:', error);
                    //this.updateSearchResults([]);
                    this.searchResults = [];
                    this.showPopup = false;
                }
            },
            handleSearchTypeChange() {
                //this.updateSearchType(this.searchType); // 更新 Vuex 中的搜索类型
                this.performSearch(); // 更新搜索结果
            },
            goSearchPage() {
                if (this.inputQuery != '') {
                    this.actualQuery = this.inputQuery;
                    this.actualType = this.searchType;
                    this.updateSearchType(this.actualType); // 更新 Vuex 中的搜索类型
                    this.$router.push({
                        path: '/searchResultPage',
                        query: {
                            query: this.actualQuery,
                            type: this.actualType
                        }
                    });
                }
            },
            goHome() {
                this.$router.push({ path: '/' });
            },
            goPage(path, name) {
                this.$store.commit('configure/setActiveName', name);
                this.$router.push({ path });
            },
            showResultsPopup() {
                if (this.inputQuery.trim() !== '') {
                    this.showPopup = true;
                }
            },
            handleOutsideClick(event) {
                const searchInput = this.$refs.searchInput;
                if (!searchInput.contains(event.target)) {
                    this.showPopup = false;
                }
            }
        }
    };
</script>

<style scoped>
    .header-container {
        display: flex;
        flex-direction: column;
    }

    .the-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        width: 100%;
        padding: 0px 45px;
        box-sizing: border-box;
        background-color: #ffffff;
        box-shadow: 0 4px 8px #cacaca;
        position: fixed;
        top: 0;
        left: 0;
        z-index: 1000;
    }

    .header-logo {
        display: flex;
        align-items: center;
        cursor: pointer;
    }

    .navbar {
        list-style: none;
        padding: 0;
        margin: 0;
        display: flex;
        gap: 60px;
    }

        .navbar li {
            cursor: pointer;
            padding: 20px 20px;
            position: relative;
            display: inline-block;
            font-weight: 500;
            color: #808080;
            line-height: 17px;
            transition: color 0.3s ease;
        }

            .navbar li::after {
                content: '';
                position: absolute;
                bottom: 0;
                left: 0;
                width: 0%;
                height: 5px;
                background-color: #284da0c1;
                transition: width 0.3s ease;
            }

            .navbar li.active::after {
                width: 120%;
                left: -10%
            }

            .navbar li.active {
                color: #284da0c1;
            }

            .navbar li:hover {
                color: #284da0c1;
            }

    .navbar-search {
        position: relative;
        display: flex;
        align-items: center;
    }

        .navbar-search select {
            padding: 8px;
            margin-right: 10px;
            border-radius: 20px;
            border: 1.5px solid #cacaca;
            outline: none;
            transition: border-color 0.3s ease;
        }

            .navbar-search select:focus {
                border-color: #284da0c1;
            }

        .navbar-search input {
            padding: 8px;
            border-radius: 20px;
            border: 1.5px solid #cacaca;
            outline: none;
            width: 300px;
            height: 35px;
            transition: border-color 0.3s ease;
        }

            .navbar-search input:focus {
                border-color: #284da0c1;
            }

        .navbar-search button {
            padding: 8px 16px;
            margin-left: 10px;
            border-radius: 20px;
            border: none;
            background-color: rgba(64, 108, 194, 0.9);
            color: white;
            cursor: pointer;
        }
            .navbar-search button:hover {
                background-color: #95ADE0;
            }
            .navbar-search button:active {
                background-color: #385FAB;
            }

    .search-results-popup {
        position: absolute;
        top: 100%;
        left: 0;
        width: 100%;
        background-color: white;
        box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
        border-radius: 4px;
        z-index: 1000;
        max-height: 400px;
        overflow-y: auto;
    }
</style>
