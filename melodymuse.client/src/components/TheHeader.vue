<template>
    <nav class="the-header">
        <div class="header-logo">
            <img :src="Images.logoUrl" alt="Home Icon" style="height: 40px; width: 40px;">
            <img :src="Images.nameUr1" alt="Home Icon" style="height: 40px; width: 90px;">
        </div>
        <ul class="navbar">
            <li :class="{ active: item.name === activeName }" v-for="item in navMsg" :key="item.path" @click="goPage(item.path, item.name)">
                {{ item.name }}
            </li>
        </ul>
        <div class="navbar-search">
            <select v-model="searchType">
                <option value="artists">Artist</option>
                <option value="songs">Song Name</option>
                <option value="lyrics">Lyrics</option>
            </select>
            <input type="text" v-model="searchQuery" placeholder="Search..." />
            <button @click="performSearch">Search</button>
        </div>
        <search-results :results="searchResults" />
    </nav>
</template>

<script>
    import axios from 'axios'
    import { mapGetters } from 'vuex'
    import logoUrl from '../assets/logo2.jpg'
    import nameUr1 from '../assets/name1.jpg'
    import { navMsg } from '../assets/data/header'
    import SearchResults from './SearchResults.vue'

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
                navMsg: [],
                searchQuery: '',
                searchResults: [],
                searchType: 'artists'  // 默认搜索类型为歌手
            }
        },
        computed: {
            ...mapGetters('configure', ['activeName'])
        },
        created() {
            this.navMsg = navMsg
        },
        methods: {
            goHome() {
                this.$router.push({ path: '/' })
            },
            async performSearch() {
                if (this.searchQuery.trim() === '') return

                const query = this.searchQuery
                const type = this.searchType
                let response

                try {
                    response = await axios.get(`https://localhost:7223/api/search/${type}`, {
                        params: {
                            query: encodeURIComponent(query)
                        }
                    })
                    console.log('API Response:', response.data)  // Log API response for debugging
                    this.searchResults = response.data // Directly use the response data
                } catch (error) {
                    console.error('API Error:', error)
                    this.searchResults = []
                }
            },
            goPage(path, name) {
                this.$store.commit('configure/setActiveName', name)
                this.$router.push({ path })
            }
        }
    }
</script>

<style scoped>
    .header-container {
        display: flex;
        flex-direction: column; /* Stack elements vertically */
    }

    .the-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        width: 100%;
        padding: 0px 30px;
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
    }

    .navbar {
        list-style: none;
        padding: 0;
        margin: 0;
        display: flex;
        gap: 20px;
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
            }

            .navbar li.active::after {
                width: 100%;
            }

            .navbar li.active {
                color: #284da0c1;
            }

            .navbar li:hover {
                color: #284da0c1;
            }

    .navbar-search {
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
            background-color: #284da0c1;
            color: white;
            cursor: pointer;
        }

    .search-results-container {
        width: 100%;
        max-width: 1200px; /* 可根据需要调整 */
        margin: 80px auto 20px; /* 确保与搜索栏有足够的间距 */
        padding: 0 20px; /* 添加水平内边距 */
    }
</style>
