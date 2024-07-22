<template>
    <div>
        <h1>User IDs</h1>
        <table border="1" style="width: 100%">
            <thead>
                <tr>
                    <th>User ID</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="userId in userIds" :key="userId">
                    <td>{{ userId }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    // 创建响应式变量以存储用户ID列表
    const userIds = ref([]);

    // 定义获取用户ID的函数
    const fetchUserIds = async () => {
        try {
            // 调用后端API
            const response = await axios.get('https://localhost:7223/api/users');
            // 直接将数据赋值给 userIds
            userIds.value = response.data;
        } catch (error) {
            console.error('Failed to fetch user IDs:', error);
        }
    };

    // 在组件挂载时调用fetchUserIds函数
    onMounted(fetchUserIds);
</script>

<style scoped>
    table {
        border-collapse: collapse;
        width: 100%;
    }

    th, td {
        border: 1px solid #ddd;
        padding: 8px;
    }

    th {
        padding-top: 12px;
        padding-bottom: 12px;
        text-align: left;
        background-color: #f2f2f2;
    }
</style>
