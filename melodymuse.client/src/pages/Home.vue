<template>
  <div class="carousel-wrapper">
    <h1>首页</h1>
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
    </div>
  </div>
</template>

<script>
import img1 from "@/pics/1.jpg";
import img2 from "@/pics/2.jpg";
import img3 from "@/pics/3.jpg";
import img4 from "@/pics/4.jpg";
import img5 from "@/pics/5.jpg";
export default {
  name: 'home',
  data() {
    return {
      currentIndex: 0,
      slides: [
          { src: img1 , alt: 'Image 1' },
          { src: img2 , alt: 'Image 2' },
          { src: img3 , alt: 'Image 3' },
          { src: img4 , alt: 'Image 4' },
          { src: img5 , alt: 'Image 5' },
      ],
      autoPlayInterval: null,
      intervalDuration: 3000,
      carouselHeight: '50vh' // 设置轮播组件高度为页面上半部分的50%
    };
  },
  methods: {
    showSlide(index) {
      this.currentIndex = (index + this.slides.length) % this.slides.length;
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
  width: 150vh; /* 设置轮播组件的宽度为100%，以铺满整个视口 */
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
</style>