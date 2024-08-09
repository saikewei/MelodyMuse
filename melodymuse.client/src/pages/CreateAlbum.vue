<template>
    <header>
        <TheHeader />
    </header>
    <main>
        <newAlbum />
    </main>
    
    <footer>
        <TheFooter />
    </footer>

</template>

<script>
    import newAlbum from "../components/newAlbum.vue";
    import TheHeader from "@/components/TheHeader.vue";
    import TheFooter from "@/components/TheFooter.vue";

    export default {
        components:{
            TheHeader,
            TheFooter,
            newAlbum
        },
        data(){
            return{
                
            }
        }
    }
</script>

<style>
header {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    z-index: 1000;
}

footer {
    position: absolute;
    bottom: 0;
    left: 0;
    width: 100%;
    z-index: 1000;
}

main {
    margin-top: 60px; /* 或者 padding-top: 60px; */
    margin-bottom: 40px; /* 或者 padding-bottom: 40px; */
}
</style>