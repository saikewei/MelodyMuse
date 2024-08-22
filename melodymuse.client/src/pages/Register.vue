<template>
  <div id="app">
    <register_comp />
    <div class="title">
        <div class="btn" @click="$router.push('/login')">登录</div>
        <div class="btn" @click="$router.push('/register')">注册</div>
    </div>
  </div>
</template>
 
<script>
//避免页面和组件重名导致的递归调用，改为_comp后缀
import register_comp from '../components/register.vue'
export default {
  name: 'Register',
  components: {
    register_comp
  }
}
</script>
 
<style>
.title{
    text-align: center;
    background-color:transparent;
}

.btn {   
    background-color: transparent;
    border-radius:10px;
    display:inline-block;
    cursor:pointer;
    color:#bb8bbe;
    font-family:Arial;
    font-size:20px;
    padding:16px 31px;
    text-decoration:none;
    margin: 10px 20px;  
}
.btn:hover {
    background-color:#f9f7f9;
}
.btn:active {
    position:relative;
    top:1px;
}
</style>