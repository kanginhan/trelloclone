<template>
  <v-app>
    <v-container class="mt-5">
      <v-layout column class="mw-4 mb-5 pb-5" style="margin:0 auto;">
        <v-flex>
          <div class="display-1 font-weight-bold">Log in to Trello</div>
          <p class="title font-weight-light lh-1 ls-1">or <router-link to="/register">create an account</router-link></p>
        </v-flex>
        <v-flex>
          <v-text-field 
            label="Email address"
            v-model="user.email"
            placeholder="e.g., Calvin@gross.club"
            @keyup.enter="login"
            ref="email"
          ></v-text-field>
          <v-text-field
            label="Password"
            v-model="user.password"
            :append-icon="passwordShow ? 'visibility_off' : 'visibility'"
            :type="passwordShow ? 'text' : 'password'"
            placeholder="e.g., ••••••••••••"
            @click:append="passwordShow = !passwordShow"
            @keyup.enter="login"
          ></v-text-field>
          <v-btn color="success" large class="my-5 right" @click="login" :disabled="isLoading">Log In</v-btn>
        </v-flex>
      </v-layout>
      <landing-footer></landing-footer>
    </v-container>
  </v-app>
</template>

<script>
import UrlConfig from "../api/UrlConfig.js";
import _ from "lodash";
import LandingFooter from "../components/common/LandingFooter.vue"

export default {
  name: "login",
  components: {LandingFooter},
  data: function() {
    return {
      user: {
        name: "",
        email: "",
        password: ""
      },
      passwordShow: false,
      nprogressObj: this.$nprogress
    };
  },
  computed: {
    isLoading: function(){
      return this.nprogressObj.isStarted();
    }
  },
  methods: {
    login: function() {
      this.$axios.post(UrlConfig.auth.login, this.user)
      .then((response) => {
        this.$router.push("/board");
      })
      .catch((ex) => {
        alert(ex);
      })
    }
  },
  mounted: function(){
    this.$refs.email.focus();
  }
};
</script>

<style scoped>
.mw-4{
  max-width: 500px !important;
}
</style>