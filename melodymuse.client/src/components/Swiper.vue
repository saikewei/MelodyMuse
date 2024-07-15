<template>
    <div class="carousel-wrapper">
        <div class="carousel">
            <div class="carousel-container" :style="{ transform: `translateX(-${currentIndex * 100}%)` }">
                <div class="carousel-item" v-for="(slide, index) in slides" :key="index">
                    <img :src="slide.src" :alt="slide.alt">
                </div>
            </div>
            <div class="carousel-controls">
                <button @click="prevSlide" class="arrow left">&#9664;</button>
                <button @click="nextSlide" class="arrow right">&#9654;</button>
            </div>
            <div class="page-indicator">
                <span v-for="(slide, index) in slides" :key="index"
                      class="page-dot"
                      :class="{ active: index === currentIndex }"
                      @click="showSlide(index)">
                </span>
            </div>
        </div>
    </div>
</template>

<script>
    import { slides } from "../assets/data/swiper";

    export default {
        name: 'swiper',
        data() {
            return {
                currentIndex: 0,
                slides: [], // 使用 slides
                autoPlayInterval: null,
                intervalDuration: 3000,
                carouselHeight: '50vh' // 设置轮播组件高度为页面上半部分的50%
            };
        },
        created() {
            this.slides = slides; // 使用 slides
        },
        methods: {
            showSlide(index) {
                this.currentIndex = (index + this.slides.length) % this.slides.length; // 使用 slides
            },
            nextSlide() {
                this.showSlide(this.currentIndex + 1);
            },
            prevSlide() {
                this.showSlide(this.currentIndex - 1);
            },
            startAutoPlay() {
                this.autoPlayInterval = setInterval(this.nextSlide, this.intervalDuration);
            },
            stopAutoPlay() {
                clearInterval(this.autoPlayInterval);
            }
        },
        mounted() {
            this.startAutoPlay();
        },
        beforeDestroy() {
            this.stopAutoPlay();
        }
    };
</script>

<style>
    html, body {
        height: 100%; /* 确保html和body元素占据整个视口 */
        margin: 0;
        padding: 0;
    }

    .carousel-wrapper {
        position: absolute; /* 使轮播组件可以定位 */
        top: 100px; /* 距离页面顶部的距离 */
        left: 50%; /* 水平居中 */
        transform: translateX(-50%); /* 水平居中 */
        height: 50vh; /* 设置轮播组件在页面上方的高度为页面上半部分的50% */
        width: 150vh; /* 设置轮播组件的宽度 */
        overflow: hidden; /* 隐藏溢出部分 */
    }

    .carousel {
        width: 100%;
        height: 100%; /* 设置轮播容器高度为100% */
        overflow: hidden;
        position: relative;
    }

    .carousel-container {
        display: flex;
        transition: transform 0.5s ease;
        height: 100%; /* 设置容器高度为100% */
    }

    .carousel-item {
        flex: 0 0 100%; /* Each carousel item takes up 100% width */
        height: 100%; /* 设置轮播项高度为100% */
    }

    .carousel img {
        width: 100%; /* 图片宽度100% */
        height: 100%; /* 图片高度100% */
        object-fit: cover; /* 保持图片比例填充 */
    }

    /* Left and right arrow styles */
    .carousel-controls {
        position: absolute;
        top: 45%;
        width: 100%;
        display: flex;
        justify-content: space-between;
    }

        .carousel-controls button {
            background: rgba(0, 0, 0, 0.5);
            color: white;
            border: none;
            padding: 10px;
            cursor: pointer;
            font-size: 18px;
            outline: none;
            position: absolute; /* 使按钮定位 */
            top: 50%; /* 垂直居中 */
            transform: translateY(-50%);
        }

        .carousel-controls .arrow.left {
            left: 10px;
        }

        .carousel-controls .arrow.right {
            right: 10px;
        }

    /* Page indicator styles */
    .page-indicator {
        position: absolute;
        bottom: 10px;
        left: 50%;
        transform: translateX(-50%);
        display: flex;
    }

    .page-dot {
        width: 10px;
        height: 10px;
        background-color: #ccc;
        border-radius: 50%;
        margin: 0 5px;
        cursor: pointer;
    }

        .page-dot.active {
            background-color: #555;
        }
</style>
