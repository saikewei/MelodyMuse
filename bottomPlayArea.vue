<template>
    <!--底部播放区-->
    <div class='bottomPlayArea'>{{msg}}</div>
</template>

<script>export default {
  name: 'bottomPlayArea',
  data(){
      return{
          msg:''
      }
  }
}</script>


<style scoped>
 /*底部播放区*/
 .bottomPlayArea {
     width: 100%;
     height: 60px;
     background: #b5e4e9;
 }
</style>