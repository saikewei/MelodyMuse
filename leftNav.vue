<template>
    <!--×ó±ßµ¼º½À¸-->
    <div class='leftNav'>{{msg}}</div>
</template>

<script>
export default {
  name: 'leftNav',
  data(){
      return{
          msg:''
      }
  }
}
</script>

<style scoped>
 /*×ó±ßµ¼º½À¸*/
.leftNav{
    width:210px;
    height:100%;
    background:#96c9d6;
    position:absolute;
    left:0;
    top:0;
}
</style>