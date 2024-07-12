<template>
    <!--中间音乐区-->
    <div class='musicArea'>{{msg}}</div>
</template>

<script>export default {
  name: 'musicArea',
  data(){
      return{
          msg:''
      }
  }
}</script>

<style scoped>
/*中间音乐区*/
.musicArea{
    width:100%;
    height:570px;
    background:#defeff;
}
</style>