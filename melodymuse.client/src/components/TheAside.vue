<template>
<div class="sidebar">
    <el-menu
        :default-active='this.currentRoute'
        class="sidebar-el-menu"
        background-color="#334256"
        text-color="#ffffff"
        active-text-color="#20a0ff"
        @select="handleSelect"
      >
        <el-menu-item index="/edit-personal-info">
          <span>个人信息</span>
        </el-menu-item>
        <el-menu-item index="/check-song">
          <span>歌曲审核</span>
        </el-menu-item>
        <el-menu-item index="/song-info">
          <span>歌曲管理</span>
        </el-menu-item>
        <el-menu-item index="/uploadSong">
          <span>上传歌曲</span>
        </el-menu-item>
      </el-menu>
</div>
</template>

<script>
import { ElMenu } from 'element-plus';
import { useRoute, useRouter } from 'vue-router'

export default{
    components: {
        ElMenu
    },
    data() {
      return {
        currentRoute: ''
      }
    },
    created() {
      this.currentRoute = useRoute().path;
    }, 
    setup() {
    const router = useRouter()
    const activeIndex = '2' // Set this to your default active menu item

    const handleSelect = (index) => {
      console.log(index);
      router.push(index);
    }

    return {
      activeIndex,
      handleSelect,
        }
    }
}
</script>

<style scoped>
    .sidebar{
        display: block;
        position: absolute;
        left: 0;
        top: 40px;
        bottom: 0;
        background-color: #334256;
        overflow-y: scroll;
    }

    .sidebar-el-menu{
        width: 150px;
    }
</style>
