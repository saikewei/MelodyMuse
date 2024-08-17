<template>
    <div id="app">
      <MusicPlayer />
    </div>
</template>
  
<script>
  import MusicPlayer from "../components/player.vue"

  export default {
    components: {
      MusicPlayer,
    },
  };
</script>
  
<style>
  #app {
    padding: 20px;
  }
</style>
  