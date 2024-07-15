<template>
    <div class="home">
        <swiper />
        <TheFooter />
    </div>
</template>

<script>
    import Swiper from "../components/Swiper.vue";
    import TheFooter from "../components/TheFooter.vue"
    export default {
        name: 'home',
        components: {
            Swiper,
            TheFooter
        },
        data() {
            return {
            }
        },
        created() {
        },
        methods: {
        }
    }
</script>

<style scoped>
</style>
