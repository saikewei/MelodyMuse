<template>
    <div id="app" class="m-content-display">
        <div class="m-router-view">
            <router-view />
        </div>
    </div>
    <TheFooter></TheFooter>
    <playBar v-if="showPlayBar" />
</template>

<script>
import TheFooter from './components/TheFooter.vue';
import playBar from './components/playBar.vue';

    export default {
        components:{
            TheFooter,
            playBar
        },
        computed: {
            // 通过路由检查是否需要展示 playBar
            showPlayBar() {
                return this.$route.name !== 'Login'; // 当路由不是登录页时展示 PlayBar
            }
        }
}
</script>

<style>
.m-content-display {
        display: flex;
        flex-direction: column;
        min-height: 800px;
}
.m-router-view {
  flex: 1;
}
</style>