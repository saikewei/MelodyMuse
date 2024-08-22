<template>
    <div>
        <TheHeader />
        歌单
        <TheFooter />
    </div>
</template>

<script>
    import TheFooter from "../components/TheFooter.vue"
    import TheHeader from "../components/TheHeader.vue"

    export default {
        name: 'song-list',
        components: {
            TheHeader,
            TheFooter
        }
    }
</script>