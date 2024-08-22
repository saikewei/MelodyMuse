<template>
    <div class="home">
        <TheHeader />
        <swiper />
        <TheFooter />
    </div>
</template>

<script>
    import Swiper from "../components/Swiper.vue";
    import TheFooter from "../components/TheFooter.vue"
    import TheHeader from "../components/TheHeader.vue"
    export default {
        name: 'home',
        components: {
            Swiper,
            TheHeader,
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
