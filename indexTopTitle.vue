<template>
    <!--顶部title条-->
    <div class='indexTopTitle'>{{msg}}</div>
</template>

<script>
export default {
  name: 'indexTopTitle',
  data(){
      return{
          msg:''
      }
  }
}
</script>

<style scoped>
/*顶部title条*/
.indexTopTitle {
    width: 100%;
    height: 60px;
    background: #b5e4e9;
}
</style>