<template>
    <div>
        <TheHeader />
        <TheFooter />

        我的音乐
    </div>
</template>

<script>
    import TheFooter from "../components/TheFooter.vue"
    import TheHeader from "../components/TheHeader.vue"

    export default {
        name: 'my-music',
        components: {
            TheHeader,
            TheFooter
        }
    }
</script>