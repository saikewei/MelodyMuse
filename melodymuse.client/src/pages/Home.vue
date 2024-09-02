<template>
    <div class="home">
        <TheHeader />
        <Swiper />
        <RecommendedSongs />
        <TheFooter />
    </div>
</template>

<script>
    import Swiper from "../components/Swiper.vue";
    import TheFooter from "../components/TheFooter.vue";
    import TheHeader from "../components/TheHeader.vue";
    import RecommendedSongs from "../components/RecommendedSongs.vue";

    export default {
        name: 'Home',
        components: {
            Swiper,
            TheFooter,
            TheHeader,
            RecommendedSongs
        }
    }
</script>

<style scoped>
    .home {
        min-height: 100vh; /* 确保页面至少占据视口高度 */
        padding-bottom: 80px; /* 留出足够空间给 footer，调整为适合的高度 */
        box-sizing: border-box; /* 确保 padding 包含在元素的总高度内 */
    }

    .swiper {
        position: relative;
        z-index: 1; /* 确保轮播图在推荐歌曲上面 */
        margin-bottom: 20px; /* 为推荐歌曲留出间距 */
    }

    .recommended-songs {
        position: relative;
        z-index: 0; /* 确保推荐歌曲在轮播图下方 */
        margin-bottom: 60px; /* 为 footer 留出空间 */
    }

    .the-footer {
        position: fixed;
        bottom: 0;
        left: 50%;
        transform: translateX(-50%);
        width: 100%; /* 确保 footer 宽度覆盖整个视口宽度 */
        max-width: 1200px; /* 最大宽度，确保 footer 不会过宽 */
        height: 70px; /* 设置 footer 的高度 */
        background-color: #f1f1f1; /* 背景颜色 */
        z-index: 2; /* 确保 footer 在其他内容上面 */
        text-align: center;
        line-height: 30px; /* 垂直居中 footer 中的文本 */
        padding: 0 0px; /* 添加左右内边距 */
        box-sizing: border-box; /* 确保 padding 包含在元素的总宽度内 */
    }
</style>

