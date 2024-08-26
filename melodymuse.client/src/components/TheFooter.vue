<template>
    <div class="the-footer">
        <p>关于 | 帮助 | 条款 | 反馈</p>
        <p>Copyright ©2024</p>
    </div>
</template>

<script>
    export default {
        name: 'TheFooter'
    }
</script>

<style scoped>
    .the-footer {
        position: absolute; /* 或者 position: absolute; */
        bottom: 50px; /* 与底部的距离 */
        left: 50%; /* 左侧距离为页面宽度的一半 */
        transform: translateX(-50%); /* 水平居中 */
        background-color: #f0f0f0; /* 背景色 */
        display: flex;
        justify-content: center; /* 水平居中 */
        align-items: center; /* 垂直居中 */
        width: 85%; /* 宽度 */
        flex-direction: column; /* 垂直布局 */
        z-index: 999; /* 层级 */
        padding: 10px; /* 内边距 */
    }

        .the-footer p {
            height: 25px; /* 段落高度 */
            margin: 5px 0; /* 外边距 */
        }
</style>