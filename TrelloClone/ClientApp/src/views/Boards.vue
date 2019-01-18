<template>
    <v-app>
        <v-container class="my-5 py-5">
            <v-layout>
                <v-flex text-xs-center>
                    <h1>Hello Trello</h1>
                    <v-btn color="info" @click="testAuth">Who am I?</v-btn>
                    <v-btn color="success" to="/home">Home</v-btn>
                    <v-btn color="warning" @click="logout">Log Out</v-btn>
                </v-flex>
            </v-layout>
        </v-container>
    </v-app>
</template>

<script>
import UrlConfig from '../api/UrlConfig.js'

export default {
    name: "boards",
    methods: {
        testAuth:function(){
            this.$axios.get(UrlConfig.test.whoami)
            .then((response) => {
                alert(response.data);
            })
        },
        logout: function(){
            this.$axios.get(UrlConfig.auth.logout)
            .then((response) => {
                this.$router.push("/");
            })
        }
    }
}
</script>

<style scoped>
</style>

