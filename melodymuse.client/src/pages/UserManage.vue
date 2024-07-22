<template>
    <div>
        <h1>User Information</h1>
        <table>
            <thead>
                <tr>
                    <th>User ID</th>
                    <th>User Name</th>
                    <th>User Status</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <tr v-for="user in users" :key="user.userId">
                    <td>{{ user.userId }}</td>
                    <td>{{ user.userName }}</td>
                    <td>{{ getStatusText(user.userStatus) }}</td>
                    <td>
                        <button @click="toggleBanUser(user)"
                                :class="{'btn-ban': user.userStatus !== '0', 'btn-unban': user.userStatus === '0'}">
                            {{ user.userStatus === '0' ? 'Unban User' : 'Ban User' }}
                        </button>
                        <button @click="toggleAdminUser(user)"
                                :class="{'btn-promote': user.userStatus !== '2', 'btn-demote': user.userStatus === '2'}">
                            {{ user.userStatus === '2' ? 'Demote Admin' : 'Promote to Admin' }}
                        </button>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import axios from 'axios';

    const users = ref([]);

    const fetchUserIds = async () => {
        try {
            const response = await axios.get('https://localhost:7223/api/users');
            return response.data;
        } catch (error) {
            console.error('Failed to fetch user IDs:', error);
            return [];
        }
    };

    const fetchUserDetails = async (userId) => {
        try {
            const response = await axios.get(`https://localhost:7223/api/users/${userId}`);
            return response.data;
        } catch (error) {
            console.error(`Failed to fetch details for user ID ${userId}:`, error);
            return null;
        }
    };

    const fetchUsers = async () => {
        try {
            const userIds = await fetchUserIds();
            const userPromises = userIds.map(userId => fetchUserDetails(userId));
            const userDetails = await Promise.all(userPromises);
            users.value = userDetails.filter(user => user !== null);
        } catch (error) {
            console.error('Failed to fetch users:', error);
        }
    };

    const updateUserStatus = async (userId, newStatus) => {
        try {
            const response = await axios.put(`https://localhost:7223/api/users/${userId}/updateStatus?newStatus=${newStatus}`);
            console.log(response.data.msg);
            const user = users.value.find(u => u.userId === userId);
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

    button {
        margin: 5px;
        padding: 5px 10px;
        border: none;
        cursor: pointer;
        border-radius: 5px;
        font-size: 14px;
    }

    .btn-ban {
        background-color: #808080;
        color: white;
    }

    .btn-unban {
        background-color: red;
        color: white;
    }

    .btn-promote {
        background-color: #808080;
        color: white;
    }

    .btn-demote {
        background-color: blue;
        color: white;
    }

    button:hover {
        opacity: 0.8;
    }
</style>








