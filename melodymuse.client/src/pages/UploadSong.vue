<template>
    <header>
        <SimpleHeader current-page="上传歌曲"/>
    </header>
    <aside>
        <TheAside />

    </aside>
    <main>
        <newSong />
    </main>


</template>
  
<script>
    import TheHeader from "@/components/TheHeader.vue";
    import TheFooter from "@/components/TheFooter.vue";
    import newSong from "../components/newSong.vue";
    import TheAside from "@/components/TheAside.vue";
    import SimpleHeader from "@/components/SimpleHeader.vue";
    import { useRoute, useRouter } from 'vue-router';

    export default {
        components:{
            TheFooter,
            TheHeader,
            newSong,
            TheAside,
            SimpleHeader
        },
        data(){
            return{
                  
            }
        },
        mounted(){
            if(useRoute().params['status']!='admin'){
                useRouter().push("/404");
            }
        }
    }
</script>
  
<style>
  
aside{

}
main{
    position: relative; /* 或 absolute */
    left: 20px; /* 将元素向右移动20px */
    margin-left: 80px;
    padding-left: 20px;
}

</style>