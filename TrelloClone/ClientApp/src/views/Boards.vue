<template>
  <v-app>
    <v-content app>
      <v-toolbar color="pink" fixed height="50em" dark app>
        <v-layout row align-center>
          <v-btn icon to="/home">
            <v-icon dark>home</v-icon>
          </v-btn>
          <v-text-field flat solo-inverted append-icon="search" label="Search" ref="search"></v-text-field>

          <v-flex offset-xs-4 xs4 text-xs-center>
            <v-img
              :src="require('../images/trello-logo-white.svg')"
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
                  <div class="text-xs-center py-1">User {{userEmail}}</div>
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

      <v-container class="mw-2 pt-5">
        <v-layout class="pb-2">
          <v-icon small class="pr-3">person</v-icon>
          <span class="subheading">Boards</span>
        </v-layout>
        <v-layout row wrap>
          <v-card
            v-for="board in filteredBoards"
            :key="board.seq"
            dark
            color="green darken-2"
            width="12em"
            height="6em"
            class="clickable mx-2 my-1"
            @click="goBoard(board.hashId)"
          >
            <v-card-title>{{board.boardTitle}}</v-card-title>
          </v-card>
          <v-card
            flat
            color="grey lighten-2"
            width="12em"
            height="6em"
            class="clickable mx-2 my-1 text-xs-center py-3"
            @click="addNewBoard"
          >
            <span class="caption font-weight-medium grey--text text--darken-2">Create New Board...</span>
          </v-card>
        </v-layout>
      </v-container>
    </v-content>
  </v-app>
</template>

<script>
import StoreConfig from "../store/StoreConfig";
import { mapActions } from "vuex";
import UrlConfig from "../api/UrlConfig";
import _ from "lodash";

export default {
  name: "boards",
  data: function() {
    return {
      userEmail: "",
      userMenu: false,
      boards: [
        // { boardSeq: 0, boardTitle: "보드1" },
        // { boardSeq: 1, boardTitle: "보드2" }
      ]
    };
  },
  computed: {
    filteredBoards: function() {
      var keyword = this.$refs.search && this.$refs.search.lazyValue;
      if (keyword) {
        return this.boards.filter(x => x.boardTitle.includes(keyword));
      } else {
        return this.boards;
      }
    }
  },
  methods: {
    logoClick: function() {
      this.$router.push("/boards");
    },
    logout: function() {
      this.$axios.get(UrlConfig.auth.logout).then(response => {
        this.$router.push("/");
      });
    },
    setKeyword: _.debounce(function(e) {}, 300),
    goBoard: function(hashId){
        this.$router.push("/board/" + hashId)
    },
    addNewBoard: function(){
        var maxSeq = -1;
        if (this.boards.length > 0) {
            maxSeq = _.maxBy(this.boards, 'boardSeq').boardSeq
        }

        var board = {
            boardSeq: maxSeq + 1,
            boardTitle: "Board Title"
        }
        this.$axios.post(UrlConfig.board.saveBoard, board)
        .then((response) => {
            board.hashId = response.data
            this.boards.push(board)
        })
    }
  },
  beforeCreate: function() {
    this.$axios.get(UrlConfig.board.getBoardList).then(response => {
      this.boards = response.data;
    });
    this.$axios.get(UrlConfig.test.whoami)
    .then(response => {
      this.userEmail = response.data[0];
    })
    .catch(()=>{
        this.$router.push("/")
    })
  },
};
</script>

<style>
.clickable {
  cursor: pointer;
}
.clickable:hover {
  opacity: 0.6;
}
.mw-2 {
  max-width: 900px !important;
}
</style>