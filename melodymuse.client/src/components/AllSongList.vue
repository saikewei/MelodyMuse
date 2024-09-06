<template>
    <div class="playlist-container">
        <div class="header">
            <p class="header-text">我的歌单</p>

            <div class="buttons">
                <button @click="openCreateDialog" class="custom-button create-button">
                    <h5>创建歌单</h5>
                </button>
            </div>
        </div>

        <!-- 分隔线 -->
        <div class="separator"></div>
        <el-row :gutter="20" v-if="songlists.length > 0" class="playlist-row">
            <el-col :span="6" v-for="(playlist, index) in songlists" :key="playlist.songlistId">
                <el-card class="playlist-card" shadow="hover">
                    <el-image :src="getCoverImage(index)"
                              fit="cover"
                              class="cover-image"
                              lazy
                              :preview-src-list="previewList"></el-image>
                    <div class="playlist-content">
                        <div class="playlist-header">
                            <span>{{ playlist.songlistName }}</span>
                        </div>
                        <div class="playlist-footer">
                            <button class="custom-button play-button" @click="playSongList(playlist)">播放</button>
                            <button class="custom-button edit-button" @click="openEditDialog(playlist)">编辑</button>
                            <button class="custom-button details-button" @click="viewDetails(playlist)">详情</button>
                            <button class="custom-button delete-button" @click="confirmDelete(playlist.songlistId)">删除</button>
                        </div>
                    </div>
                </el-card>
            </el-col>
        </el-row>
        <div v-else-if="loading" class="loading-message">
            <p>加载歌单中...</p>
        </div>
        <div v-else class="loading-message">
            <p>未创建任何歌单</p>
        </div>

        <el-dialog v-model="dialogVisible" width="700px">
            <div>歌单信息</div>
            <el-divider direction="horizontal"></el-divider>
            <el-form :model="currentPlaylist" ref="formRef" label-width="90px" :rules="rules">
                <el-form-item label="歌单名称" prop="songlistName">
                    <el-input v-model="currentPlaylist.songlistName" />
                </el-form-item>
                <el-form-item label="是否公开">
                    <el-checkbox v-model="currentPlaylist.isPublic">公开</el-checkbox>
                </el-form-item>
                <el-divider direction="horizontal"></el-divider>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="dialogVisible = false">取 消</el-button>
                <el-button type="primary" @click="savePlaylist">确 定</el-button>
            </span>
        </el-dialog>
    </div>
</template>


<script>
    import api from '../api/http.js';
    import { ElMessageBox, ElMessage } from 'element-plus';
    import img1 from '../assets/pics/song1.jpg';
    import img2 from '../assets/pics/song2.jpg';
    import img3 from '../assets/pics/song3.jpg';
    import img4 from '../assets/pics/song4.jpg';
    import img5 from '../assets/pics/song5.jpg';
    import img6 from '../assets/pics/song6.jpg';
    import img7 from '../assets/pics/song7.jpg';
    import img8 from '../assets/pics/song8.jpg';
    import img9 from '../assets/pics/song9.jpg';
    import img10 from '../assets/pics/song10.jpg';

    export default {
        name: 'PlaylistComponent',
        data() {
            return {
                images: [img1, img2, img3, img4, img5, img6, img7, img8, img9, img10], // 图片数组
                songlists: [],
                loading: true,
                dialogVisible: false,
                currentPlaylist: {},
                rules: {
                    songlistName: [
                        { required: true, message: '歌单名称不能为空', trigger: 'blur' }
                    ]
                },
                previewList: [img1, img2, img3, img4, img5, img6, img7, img8, img9, img10], // 预览图片列表
            };
        },
        methods: {
            async fetchSongList() {
                try {
                    const response = await api.apiClient.get('/api/songlist/getall');
                    this.songlists = response.data;
                } catch (error) {
                    console.error('Failed to fetch songlists:', error);
                } finally {
                    this.loading = false;
                }
            },

            getCoverImage(index) {
                // 使用模组化图片数组中的图片
                return this.images[index % this.images.length];
            },

            viewDetails(playlist) {
                this.$router.push(`/SonglistDetail/${playlist.songlistId}`);
            },

            openCreateDialog() {
                this.currentPlaylist = {}; // 重置当前歌单
                this.dialogVisible = true;
            },

            openEditDialog(playlist) {
                this.currentPlaylist = { ...playlist }; // 填充当前歌单数据
                this.dialogVisible = true;
            },

            async playSongList(playlist) {
                try {
                    const songsResponse = await api.apiClient.get(`/api/songlist/${playlist.songlistId}/songs`);
                    if (songsResponse.status < 400) {
                        const songList = songsResponse.data.map(s => s.songId);
                        this.$store.commit('setListOfSongs', songList);
                        // 更新当前播放的歌曲 ID
                        this.$store.commit('setId', songsResponse.data[0].songId);

                        // 使用 Vue Router 导航到 mediaplayer 页面，并传递歌曲 ID 和歌曲列表
                        this.$router.push({
                            name: 'mediaplayer',
                            params: {
                                songId: songsResponse.data[0].songId, // 第一个歌曲的 ID
                                songList: songsResponse.data[0].songId   // 所有歌曲 ID 组成的字符串
                            }
                        });
                        //this.$router.push(`/mediaplayer/${songsResponse.data[0].songId}/${songList}`);
                    } else {
                        ElMessage.error("无法播放歌单，歌单没有歌曲");
                    }
                } catch (error) {
                    ElMessage.error("无法播放歌单:" + error);
                }
            },

            async confirmDelete(songlistId) {
                try {
                    await ElMessageBox.confirm(
                        '你确定要删除这个歌单吗？',
                        '确认删除',
                        {
                            confirmButtonText: '确定',
                            cancelButtonText: '取消',
                            type: 'warning'
                        }
                    ).then(async () => {
                        await api.apiClient.delete(`/api/songlist/${songlistId}/delete`);
                        this.fetchSongList(); // 刷新歌单列表
                        ElMessage.success('删除成功');
                    }).catch(() => {
                        ElMessage.info('删除操作已取消');
                    });
                } catch (error) {
                    console.error('Failed to delete playlist:', error);
                }
            },

            async savePlaylist() {
                const form = this.$refs.formRef; // 获取 el-form 实例
                form.validate(async (valid) => {
                    if (valid) {
                        try {
                            const payload = {
                                SonglistName: this.currentPlaylist.songlistName,
                                IsPublic: this.currentPlaylist.isPublic ? '1' : '0'
                            };
                            if (this.currentPlaylist.songlistId) {
                                // 编辑模式
                                await api.apiClient.put(`/api/songlist/${this.currentPlaylist.songlistId}/update`, payload);
                                ElMessage.success('歌单编辑成功');
                            } else {
                                // 创建模式
                                await api.apiClient.post('/api/songlist/add', payload);
                                ElMessage.success('歌单创建成功');
                            }
                            this.fetchSongList(); // 刷新歌单列表
                            this.dialogVisible = false;
                        } catch (error) {
                            console.error('Failed to save playlist:', error);
                        }
                    } else {
                        ElMessage.error('请填写必填项');
                    }
                });
            }
        },
        mounted() {
            this.fetchSongList();
        }
    };
