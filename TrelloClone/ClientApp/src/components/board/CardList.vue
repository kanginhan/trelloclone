<template>
  <div>
    <v-card color="grey lighten-2" width="20em" class="pb-2 px-2 mx-1">
      <v-layout>
        <v-text-field
          v-if="isTitleEditing && canEditing"
          autofocus
          box
          :value="cardList.listTitle"
          @blur="saveTitle"
          @keyup.enter="saveTitle"
        ></v-text-field>
        <v-card-title
          v-else
          class="font-weight-bold clickable"
          @click="changeTitleEditing"
        >{{cardList.listTitle}}</v-card-title>

        <v-menu v-if="canEditing" :nudge-width="200" offset-x style="position:absolute; right:0;">
          <v-btn icon slot="activator">
            <v-icon small>more_horiz</v-icon>
          </v-btn>
          <v-list>
            <v-list-tile @click="deleteList">
              <v-list-tile-title>Delete This List</v-list-tile-title>
            </v-list-tile>
          </v-list>
        </v-menu>
      </v-layout>
      <Container
        group-name="col"
        @drop="onCardDrop"
        @drag-start="(e) => log('drag start', e)"
        @drag-end="(e) => log('drag end', e)"
        :get-child-payload="getCardPayload()"
        drag-class="card-ghost"
        drop-class="card-ghost-drop"
      >
        <Draggable v-for="card in cardList.cards" :key="card.cardSeq">
          <card :card="card" :cardList="cardList"></card>
        </Draggable>
      </Container>
      
      <div v-if="canEditing">
        <div v-if="isCardEditing" class="pt-2">
          <v-text-field solo hide-details ref="cardTitle" autofocus @keyup.enter="saveCard"></v-text-field>
          <v-btn color="success" dark @click="saveCard">Add Card</v-btn>
          <v-btn icon @click="changeCardEditing">
            <v-icon>close</v-icon>
          </v-btn>
        </div>
        <v-card v-else flat tile color="grey lighten-2" class="clickable" @click="changeCardEditing">
          <div class="px-2 pt-2">+ Add another card</div>
        </v-card>
      </div>
    </v-card>
  </div>
</template>

<script>
import Card from "./Card.vue";
import StoreConfig from "../../store/StoreConfig";
import { Container, Draggable } from "vue-smooth-dnd";
import { applyDrag } from "../../utils/helpers";

export default {
  name: "cardlist",
  props: ["cardList"],
  components: {
    Card,
    Container,
    Draggable
  },
  data: function() {
    return {
      dialog: false,
      isTitleEditing: false,
      isCardEditing: false
    };
  },
  computed: {
    canEditing: function(){
      return this.$store.state.board.canEditing
    }
  },
  methods: {
    changeTitleEditing: function() {
      this.isTitleEditing = !this.isTitleEditing;
    },
    changeCardEditing: function() {
      this.isCardEditing = !this.isCardEditing;
    },
    saveTitle: function(e) {
      this.isTitleEditing = false;

      if (e.target.value) {
        var payload = {
          listTitle: e.target.value,
          cardList: this.cardList
        };
        this.$store.dispatch(StoreConfig.saveListTitle, payload);
      }
    },
    saveCard: function(e) {
      if (this.$refs.cardTitle.lazyValue) {
        this.isCardEditing = false;

        var payload = {
          cardTitle: this.$refs.cardTitle.lazyValue,
          listSeq: this.cardList.seq
        };
        this.$store.dispatch(StoreConfig.saveCard, payload);
      }
    },
    deleteList: function() {
      if (confirm("Are you sure you want to delete?")) {
        var payload = {
          seq: this.cardList.seq
        };
        this.$store.dispatch(StoreConfig.deleteList, payload);
      }
    },
    onCardDrop(dropResult) {
      if (dropResult.removedIndex !== null || dropResult.addedIndex !== null) {
        dropResult.listSeq = this.cardList.seq;
        this.$store.dispatch(StoreConfig.changeCardSort, dropResult);
      }
    },
    getCardPayload() {
      return index => this.cardList.cards[index];
    },
    log(...params) {
      console.log(...params);
    }
  }
};
</script>

<style scoped>
.clickable {
  cursor: pointer !important;
}
.clickable:hover {
  opacity: 0.6 !important;
}
.card-ghost {
  transition: transform 0.18s ease;
  transform: rotateZ(5deg);
}
.card-ghost-drop {
  transition: transform 0.18s ease-in-out;
  transform: rotateZ(0deg);
}
</style>