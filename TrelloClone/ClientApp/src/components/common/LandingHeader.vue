<template>
  <v-toolbar color="primary" fixed height="80em" dark :class="{'on-top':isOnTop}"
  >
  <v-card>
    <v-img
      :src="require('../../images/trello-logo-white.svg')"
      max-width="130"
      @click="logoClick"
      class="clickable"
    ></v-img>
  </v-card>
    <v-spacer></v-spacer>
    <v-layout justify-end>
      <template v-if="isLogin === true">
        <v-btn to="/board" color="blue darken-3" dark round>Go to Your Boards</v-btn>
      </template>
      <template v-else-if="isLogin === false">
        <v-btn to="/login" flat round>Log in</v-btn>
        <v-btn to="/register" color="white" light round class="primary--text">Sign Up</v-btn>
      </template>
    </v-layout>
  </v-toolbar>
</template>

<script>
import UrlConfig from '../../api/UrlConfig';

export default {
  name: "landingheader",
  data: function(){
    return {
      isLogin: null,
      isOnTop: window.scrollY == 0
    }
  },
  methods: {
    logoClick: function() {
      this.$router.push("/");
    },
    handleScroll (e) {
      this.isOnTop = window.scrollY == 0;
    }
  },
  beforeCreate: function(){
    this.$axios.get(UrlConfig.auth.checkToken)
      .then((response) => {
          this.isLogin = true;
      })
      .catch((ex) => {
          this.isLogin = false;
      })
  },
  created () {
    window.addEventListener('scroll', this.handleScroll);
  },
  destroyed () {
    window.removeEventListener('scroll', this.handleScroll);
  }
};
</script>

<style scoped>
.clickable {
  cursor: pointer;
}
.on-top{
  background: rgba(155,94,223,0) !important;
  box-shadow: 0 0 10px rgba(0,0,0,0);
  transition: background 1s ease 0s,box-shadow 1s ease 0s;
}
</style>
