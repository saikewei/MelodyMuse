<template>
<!--最外框-->
    <div class="wrap">
        <!--左边导航栏-->
        <leftNav/>
        <!--顶部title条-->
        <indexTopTitle/>
        <!--中间音乐区-->
        <musicArea/>
        <!--底部播放器-->
        <bottomPlayArea />
</div>
</template>

<script>
import leftNav from './leftNav/leftNav.vue'
import indexTopTitle from './indexTopTitle/indexTopTitle.vue'
import bottomPlayArea from './bottomPlayArea/bottomPlayArea.vue'
import musicArea from './musicArea/musicArea.vue'
export default {
  name: 'HelloWorld',
  data(){
      return{
          msg:'MusicMuse'
      }
  },
  components:{leftNav,indexTopTitle,bottomPlayArea,musicArea}
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
/*最外框*/
.wrap{
    width:810px;
    height:690px;
    margin:30px auto;
    background:#ffffff;
    position:relative;
    padding-left:210px;
}

</style>
