<template>
    <header>
        <TheHeader />
    </header>
    <newSong />
    <footer>
        <TheFooter />
    </footer>

</template>
  
<script>
    import TheHeader from "@/components/TheHeader.vue";
    import TheFooter from "@/components/TheFooter.vue";
    import newSong from "../components/newSong.vue";
    export default {
        components:{
            TheFooter,
            TheHeader,
            newSong
        },
        data(){
            return{
                  
            }
        }
    }
</script>
  
<style>
  
</style>