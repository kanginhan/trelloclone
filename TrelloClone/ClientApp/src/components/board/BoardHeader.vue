<template>
  <v-toolbar color="pink" fixed height="50em" dark app>
    <v-layout row align-center>
      <v-btn icon to="/home">
        <v-icon dark>home</v-icon>
      </v-btn>
      <v-text-field flat solo-inverted append-icon="search" label="Search" @keyup="setKeyword"></v-text-field>

      <v-flex offset-xs-4 xs4 text-xs-center>
        <v-img
          :src="require('../../images/trello-logo-white.svg')"
          max-width="100"
          @click="logoClick"
          class="clickable"
          style="margin: auto"
        ></v-img>
      </v-flex>
      <v-flex xs4>
        <v-layout justify-end>
          <v-menu v-model="userMenu" left :nudge-width="200" offset-x>
            <v-btn slot="activator" color="white" light round class="pink--text" icon>
              <v-icon>person</v-icon>
            </v-btn>
            <v-card>
              <div class="text-xs-center py-1">사용자 {{userEmail}}</div>
              <v-divider></v-divider>
              <v-list>
                <v-list-tile @click="logout">
                  <v-list-tile-title>Log Out</v-list-tile-title>
                </v-list-tile>
              </v-list>
            </v-card>
          </v-menu>
        </v-layout>
      </v-flex>
    </v-layout>
  </v-toolbar>
</template>

<script>
import UrlConfig from "../../api/UrlConfig.js";
import StoreConfig from "../../store/StoreConfig";

export default {
  name: "boardheader",
  data: function() {
    return {
      userEmail: "",
      userMenu: false
    };
  },
  methods: {
    logoClick: function(){
      this.$router.push("/home")
    },
    logout: function() {
      this.$axios.get(UrlConfig.auth.logout).then(response => {
        this.$router.push("/");
      });
    },
    setKeyword: _.debounce(function(e){
      this.$store.dispatch(StoreConfig.setKeyword, e.target.value)
    }, 300)
  },
  beforeCreate: function() {
    this.$axios.get(UrlConfig.test.whoami).then(response => {
      this.userEmail = response.data[0];
    });
  }
};
</script>

<style scoped>
.clickable {
  cursor: pointer;
}
</style>