<template>
    <div>
        <TheHeader />
        歌手
        <TheFooter />
    </div>
</template>

<script>
    import TheFooter from "../components/TheFooter.vue"
    import TheHeader from "../components/TheHeader.vue"

    export default {
        name: 'singer',
        components: {
            TheHeader,
            TheFooter
        }
    }
</script>