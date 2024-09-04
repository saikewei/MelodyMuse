<template>
    <transition name="slide-fade">
        <div class="the-aside">
            <h2 class="title">播放列表</h2>
            <ul class="menus">
                <li v-for="(item, index) in listOfSongs" :key="index" :class="{'is-play': id === item}"
                    @click="toplay(item)">
                    {{ item }} <!-- 显示歌曲ID -->
                </li>
            </ul>
        </div>
    </transition>
</template>

<script>
    import { mapGetters } from 'vuex';

    export default {
        name: 'the-aside',
        computed: {
            ...mapGetters([
                'listOfSongs',  // 当前歌曲ID列表
                'id',           // 当前播放的歌曲ID
            ])
        },
        methods: {
            // 播放
            toplay(item) {
                // 更新播放中的歌曲ID
                this.$store.commit('setId', item);

                // 在此处添加其他与播放相关的逻辑（如果有的话）
            },
        }
    }
</script>

<style scoped>
    .the-aside {
        position: fixed;
        left: 800px;
        top: 500px;
    }

    .menus {
        list-style: none;
        padding: 0;
    }

        .menus li {
            padding: 10px;
            cursor: pointer;
        }

            .menus li.is-play {
                background-color: #f0f0f0;
                font-weight: bold;
            }
</style>