</script>

<style scoped>
    .playlist-card {
        position: relative;
        width: 300px; /* 固定宽度 */
        height: 390px; /* 固定高度 */
        overflow: hidden; /* 隐藏溢出部分 */
        border-radius: 8px; /* 圆角边框 */
        margin-bottom: 20px; /* 歌单之间的间距 */
    }

    .cover-image {
        width: 100%;
        height: 60%; /* 图片占用 60% 高度 */
        border-bottom: 1px solid #e4e4e4; /* 下边框 */
    }

    .playlist-content {
        padding: 10px;
        background-color: #fff;
        height: 40%; /* 内容占用 40% 高度 */
        display: flex;
        flex-direction: column;
        justify-content: space-between; /* 内容区域内的元素分布 */
    }

    .playlist-header {
        font-size: 16px;
        font-weight: bold;
        margin-bottom: 10px;
        text-align: center;
    }

    .playlist-footer {
        display: flex;
        justify-content: space-around;
        align-items: center;
        margin-top: 10px;
    }

    /* 确保按钮样式一致 */
    .custom-button {
        padding: 8px 12px;
        border-radius: 20px;
        border: none;
        background-color: rgba(64, 108, 194, 0.9);
        color: white;
        cursor: pointer;
        font-size: 14px;
        transition: background-color 0.3s, color 0.3s, border-color 0.3s;
    }

        .custom-button:hover {
            background-color: #95ADE0;
        }

        .custom-button:active {
            background-color: #385FAB;
        }

    /* 特殊样式，白色背景 */
    .create-button {
        background-color: white;
        color: rgba(64, 108, 194, 0.9);
    }

        .create-button:hover {
            background-color: #f0f0f0;
        }

        .create-button:active {
            background-color: #e0e0e0;
        }

    .header {
        height: 190px;
        background-color: #284da0c1;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        margin-bottom: 30px; /* 容器与按钮之间的间距 */
    }

    .header-text {
        color: aliceblue;
        font-size: 30px;
        text-align: center;
    }

    .playlist-container {
        margin-top: 0;
        height: calc(100vh - 240px);
        overflow-y: auto;
        padding: 20px;
        box-sizing: border-box;
    }

    .separator {
        border-top: 1px solid #e4e4e4;
        margin: 20px 0;
    }

    .loading-message {
        text-align: center;
        padding: 20px;
    }

    .dialog-footer {
        text-align: right;
    }

    .buttons {
        margin-top: 20px; /* 调整这里的值来增加间距 */
    }

</style>






