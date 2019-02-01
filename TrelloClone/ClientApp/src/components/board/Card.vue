<template>
  <v-dialog v-model="dialog" width="60em">
    <v-card slot="activator" class="my-1 clickable" width="18.8em">
      <div class="py-2 px-2">{{card.title}}</div>
      <div class="px-1">
        <v-icon v-if="card.description">list</v-icon>
      </div>
    </v-card>

    <v-toolbar dark color="blue darken-2" height="80em">
      <v-icon dark>web</v-icon>
      <v-layout column class="pl-3">
        <v-text-field v-if="isTitleEditing" :value="card.title" hide-details box autofocus color="white" @blur="saveCardTitle" @keyup.enter="saveCardTitle"></v-text-field>
        <v-toolbar-title v-else @click="changeTitleEditing" class="clickable">{{card.title}}</v-toolbar-title>
        <div class="caption">
          in list
          <span style="text-decoration:underline;">{{cardList.listTitle}}</span>
        </div>
      </v-layout>
      <v-spacer></v-spacer>
      <v-toolbar-items>
        <v-btn icon dark @click="deleteCard">
          <v-icon>delete_outline</v-icon>
        </v-btn>
        <v-btn icon dark @click="dialog = false">
          <v-icon>close</v-icon>
        </v-btn>
      </v-toolbar-items>
    </v-toolbar>

    <v-card color="grey lighten-2" class="pb-3">
      <v-card-title class="title">
        <v-icon>list</v-icon>
        <span class="pl-2">Description</span>
      </v-card-title>

      <template v-if="isDescEditing">
        <v-textarea box autofocus auto-grow hide-details :value="descForTextarea" class="px-5" ref="textarea"></v-textarea>
          <v-btn color="success" dark class="ml-5" @click="saveCardDesc">Save</v-btn>
          <v-btn icon @click="changeDescEditing">
            <v-icon>close</v-icon>
          </v-btn>
      </template>
      <p v-else 
        :class="{'mx-5':true, 'pt-2':true, 'px-2':true, 'clickable':true, 'pb-5':!card.description, 'descEmpty':!card.description, 'text-xs-center':!card.description, 'pt-4':!card.description}" 
        @click="changeDescEditing"
      ><span v-html="card.description || 'Add a more detailed description...'"></span></p>

    </v-card>
  </v-dialog>
</template>

<script>
import StoreConfig from "../../store/StoreConfig";

export default {
  name: "card",
  props: ["card", "cardList"],
  data: function() {
    return {
      dialog: false,
      isTitleEditing: false,
      isDescEditing: false
    };
  },
  computed: {
    descForTextarea: function(){
      return (this.card.description || "").replace(/<br\/>/gi, "\r\n")
    }
  },
  methods: {
    changeTitleEditing: function(){
      this.isTitleEditing = !this.isTitleEditing
    },
    changeDescEditing: function(){
      this.isDescEditing = !this.isDescEditing
    },
    saveCardTitle: function(e){
      this.isTitleEditing = false;
      
      if(e.target.value){
        var payload = {
          listSeq: this.cardList.listSeq,
          cardSeq: this.card.cardSeq,
          cardTitle: e.target.value,
          description: this.card.description
        }
        this.$store.dispatch(StoreConfig.saveCardTitle, payload)
      }
    },
    saveCardDesc: function(e){
      this.isDescEditing = false;
      
      if(this.$refs.textarea.lazyValue){
        var payload = {
          listSeq: this.cardList.listSeq,
          cardSeq: this.card.cardSeq,
          cardTitle: this.card.title,
          description: this.$refs.textarea.lazyValue.replace(/(?:\r\n|\r|\n)/g, '<br/>')
        }
        this.$store.dispatch(StoreConfig.saveCardDesc, payload)
      }
    },
    deleteCard: function(){
      if(confirm("Are you sure you want to delete?")){
        this.dialog = false;
        var payload = {
          listSeq: this.cardList.listSeq,
          cardSeq: this.card.cardSeq
        }
        this.$store.dispatch(StoreConfig.deleteCard, payload)
      }
    }
  },
};
</script>

<style scoped>
.clickable {
  cursor: pointer;
}
.clickable:hover {
  opacity: 0.6;
}
.descEmpty{
  background-color: #CCCCCC
}
</style>