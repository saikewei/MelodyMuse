<template>
    <nav class="the-header">
        <div class="header-logo" @click="goHome">
            <img :src="Images.logoUrl" alt="Home Icon" style="height: 55px; width: 55px;">
        </div>
        <ul class="navbar">
            <li v-for="item in navMsg" :key="item.path" @click="goPage(item.path,item.name)">
                {{ item.name }}
            </li>
        </ul>
        <div class="navbar-search">
            <input type="text" v-model="searchQuery" @input="onSearch" placeholder="Search..." />
        </div>
    </nav>
</template>

<script>
    import logoUrl from "../assets/logo.png"
    import { navMsg } from '../assets/data/header'

    export default {
        data() {
            return {
                Images: {
                    logoUrl
                },
                navMsg: [],
                searchQuery: ''
            }
        },
        created() {
            this.navMsg = navMsg;
        },
        methods: {
            goHome() {
                this.$router.push({path: "/"});
            },
            onSearch() {
                this.$emit('search', this.searchQuery);
            },
            goPage(path,name){
                this.$router.push({path:path});
            }
        },
    }
</script>

<style scoped>
    .the-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        width: 100%;
        padding: 10px 20px;
        box-sizing: border-box;
        background-color: #eeceea;
        box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
        position: fixed;
        top: 0;
        left: 0;
    }

    .header-logo {
        display: flex;
        align-items: center;
    }

    .logo-img {
        height: 35px;
        width: 35px;
    }

    .navbar {
        display: flex;
        align-items: center;
        list-style: none;
        padding: 0;
        margin: 0;
        flex: 1; /* 占据剩余空间 */
        justify-content: center;
    }

        .navbar li {
            margin-right: 40px;
            font-size: 18px;
            color: #6a5d64;
            cursor: pointer;
        }

    .navbar-search {
        display: flex;
        align-items: center;
    }

        .navbar-search input {
            padding: 5px;
            border-radius: 4px;
            border: 1px solid #ccc;
            outline: none;
            width: 300px; /* 设置搜索框的宽度 */
            height: 30px;
        }
</style>
