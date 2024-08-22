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
        background-color: #f0f0f0; /* 背景色 */
        display: flex;
        justify-content: center; /* 水平居中 */
        align-items: center; /* 垂直居中 */
        width: 100%; /* 占据整个页面宽度 */
        flex-direction: column; /* 垂直布局 */
        padding: 10px; /* 内边距 */
        margin-top: 100px; /* 与页面内容的间距 */
    }

        .the-footer p {
            margin: 5px 0; /* 外边距 */
            text-align: center; /* 文字居中 */
        }
</style>
