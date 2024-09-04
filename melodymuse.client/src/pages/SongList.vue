<template>
    <div>
        <TheHeader />
        <AllSongList />
    </div>
</template>

<script>
    import TheHeader from "../components/TheHeader.vue"
    import AllSongList from "@/components/AllSongList.vue";

    export default {
        name: 'song-list',
        components: {
            TheHeader,
            AllSongList
        }
    }
</script>