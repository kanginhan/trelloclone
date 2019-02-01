<template>
  <v-app>
    <board-header></board-header>
    <v-content app style="background-color:rgb(81, 152, 57); overflow: auto;" class="px-2">
      <v-layout class="py-3" align-center>
        <v-text-field
          v-if="titleEditing"
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
          class="title white--text font-weight-bold pl-2 clickable"
          @click="changeTitleEditing"
        >{{boardTitle}}</div>
        <div class="body-2 font-weight-light white--text pl-2 pr-3">| Personal | Private</div>
      </v-layout>
      <v-layout class="pb-3">
<Container
      orientation="horizontal"
      @drop="onColumnDrop($event)"
      @drag-start="dragStart"
    >
    
    <Draggable v-for="cardList in cardLists" :key="cardList.listSeq">
      <card-list :cardList="cardList"></card-list>
      
    </Draggable>
    
    
</Container>
<div>
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
    
      
  </v-app>
</template>

<script>
import BoardHeader from "../components/board/BoardHeader";
import CardList from "../components/board/CardList";
import StoreConfig from "../store/StoreConfig";
import { mapActions } from "vuex";
import UrlConfig from "../api/UrlConfig";
import _ from "lodash";

import { Container, Draggable } from 'vue-smooth-dnd'
import { applyDrag, generateItems } from '../utils/helpers'
const lorem = `Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. 
Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. 
Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.`
const columnNames = ['Lorem', 'Ipsum', 'Consectetur', 'Eiusmod']
const cardColors = [
  'azure',
  'beige',
  'bisque',
  'blanchedalmond',
  'burlywood',
  'cornsilk',
  'gainsboro',
  'ghostwhite',
  'ivory',
  'khaki'
]
const pickColor = () => {
  const rand = Math.floor(Math.random() * 10)
  return cardColors[rand]
}
const scene = {
  type: 'container',
  props: {
    orientation: 'horizontal'
  },
  children: generateItems(4, i => ({
    id: `column${i}`,
    type: 'container',
    name: columnNames[i],
    props: {
      orientation: 'vertical',
      className: 'card-container'
    },
    children: generateItems(+(Math.random() * 10).toFixed() + 5, j => ({
      type: 'draggable',
      id: `${i}${j}`,
      props: {
        className: 'card',
        style: {backgroundColor: pickColor()}
      },
      data: lorem.slice(0, Math.floor(Math.random() * 150) + 30)
    }))
  }))
}

export default {
  name: "board",
  components: {
    BoardHeader,
    CardList,
    Container, Draggable
  },
  data: function() {
    return {
      titleEditing: false,
      scene,
      cardLists: [{
                    listSeq: 0,
                    listTitle: "공지사항",
                    cards: [
                        { cardSeq: 0, title: "모임", description: "0000" },
                        { cardSeq: 1, title: "모임2", description: "0000" }
                    ]
                },
                {
                    listSeq: 1,
                    listTitle: "해야할일",
                    cards: [
                        { cardSeq: 0, title: "운동하기", description: "0000" },
                        { cardSeq: 1, title: "노래하기", description: "0000" }
                    ]
                }
            ]
    };
  },
  computed: {
    boardTitle() {
      return this.$store.state.board.boardTitle;
    },
    // cardLists: function() {
    //   debugger;
    //   var cardList = [];
    //   var keyword = this.$store.state.keyword;
    //   this.$store.state.board.cardLists.forEach(x => {
    //     if (x.listTitle.toUpperCase().includes(keyword.toUpperCase())) {
    //       cardList.push(x);
    //     } else {
    //       var cards = [];
    //       x.cards &&
    //         x.cards.forEach(c => {
    //           if (
    //             c.title.toUpperCase().includes(keyword.toUpperCase()) ||
    //             c.description.toUpperCase().includes(keyword.toUpperCase())
    //           ) {
    //             cards.push(c);
    //           }
    //         });

    //       if (cards.length > 0) {
    //         cardList.push({
    //           listSeq: x.listSeq,
    //           listTitle: x.listTitle,
    //           cards: cards
    //         });
    //       }
    //     }
    //   });
    //   return cardList;
    // }
  },
  methods: {
    changeTitleEditing: function() {
      this.titleEditing = !this.titleEditing;
    },
    saveTitle: function(e) {
      this.titleEditing = false;

      if (e.target.value) {
        this.$store.dispatch(StoreConfig.saveBoardTitle, e.target.value);
      }
    },
        onColumnDrop (dropResult) {
      const scene = Object.assign({}, this.scene)
      scene.children = applyDrag(scene.children, dropResult)
      this.scene = scene
    },
    onCardDrop (columnId, dropResult) {
      if (dropResult.removedIndex !== null || dropResult.addedIndex !== null) {
        const scene = Object.assign({}, this.scene)
        const column = scene.children.filter(p => p.id === columnId)[0]
        const columnIndex = scene.children.indexOf(column)
        const newColumn = Object.assign({}, column)
        newColumn.children = applyDrag(newColumn.children, dropResult)
        scene.children.splice(columnIndex, 1, newColumn)
        this.scene = scene
      }
    },
    getCardPayload (columnId) {
      return index => {
        return this.scene.children.filter(p => p.id === columnId)[0].children[index]
      }
    },
    dragStart () {
      console.log('drag started')
    },
    log (...params) {
      console.log(...params)
    },
    ...mapActions([StoreConfig.addCardList])
  },
  beforeCreate: function() {
    this.$axios
      .get(UrlConfig.board.getBoard + "/0")
      .then(response => {
        this.$store.dispatch(StoreConfig.getBoard, response.data);
      })
      .catch(ex => {
        alert(ex);
        // this.$router.push("/")
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