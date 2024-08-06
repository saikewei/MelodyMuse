<template>
    <div>
        <h1 class="title">User Information</h1>
        <div class="table-container">
            <el-table :data="users" style="width: 100%">
                <el-table-column prop="userId" label="User ID" width="150" header-align="center" align="center" />
                <el-table-column prop="userName" label="User Name" width="250" header-align="center" align="center" />
                <el-table-column label="User Status" width="150" header-align="center" align="center">
                    <template #default="scope">
                        {{ getStatusText(scope.row.userStatus) }}
                    </template>
                </el-table-column>
                <el-table-column label="Actions" width="500" header-align="center" align="center">
                    <template #default="scope">
                        <el-button @click="toggleBanUser(scope.row)"
                                   :type="scope.row.userStatus === '0' ? 'primary' : 'danger'"
                                   size="mini"
                                   class="action-button">
                            {{ scope.row.userStatus === '0' ? 'Unban User' : 'Ban User' }}
                        </el-button>
                        <el-button @click="toggleAdminUser(scope.row)"
                                   :type="scope.row.userStatus === '2' ? 'warning' : 'success'"
                                   size="mini"
                                   class="action-button">
                            {{ scope.row.userStatus === '2' ? 'Demote Admin' : 'Promote to Admin' }}
                        </el-button>
                    </template>
                </el-table-column>
            </el-table>
        </div>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const users = ref([]);

    const fetchUserIds = async () => {
        try {
            const response = await axios.get('http://localhost:7223/api/users');
            return response.data;
        } catch (error) {
            console.error('Failed to fetch user IDs:', error);
            return [];
        }
    };

    const fetchUserDetails = async (userId) => {
        try {
            const response = await axios.get(`http://localhost:7223/api/users/${userId}`);
            return response.data;
        } catch (error) {
            console.error(`Failed to fetch details for user ID ${userId}:`, error);
            return null;
        }
    };

    const fetchUsers = async () => {
        try {
            const userIds = await fetchUserIds();
            const userPromises = userIds.map((userId) => fetchUserDetails(userId));
            const userDetails = await Promise.all(userPromises);
            users.value = userDetails.filter((user) => user !== null);
        } catch (error) {
            console.error('Failed to fetch users:', error);
        }
    };

    const updateUserStatus = async (userId, newStatus) => {
        try {
            const response = await axios.put(`http://localhost:7223/api/users/${userId}/updateStatus?newStatus=${newStatus}`);
            console.log(response.data.msg);
            const user = users.value.find((u) => u.userId === userId);
            if (user) {
                user.userStatus = newStatus;
            }
        } catch (error) {
            console.error('Failed to update user status:', error);
        }
    };

    const getStatusText = (status) => {
        switch (status) {
            case '0':
                return 'Banned';
            case '1':
                return 'User';
            case '2':
                return 'Admin';
            default:
                return 'Unknown';
        }
    };

    const toggleBanUser = (user) => {
        const newStatus = user.userStatus === '0' ? '1' : '0';
        updateUserStatus(user.userId, newStatus);
    };

    const toggleAdminUser = (user) => {
        const newStatus = user.userStatus === '2' ? '1' : '2';
        updateUserStatus(user.userId, newStatus);
    };

    onMounted(fetchUsers);
</script>

<style scoped>
    .table-container {
        width: 100%;
        margin: 0 auto;
    }

    .title {
        text-align: center;
    }

    .el-table th, .el-table td {
        text-align: center;
    }

    .action-button {
        width: 150px;
        text-align: center;
    }
</style>
