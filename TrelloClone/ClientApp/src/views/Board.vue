<template>
  <v-app>
    <board-header></board-header>
    <v-content v-if="isLoading" app style="background-color:rgb(81, 152, 57); overflow: auto;" class="px-2">
    </v-content>
    <v-content v-else-if="isValidBoard" app style="background-color:rgb(81, 152, 57); overflow: auto;" class="px-2">
      <v-layout class="py-3" align-center>
        <v-text-field
          v-if="titleEditing && canEditing"
          dark
          ref="boardTitle"
          class="title font-weight-bold"
          autofocus
          hide-details
          box
          :value="boardTitle"
          @blur="saveTitle"
          @keyup.enter="saveTitle"
        ></v-text-field>
        <div
          v-else
          class="title white--text font-weight-bold pl-2 pt-4 clickable"
          @click="changeTitleEditing"
        >{{boardTitle}}</div>
        
        <v-switch v-if="canEditing" :label="isPublic?'Public':'Private'" v-model="isPublic" hide-details color="pink" class="pl-5" dark></v-switch>
      </v-layout>
      <v-layout class="pb-3">
        <Container orientation="horizontal" @drop="onColumnDrop($event)" @drag-start="dragStart">
          <Draggable v-for="cardList in cardLists" :key="cardList.listSeq">
            <card-list :cardList="cardList"></card-list>
          </Draggable>
        </Container>
        <div v-if="canEditing">
          <v-card
            dark
            color="#3f6f21"
            width="20em"
            flat
            class="px-4 py-2 ml-1 clickable"
            @click="addCardList({listTitle:'List Title'})"
          >
            <span>+ Another another list</span>
          </v-card>
        </div>
      </v-layout>
    </v-content>
    <v-content v-else>
      <v-container>
        <v-layout>
          <v-flex class="text-xs-center py-5">
            <span class="title">Board Not Found</span>
          </v-flex>
        </v-layout>
      </v-container>
    </v-content>
  </v-app>
</template>

<script>
import BoardHeader from "../components/board/BoardHeader";
import CardList from "../components/board/CardList";
import StoreConfig from "../store/StoreConfig";
import { mapActions } from "vuex";
import UrlConfig from "../api/UrlConfig";
import _ from "lodash";
import { Container, Draggable } from "vue-smooth-dnd";
import { applyDrag } from "../utils/helpers";


export default {
  name: "board",
  props:["hashId"],
  components: {
    BoardHeader,
    CardList,
    Container,
    Draggable
  },
  data: function() {
    return {
      isLoading: false,
      isValidBoard: true,
      titleEditing: false
    };
  },
  computed: {
    boardTitle() {
      return this.$store.state.board.boardTitle;
    },
    isPublic: {
      get: function () {
        return this.$store.state.board.isPublic
      },
      set: function (newValue) {
        this.$store.dispatch(StoreConfig.setPublic, newValue)
      }
    },
    canEditing: function(){
      return this.$store.state.board.canEditing
    },
    cardLists: function() {
      var cardList = [];
      var keyword = this.$store.state.keyword;
      if (keyword) {
        this.$store.state.board.cardLists.forEach(x => {
          if (x.listTitle.toUpperCase().includes(keyword.toUpperCase())) {
            cardList.push({
              seq: x.seq,
              listTitle: x.listTitle,
              prevSeq: x.prevSeq,
              order: x.order,
              cards: x.cards.sort((a, b) => a.order - b.order)
            });
          } else {
            var cards = [];
            x.cards &&
              x.cards.forEach(c => {
                if (
                  c.title.toUpperCase().includes(keyword.toUpperCase()) ||
                  c.description.toUpperCase().includes(keyword.toUpperCase())
                ) {
                  cards.push(c);
                }
              });

            if (cards.length > 0) {
              cardList.push({
                seq: x.seq,
                listTitle: x.listTitle,
                cards: cards.sort((a, b) => a.order - b.order)
              });
            }
          }
        });
        return cardList.sort((a, b) => a.order - b.order);
      } else {
        return this.$store.state.board.cardLists;
      }
    }
  },
  methods: {
    changeTitleEditing: function() {
      this.titleEditing = !this.titleEditing;
    },
    saveTitle: function(e) {
      this.titleEditing = false;

      if (e.target.value) {
        this.$store.dispatch(StoreConfig.saveBoardTitle, e.target.value)
      }
    },
    onColumnDrop(dropResult) {
      if(dropResult.removedIndex != dropResult.addedIndex){
        this.$store.dispatch(StoreConfig.changeListSort, dropResult)
      }
    },
    dragStart() {
      console.log("drag started");
    },
    ...mapActions([StoreConfig.addCardList])
  },
  beforeMount: function() {
    this.isLoading = true

    this.$axios
      .get(UrlConfig.board.getBoard + "/" + this.hashId)
      .then(response => {
        this.$store.dispatch(StoreConfig.getBoard, response.data);
        this.isLoading = false
      })
      .catch(ex => {
        this.isLoading = false
        this.isValidBoard = false
      });
  }
};
</script>

<style>
.clickable {
  cursor: pointer;
}
.clickable:hover {
  opacity: 0.6;
}
</style>