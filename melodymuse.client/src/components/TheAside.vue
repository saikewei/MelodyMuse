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
        <el-menu-item index="/personal-info">
          <span>个人信息</span>
        </el-menu-item>
        <el-menu-item index="/statistics">
          <span>统计信息</span>
        </el-menu-item>
        <div v-if="status=='admin'">
          <el-menu-item index="/check-song">
          <span>歌曲审核</span>
        </el-menu-item>
        <el-menu-item index="/song-info">
          <span>歌曲管理</span>
        </el-menu-item>
        <el-menu-item index="/uploadSong">
          <span>上传歌曲</span>
        </el-menu-item>
        <el-menu-item index="/usermanage">
          <span>用户管理</span>
        </el-menu-item>
        </div>
      </el-menu>
</div>
</template>

<script>
import { ElMenu } from 'element-plus';
import { useRoute, useRouter } from 'vue-router'
import { ref } from 'vue';

export default{
    components: {
        ElMenu
    },
    data() {
      return {
        currentRoute: '',
        status: '',
      }
    },
    created() {
      this.currentRoute = '/'+useRoute().path.split('/').pop();
      this.status = useRoute().params['status'];
      if(this.status!='normal'&&this.status!='admin'){
        useRouter().push('/404');
      }
    }, 
    setup() {
    const router = useRouter()
    const activeIndex = '2' // Set this to your default active menu item
    const status = ref(useRoute().params['status']);

    console.log(status);

    const handleSelect = (index) => {
      console.log(index);
      router.push(`/user/${status.value}`+index);
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
