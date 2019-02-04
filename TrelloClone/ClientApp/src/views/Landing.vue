<template>
  <div></div>
</template>

<script>
import UrlConfig from '../api/UrlConfig';

export default {
  name: "landing",
  beforeCreate: function(){
    this.$axios.get(UrlConfig.auth.checkToken)
      .then((response) => {
          this.$router.push('/boards')
      })
      .catch((ex) => {
        this.$router.push('/home')
      })
  }
};
</script>

<style scoped>
</style>