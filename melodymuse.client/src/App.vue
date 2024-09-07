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
        components: {
            TheFooter,
            playBar
        },
        computed: {
            // ͨ��·�ɼ���Ƿ���Ҫչʾ playBar
            showPlayBar() {
                return !['Login', 'Register', 'ForgottenPassword'].includes(this.$route.name); // ��·�ɲ��ǵ�¼ҳʱչʾ PlayBar
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