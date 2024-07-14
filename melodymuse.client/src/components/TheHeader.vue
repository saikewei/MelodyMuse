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
            <input type="text" v-model="searchQuery" @input="onSearch" placeholder="Search..." />
        </div>
    </nav>
</template>

<script>
    import { mapGetters } from 'vuex'
    import logoUrl from '../assets/logo2.jpg'
    import nameUr1 from '../assets/name1.jpg'
    import { navMsg } from '../assets/data/header'

    export default {
        data() {
            return {
                Images: {
                    logoUrl,nameUr1
                },
                navMsg: [],
                searchQuery: ''
            }
        },
        computed: {
            ...mapGetters('configure', ['activeName']) // 指定模块名称
        },
        created() {
            this.navMsg = navMsg
        },
        methods: {
            goHome() {
                this.$router.push({ path: '/' })
            },
            onSearch() {
                this.$emit('search', this.searchQuery)
            },
            goPage(path, name) {
                this.$store.commit('configure/setActiveName', name) // 确保模块和 mutation 名称正确
                this.$router.push({ path })
            }
        }
    }
</script>


<style scoped>
   .the-header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    padding: 0px 30px; /* 增加内边距以改善视觉效果 */
    box-sizing: border-box;
    background-color:#ffffff;
    box-shadow: 0 4px 8px #cacaca; /* 减少阴影的强度 */
    position: fixed;
    top: 0;
    left: 0;
    z-index: 1000; /* 确保头部在其他内容上方 */
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
    gap: 20px; /* 增加导航项之间的间距 */
}

.navbar li {
    cursor: pointer;
    padding: 20px 20px; /* 增加内边距以提升点击区域 */
    position: relative;
    display: inline-block; /* 使宽度仅为内容宽度 */
    font-weight: 500;
    color: #808080; /* 文字颜色 */
    line-height: 17px; /* 调整为导航条的高度 */
    transition: color 0.3s ease; /* 平滑颜色过渡效果 */
}

/* 高光效果 */
    .navbar li::after {
        content: '';
        position: absolute;
        bottom: 0; /* 确保对齐底部 */
        left: 0;
        width: 0%; /* 默认隐藏高光条 */
        height: 5px; /* 高光条的高度 */
        background-color: #284da0c1; /* 蓝色高光条 */
    }

/* 激活状态的样式 */
.navbar li.active::after {
    width: 100%; /* 激活状态下高光条宽度为文本宽度 */
}

    .navbar li.active {
        color: #284da0c1; 
    }

/* 悬停状态的样式 */
    .navbar li:hover {
        color: #284da0c1;
    }

.navbar-search input {
    padding: 8px;
    border-radius: 20px; /* 增加圆角 */
    border: 1.5px solid #cacaca; 
    outline: none;
    width: 300px; /* 设置搜索框的宽度 */
    height: 35px; /* 调整搜索框的高度 */
    transition: border-color 0.3s ease; /* 平滑边框颜色过渡效果 */
}

    .navbar-search input:focus {
        border-color: #284da0c1; /* 聚焦时边框颜色变化 */
    }
</style>
