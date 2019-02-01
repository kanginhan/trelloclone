import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'
import StoreConfig from './StoreConfig'
import UrlConfig from '../api/UrlConfig'
import _ from "lodash";

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        keyword: "",
        board: {
            boardSeq: 0,
            boardTitle: "Local Test",
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
        }
    },
    mutations: {
        [StoreConfig.addCardList]: (state, payload) => {
            if (payload && payload.listTitle) {
                state.board.cardLists.push({
                    listSeq: payload.listSeq,
                    listTitle: payload.listTitle,
                    cards: []
                })
            }
        },
        [StoreConfig.saveBoardTitle]: (state, payload) => {
            if (payload) {
                state.board.boardTitle = payload;
            }
        },
        [StoreConfig.saveListTitle]: (state, payload) => {
            if (payload.cardList && payload.listTitle) {
                var cardList = state.board.cardLists.find(x => x.listSeq == payload.cardList.listSeq)
                if (cardList) {
                    cardList.listTitle = payload.listTitle
                }
            }
        },
        [StoreConfig.saveCard]: (state, payload) => {
            if (payload && payload.cardTitle && _.isNumber(payload.listSeq)) {
                payload.cardList.cards.push({
                    cardSeq: payload.cardSeq,
                    title: payload.cardTitle,
                    description: ""
                })
            }
        },
        [StoreConfig.deleteCard]: (state, payload) => {
            if (payload && _.isNumber(payload.listSeq) && _.isNumber(payload.cardSeq)) {
                var cardList = state.board.cardLists.find(x => x.listSeq == payload.listSeq)
                if (cardList) {
                    var cardIdx = cardList.cards.findIndex(x => x.cardSeq == payload.cardSeq)
                    _.isNumber(cardIdx) && cardIdx > -1 && cardList.cards.splice(cardIdx, 1)
                }
            }
        },
        [StoreConfig.deleteList]: (state, payload) => {
            var listIdx = state.board.cardLists.findIndex(x => x.listSeq == payload.listSeq)
            if (_.isNumber(listIdx) && listIdx > -1) {
                state.board.cardLists.splice(listIdx, 1)
            }
        },
        [StoreConfig.saveCardTitle]: (state, payload) => {
            if (payload && _.isNumber(payload.listSeq) && _.isNumber(payload.cardSeq) && payload.cardTitle) {
                var cardList = state.board.cardLists.find(x => x.listSeq == payload.listSeq)
                if (cardList) {
                    var card = cardList.cards.find(x => x.cardSeq == payload.cardSeq)
                    if (card) {
                        card.title = payload.cardTitle
                    }
                }
            }
        },
        [StoreConfig.saveCardDesc]: (state, payload) => {
            if (payload && _.isNumber(payload.listSeq) && _.isNumber(payload.cardSeq) && payload.description) {
                var cardList = state.board.cardLists.find(x => x.listSeq == payload.listSeq)
                if (cardList) {
                    var card = cardList.cards.find(x => x.cardSeq == payload.cardSeq)
                    if (card) {
                        card.description = payload.description
                    }
                }
            }
        },
        [StoreConfig.getBoard]: (state, payload) => {
            if (payload) {
                state.board = payload
            }
        },
        [StoreConfig.setKeyword]: (state, payload) => {
            state.keyword = payload
        }
    },
    actions: {
        [StoreConfig.addCardList]: (store, payload) => {
            var maxSeq = -1;
            if (store.state.board.cardLists && store.state.board.cardLists.length > 0) {
                maxSeq = _.maxBy(store.state.board.cardLists, 'listSeq').listSeq
            }
            payload.listSeq = maxSeq + 1
            payload.boardSeq = store.state.board.boardSeq

            axios.post(UrlConfig.board.saveList, payload)
                .then(() => {
                    store.commit(StoreConfig.addCardList, payload)
                })
        },
        [StoreConfig.saveBoardTitle]: (store, payload) => {
            var param = {
                boardSeq: store.state.board.boardSeq,
                boardTitle: payload
            }
            axios.post(UrlConfig.board.saveBoardTitle, param)
                .then(() => {
                    store.commit(StoreConfig.saveBoardTitle, payload)
                })
        },
        [StoreConfig.saveListTitle]: (store, payload) => {
            var param = {
                boardSeq: store.state.board.boardSeq,
                listSeq: payload.cardList.listSeq,
                listTitle: payload.listTitle
            }
            axios.post(UrlConfig.board.saveList, param)
                .then(() => {
                    store.commit(StoreConfig.saveListTitle, payload)
                })
        },
        [StoreConfig.saveCard]: (store, payload) => {
            var cardList = store.state.board.cardLists.find(x => x.listSeq == payload.listSeq)
            if (!cardList) return

            var maxSeq = -1;
            if (cardList.cards.length > 0) {
                maxSeq = _.maxBy(cardList.cards, 'cardSeq').cardSeq
            }
            payload.cardSeq = maxSeq + 1
            payload.description = ""
            payload.cardList = cardList

            var param = {
                boardSeq: store.state.board.boardSeq,
                listSeq: payload.listSeq,
                cardSeq: payload.cardSeq,
                title: payload.cardTitle,
                description: payload.description
            }

            axios.post(UrlConfig.board.saveCard, param)
                .then(() => {
                    store.commit(StoreConfig.saveCard, payload)
                })
        },
        [StoreConfig.deleteCard]: (store, payload) => {
            var param = {
                boardSeq: store.state.board.boardSeq,
                listSeq: payload.listSeq,
                cardSeq: payload.cardSeq
            }
            axios.post(UrlConfig.board.deleteCard, param)
                .then(() => {
                    store.commit(StoreConfig.deleteCard, payload)
                })
        },
        [StoreConfig.deleteList]: (store, payload) => {
            var param = {
                boardSeq: store.state.board.boardSeq,
                listSeq: payload.listSeq
            }
            axios.post(UrlConfig.board.deleteList, param)
                .then(() => {
                    store.commit(StoreConfig.deleteList, payload)
                })
        },
        [StoreConfig.saveCardTitle]: (store, payload) => {
            var param = {
                boardSeq: store.state.board.boardSeq,
                listSeq: payload.listSeq,
                cardSeq: payload.cardSeq,
                title: payload.cardTitle,
                description: payload.description
            }

            axios.post(UrlConfig.board.saveCard, param)
                .then(() => {
                    store.commit(StoreConfig.saveCardTitle, payload)
                })
        },
        [StoreConfig.saveCardDesc]: (store, payload) => {
            var param = {
                boardSeq: store.state.board.boardSeq,
                listSeq: payload.listSeq,
                cardSeq: payload.cardSeq,
                title: payload.cardTitle,
                description: payload.description
            }

            axios.post(UrlConfig.board.saveCard, param)
                .then(() => {
                    store.commit(StoreConfig.saveCardDesc, payload)
                })
        },
        [StoreConfig.getBoard]: (store, payload) => {
            store.commit(StoreConfig.getBoard, payload)
        },
        [StoreConfig.setKeyword]: (store, payload) => {
            store.commit(StoreConfig.setKeyword, payload)
        }
    }
})