<template>
    <div id="app">
      <Player />
    </div>
</template>
  
<script>
  // mediaplayer.vue
    import Player from '../components/player.vue';
  export default {
    components: {
      Player
    },
  };
</script>
  
<style>
  #app {
    padding: 20px;
  }
</style>
  