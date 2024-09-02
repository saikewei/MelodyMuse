<template>
    <transition name="fade">
      <div v-if="isVisible" ref="windowRef" class="floating-window" @mousedown="handleMouseDown" @mouseup="handleMouseUp">
        <div class="window-content">
          <button @click="hide" class="close-icon">关闭</button>
          <h3 class="title">{{ title }}</h3>
          <!-- <p>{{ content }}</p> -->
          <slot></slot>
        </div>
      </div>
    </transition>
  </template>
  
  <script setup>
  import { ref, onMounted, onUnmounted } from 'vue';
  
  const props = defineProps({
    title: {
      type: String,
      default: 'Floating Window'
    },
    content: {
      type: String,
      default: 'This is the content of the floating window.'
    }
  });
  
  const emit = defineEmits(['close']);
  
  const isVisible = ref(false);
  const windowRef = ref(null);
  
  // 显示浮窗
  function show() {
    if(isVisible.value){
        isVisible.value = false;
    }
    else{
        isVisible.value = true;
    }

  }
  
  // 隐藏浮窗
  function hide() {
    isVisible.value = false;
    emit('close');
  }
  
  // 鼠标按下时开始拖动
  function handleMouseDown(event) {
    if (!windowRef.value) return;
  
    windowRef.value.style.position = 'absolute';
    const initialX = event.clientX;
    const initialY = event.clientY;
    const initialLeft = windowRef.value.offsetLeft;
    const initialTop = windowRef.value.offsetTop;
  
    const moveWindow = (event) => {
      const newX = initialLeft + event.clientX - initialX;
      const newY = initialTop + event.clientY - initialY;
      windowRef.value.style.left = `${newX}px`;
      windowRef.value.style.top = `${newY}px`;
    };
  
    document.addEventListener('mousemove', moveWindow);
  
    const finishMove = () => {
      document.removeEventListener('mousemove', moveWindow);
      document.removeEventListener('mouseup', finishMove);
    };
  
    document.addEventListener('mouseup', finishMove);
  }
  
  // 鼠标抬起时停止拖动
  function handleMouseUp() {
    if (!windowRef.value) return;
  
    windowRef.value.style.position = 'fixed';
  }
  
  defineExpose({
    show,
    hide
  });
  
  onMounted(() => {
    // 添加事件监听器
    document.addEventListener('mousedown', handleMouseDown);
    document.addEventListener('mouseup', handleMouseUp);
  });
  
  onUnmounted(() => {
    // 移除事件监听器
    document.removeEventListener('mousedown', handleMouseDown);
    document.removeEventListener('mouseup', handleMouseUp);
  });
  </script>
  
  <style scoped>
  .title{
    position: absolute;
    font-size: 20px;
    color: #09cbf7c1;
    font-weight: bolder;
    top: 20px;
    right:100px;

  }

  .floating-window {
    position: fixed;
    top: 0;
    left: 50;
    width: 300px;
    height: 450px;
    background-color: white;
    border: 1px solid #ccc;
    box-shadow: 2px 2px 6px rgba(0, 0, 0, 0.1);
    z-index: 1000;
    display: flex;
    justify-content: center;
    align-items: center;
  }
  
  .window-content {
    text-align: center;
    padding: 20px;
  }
  
  .fade-enter-active,
  .fade-leave-active {
    transition: opacity 0.3s;
  }
  
  .fade-enter-from,
  .fade-leave-to {
    opacity: 0;
  }

  .close-icon{
    position: absolute;
    top: 10px;
    right: 10px;
    cursor: pointer;
    font-size: 10px;
    color: #284da0c1;
  }
  </style>