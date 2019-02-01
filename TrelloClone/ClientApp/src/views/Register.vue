<template>
  <v-app>
    <v-container class="mt-5">
      <v-layout column class="mw-4 mb-5 pb-5" style="margin:0 auto;">
        <v-flex>
          <div class="display-1 font-weight-bold">Create a Trello Account</div>
          <p class="title font-weight-light lh-1 ls-1">or <router-link to="/login">sign in to your account</router-link></p>
        </v-flex>
        <v-flex>
          <v-text-field 
            label="Name"
            data-vv-name="name"
            v-model="user.name"
            v-validate="'required|max:20'"
            :error-messages="errors.collect('name')"
            placeholder="e.g., Calvin and Hobbes"
            :counter="20"
            @keyup.enter="createAccount"
            ref="name"
          ></v-text-field>
          <v-text-field 
            label="Email address"
            data-vv-name="email"
            v-model="user.email"
            v-validate="{'is_not': duplicateEmail, required: true, email: true}"
            :error-messages="errors.collect('email')"
            placeholder="e.g., Calvin@gross.club"
            @keyup="checkDuplication"
          ></v-text-field>
          <v-text-field
            label="Password"
            data-vv-name="password"
            v-model="user.password"
            :append-icon="passwordShow ? 'visibility_off' : 'visibility'"
            :type="passwordShow ? 'text' : 'password'"
            hint="at least one letter, one number and one special character"
            placeholder="e.g., ••••••••••••"
            v-validate="{ required: true, regex: /^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]+$/, min:8, max:15 }"
            :counter="15"
            :error-messages="errors.collect('password')"
            @click:append="passwordShow = !passwordShow"
            @keyup.enter="createAccount"
          ></v-text-field>
          <v-text-field
            label="Password Confirm"
            data-vv-name="password_confirm"
            v-model="user.password_confirm"
            :append-icon="passwordShow ? 'visibility_off' : 'visibility'"
            :type="passwordShow ? 'text' : 'password'"
            hint="Enter password again"
            placeholder="e.g., ••••••••••••"
            v-validate="{is: user.password, required: true}"
            :counter="15"
            :error-messages="errors.collect('password_confirm')"
            @click:append="passwordShow = !passwordShow"
            @keyup.enter="createAccount"
          >
          </v-text-field>
          <v-btn color="success" large class="my-5 right" @click="createAccount" :disabled="isLoading">Create New Account</v-btn>
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
  name: "register",
  components: {LandingFooter},
  data: function() {
    return {
      user: {
        name: "",
        email: "",
        password: "",
        password_confirm: ""
      },
      duplicateEmail: "",
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
    // 계정생성
    createAccount: function() {
      this.$validator
        .validate()
        .then(valid => {
          if (!valid) {
            throw "Unvalid";
          }
          return this.$axios.post(UrlConfig.auth.register, this.user);
        })
        .then(response => {
          this.$router.push("/board");
        })
        .catch(ex => {
          alert(ex);
        });
    },

    // 중복체크
    checkDuplication: _.debounce(function(event) {
      if (event.keyCode == 13) {
        this.createAccount();
      } else {
        this.$validator
          .validate("email")
          .then(valid => {
            if (!valid) {
              this.duplicateEmail = "";
              throw "Notvalid";
            }
            return this.$axios.post(UrlConfig.auth.checkDuplication, this.user);
          })
          .then(response => {
            this.duplicateEmail = response.data;
          })
          .catch(ex => {
            this.duplicateEmail = this.user.email;
          });
      }
    }, 300)
  },
  mounted: function(){
    this.$refs.name.focus();
  }
};
</script>

<style scoped>
.mw-4{
  max-width: 500px !important;
}
</style>