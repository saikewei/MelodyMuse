<template>
  <div id="app">
    <register_comp />
    <div class="title">
      <div class="btn-login" @click="$router.push('/login')">已注册，点击登录</div>   
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

.btn-login {  
    position:absolute;
    top:650px; 
    left:37%;
    background-color: transparent;
    border-radius:10px;
    display:inline-block;
    cursor:pointer;
    color:#6a8cdcc1;
    font-family:Arial;
    font-size:20px;
    padding:16px 31px;
    text-decoration:none;
    margin: 10px 20px;  
}
.btn-login:hover {
    color:#284da0c1;
}
.btn-login:active {
    color:#284da0c1;
}
.btn-register {   
    position:absolute;
    top:630px; 
    left:37%;
    background-color: transparent;
    border-radius:10px;
    display:inline-block;
    cursor:pointer;
    color:#6a8cdcc1;
    font-family:Arial;
    font-size:20px;
    padding:16px 31px;
    text-decoration:none;
    margin: 10px 20px;  
}
.btn-register:hover {
    color:#284da0c1;
}
.btn-register:active {
    color:#284da0c1;
}
</style>