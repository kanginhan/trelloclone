import Vue from 'vue'
import Vuex from 'vuex'
import axios from 'axios'
import StoreConfig from './StoreConfig'
import UrlConfig from '../api/UrlConfig'
import _ from "lodash";
import { applyDrag } from "../utils/helpers";

Vue.use(Vuex)

export default new Vuex.Store({
    state: {
        keyword: "",
        cardSort: {
            removed: null,
            added: null
        },
        board: {
            // boardSeq: 0,
            // boardTitle: "Local Test",
            // cardLists: [{
            //         listSeq: 0,
            //         listTitle: "공지사항",
            //         sort: 0,
            //         cards: [
            //             { cardSeq: 0, title: "모임", description: "0000", sort: 0 },
            //             { cardSeq: 1, title: "모임2", description: "0000", sort: 1 }
            //         ]
            //     },
            //     {
            //         listSeq: 1,
            //         listTitle: "해야할일",
            //         sort: 1,
            //         cards: [
            //             { cardSeq: 0, title: "운동하기", description: "0000", sort: 0 },
            //             { cardSeq: 1, title: "노래하기", description: "0000", sort: 1 }
            //         ]
            //     },
            //     {
            //         listSeq: 2,
            //         listTitle: "해야할일2",
            //         sort: 2,
            //         cards: [
            //             { cardSeq: 0, title: "운동하기2", description: "0000", sort: 0 },
            //             { cardSeq: 1, title: "노래하기2", description: "0000", sort: 1 }
            //         ]
            //     }
            // ]
        }
    },
    mutations: {
        [StoreConfig.addCardList]: (state, payload) => {
            if (payload && payload.listTitle) {
                state.board.cardLists.push({
                    seq: payload.seq,
                    listTitle: payload.listTitle,
                    order: payload.order,
                    prevSeq: payload.prevSeq,
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
                var cardList = state.board.cardLists.find(x => x.seq == payload.cardList.seq)
                if (cardList) {
                    cardList.listTitle = payload.listTitle
                }
            }
        },
        [StoreConfig.saveCard]: (state, payload) => {
            if (payload && payload.cardTitle && _.isNumber(payload.listSeq)) {
                payload.cardList.cards.push({
                    seq: payload.seq,
                    title: payload.cardTitle,
                    description: "",
                    order: payload.order,
                    prevSeq: payload.prevSeq
                })
            }
        },
        [StoreConfig.deleteCard]: (state, payload) => {
            var updates = []

            if (payload && _.isNumber(payload.listSeq) && _.isNumber(payload.seq)) {
                var cardList = state.board.cardLists.find(x => x.seq == payload.listSeq)
                var findCard = cardList && cardList.cards.find(x => x.seq == payload.seq)

                if (findCard) {
                    var cardIdx = cardList.cards.indexOf(findCard)
                    if (cardList.cards[cardIdx + 1]) {
                        cardList.cards[cardIdx + 1].prevSeq = findCard.prevSeq
                        updates.push({
                            boardSeq: state.board.boardSeq,
                            listSeq: payload.listSeq,
                            seq: cardList.cards[cardIdx + 1].seq,
                            prevSeq: cardList.cards[cardIdx + 1].prevSeq
                        })
                        payload.updates = updates
                    }
                    cardList.cards.splice(cardIdx, 1)
                }
            }
        },
        [StoreConfig.deleteList]: (state, payload) => {
            var updates = []
            var findList = state.board.cardLists.find(x => x.seq == payload.seq)

            if (findList) {
                var listIdx = state.board.cardLists.indexOf(findList)
                if (state.board.cardLists[listIdx + 1]) {
                    state.board.cardLists[listIdx + 1].prevSeq = findList.prevSeq
                    updates.push({
                        boardSeq: state.board.boardSeq,
                        seq: state.board.cardLists[listIdx + 1].seq,
                        prevSeq: state.board.cardLists[listIdx + 1].prevSeq
                    })
                    payload.updates = updates
                }

                state.board.cardLists.splice(listIdx, 1)
            }
        },
        [StoreConfig.saveCardTitle]: (state, payload) => {
            if (payload && _.isNumber(payload.listSeq) && _.isNumber(payload.seq) && payload.cardTitle) {
                var cardList = state.board.cardLists.find(x => x.seq == payload.listSeq)
                if (cardList) {
                    var card = cardList.cards.find(x => x.seq == payload.seq)
                    if (card) {
                        card.title = payload.cardTitle
                    }
                }
            }
        },
        [StoreConfig.saveCardDesc]: (state, payload) => {
            if (payload && _.isNumber(payload.listSeq) && _.isNumber(payload.seq) && payload.description) {
                var cardList = state.board.cardLists.find(x => x.seq == payload.listSeq)
                if (cardList) {
                    var card = cardList.cards.find(x => x.seq == payload.seq)
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
        },
        [StoreConfig.changeListSort]: (state, payload) => {
            var board = Object.assign({}, state.board)
            var updates = []

            // 1. 삭제되는 위치에서 이전 목록을 앞으로 연결
            if (board.cardLists[payload.removedIndex + 1]) {
                board.cardLists[payload.removedIndex + 1].prevSeq = board.cardLists[payload.removedIndex - 1] ? board.cardLists[payload.removedIndex - 1].seq : null
                updates.push({
                    boardSeq: state.board.boardSeq,
                    seq: board.cardLists[payload.removedIndex + 1].seq,
                    prevSeq: board.cardLists[payload.removedIndex + 1].prevSeq
                })
            }

            // 2. 삽입되는 위치에서 삽입목록을 앞으로 연결
            board.cardLists[payload.removedIndex].prevSeq = board.cardLists[payload.addedIndex - 1] ? board.cardLists[payload.addedIndex - 1].seq : null
            updates.push({
                boardSeq: state.board.boardSeq,
                seq: board.cardLists[payload.removedIndex].seq,
                prevSeq: board.cardLists[payload.removedIndex].prevSeq
            })

            // 3. 삽입되는 위치에서 이전목록을 삽입목록에 연결
            if (board.cardLists[payload.addedIndex]) {
                board.cardLists[payload.addedIndex].prevSeq = board.cardLists[payload.removedIndex].seq
                updates.push({
                    boardSeq: state.board.boardSeq,
                    seq: board.cardLists[payload.addedIndex].seq,
                    prevSeq: board.cardLists[payload.addedIndex].prevSeq
                })
            }

            board.cardLists = applyDrag(board.cardLists, payload)
            board.cardLists.forEach((x, i) => x.order = i)
            state.board = board
            payload.updates = updates
        },
        [StoreConfig.changeCardSort]: (state, payload) => {
            var updates = []

            // 1. 삭제되는 위치에서 이전 목록을 앞으로 연결
            var removeList = state.board.cardLists.find(x => x.seq == payload.removed.listSeq)
            if (removeList.cards[payload.removed.index + 1]) {
                removeList.cards[payload.removed.index + 1].prevSeq = removeList.cards[payload.removed.index - 1] ? removeList.cards[payload.removed.index - 1].seq : null
                updates.push({
                    boardSeq: state.board.boardSeq,
                    listSeq: removeList.seq,
                    seq: removeList.cards[payload.removed.index + 1].seq,
                    prevSeq: removeList.cards[payload.removed.index + 1].prevSeq
                })
            }

            // 2. 삽입되는 위치에서 삽입목록을 앞으로 연결
            var removedItem = removeList.cards[payload.removed.index]
            var oldSeq = removedItem.seq
            var addList = state.board.cardLists.find(x => x.seq == payload.added.listSeq)
            removedItem.prevSeq = addList.cards[payload.added.index - 1] ? addList.cards[payload.added.index - 1].seq : null
            if (removeList.seq != addList.seq) {
                var maxSeq = -1;
                if (addList.cards.length > 0) {
                    maxSeq = _.maxBy(addList.cards, 'seq').seq
                }
                removedItem.seq = maxSeq + 1
            }

            updates.push({
                boardSeq: state.board.boardSeq,
                fromListSeq: removeList.seq,
                fromSeq: oldSeq,
                listSeq: addList.seq,
                seq: removedItem.seq,
                prevSeq: removedItem.prevSeq
            })

            // 3. 삽입되는 위치에서 이전목록을 삽입목록에 연결
            if (addList.cards[payload.added.index]) {
                addList.cards[payload.added.index].prevSeq = removedItem.seq
                updates.push({
                    boardSeq: state.board.boardSeq,
                    listSeq: addList.seq,
                    seq: addList.cards[payload.added.index].seq,
                    prevSeq: addList.cards[payload.added.index].prevSeq
                })
            }


            removeList.cards = applyDrag(removeList.cards, { removedIndex: payload.removed.index, addedIndex: null })
            removeList.cards.forEach((x, i) => x.order = i)

            addList.cards = applyDrag(addList.cards, { removedIndex: null, addedIndex: payload.added.index, payload: removedItem })
            addList.cards.forEach((x, i) => x.order = i)

            payload.updates = updates
        },
        [StoreConfig.deleteBoard]: (state) => {
            state.board = {}
        },
        [StoreConfig.setPublic]: (state, payload) => {
            state.board.isPublic = payload
        }
    },
    actions: {
        [StoreConfig.addCardList]: (store, payload) => {
            var maxSeq = -1
            var maxOrder = -1
            var prevSeq = null
            if (store.state.board.cardLists && store.state.board.cardLists.length > 0) {
                maxSeq = _.maxBy(store.state.board.cardLists, 'seq').seq
                var maxOrderItem = _.maxBy(store.state.board.cardLists, 'order')
                maxOrder = maxOrderItem.order
                prevSeq = maxOrderItem.seq
            }
            payload.seq = maxSeq + 1
            payload.order = maxOrder + 1
            payload.prevSeq = prevSeq
            payload.boardSeq = store.state.board.boardSeq

            store.commit(StoreConfig.addCardList, payload)

            axios.post(UrlConfig.board.saveList, payload)
        },
        [StoreConfig.saveBoardTitle]: (store, payload) => {
            store.commit(StoreConfig.saveBoardTitle, payload)
            var param = {
                boardSeq: store.state.board.boardSeq,
                boardTitle: payload
            }
            axios.post(UrlConfig.board.saveBoardTitle, param)
        },
        [StoreConfig.saveListTitle]: (store, payload) => {
            store.commit(StoreConfig.saveListTitle, payload)

            var param = {
                boardSeq: store.state.board.boardSeq,
                seq: payload.cardList.seq,
                listTitle: payload.listTitle
            }
            axios.post(UrlConfig.board.saveListTitle, param)
        },
        [StoreConfig.saveCard]: (store, payload) => {
            var cardList = store.state.board.cardLists.find(x => x.seq == payload.listSeq)
            if (!cardList) return

            var maxSeq = -1
            var maxOrder = -1
            var prevSeq = null
            if (cardList.cards && cardList.cards.length > 0) {
                maxSeq = _.maxBy(cardList.cards, 'seq').seq
                var maxOrderItem = _.maxBy(cardList.cards, 'order')
                maxOrder = maxOrderItem.order
                prevSeq = maxOrderItem.seq
            }

            payload.seq = maxSeq + 1
            payload.order = maxOrder + 1
            payload.prevSeq = prevSeq
            payload.description = ""
            payload.cardList = cardList

            var param = {
                boardSeq: store.state.board.boardSeq,
                listSeq: payload.listSeq,
                seq: payload.seq,
                title: payload.cardTitle,
                description: payload.description,
                prevSeq: payload.prevSeq
            }

            store.commit(StoreConfig.saveCard, payload)

            axios.post(UrlConfig.board.saveCard, param)
        },
        [StoreConfig.deleteCard]: (store, payload) => {
            store.commit(StoreConfig.deleteCard, payload)

            var param = {
                boardSeq: store.state.board.boardSeq,
                listSeq: payload.listSeq,
                seq: payload.seq
            }
            axios.post(UrlConfig.board.deleteCard, param)

            if (payload.updates && payload.updates.length > 0) {
                axios.post(UrlConfig.board.updateCardPrevSeq, payload.updates)
            }
        },
        [StoreConfig.deleteList]: (store, payload) => {
            store.commit(StoreConfig.deleteList, payload)

            var param = {
                boardSeq: store.state.board.boardSeq,
                seq: payload.seq
            }
            axios.post(UrlConfig.board.deleteList, param)

            if (payload.updates && payload.updates.length > 0) {
                axios.post(UrlConfig.board.updateListPrevSeq, payload.updates)
            }
        },
        [StoreConfig.saveCardTitle]: (store, payload) => {
            store.commit(StoreConfig.saveCardTitle, payload)

            var param = {
                boardSeq: store.state.board.boardSeq,
                listSeq: payload.listSeq,
                seq: payload.seq,
                title: payload.cardTitle,
                description: payload.description
            }
            axios.post(UrlConfig.board.saveCardContent, param)
        },
        [StoreConfig.saveCardDesc]: (store, payload) => {
            store.commit(StoreConfig.saveCardDesc, payload)

            var param = {
                boardSeq: store.state.board.boardSeq,
                listSeq: payload.listSeq,
                seq: payload.seq,
                title: payload.cardTitle,
                description: payload.description
            }
            axios.post(UrlConfig.board.saveCardContent, param)
        },
        [StoreConfig.getBoard]: (store, payload) => {
            store.commit(StoreConfig.getBoard, payload)
        },
        [StoreConfig.setKeyword]: (store, payload) => {
            store.commit(StoreConfig.setKeyword, payload)
        },
        [StoreConfig.changeListSort]: (store, payload) => {
            if (store.state.board.canEditing == false) {
                return;
            }

            store.commit(StoreConfig.changeListSort, payload)

            if (payload.updates && payload.updates.length > 0) {
                axios.post(UrlConfig.board.updateListPrevSeq, payload.updates)
            }
        },
        [StoreConfig.changeCardSort]: (store, payload) => {
            if (store.state.board.canEditing == false) {
                return;
            }

            if (payload.removedIndex != null) {
                store.state.cardSort.removed = {
                    listSeq: payload.listSeq,
                    index: payload.removedIndex,
                }
            }
            if (payload.addedIndex != null) {
                store.state.cardSort.added = {
                    listSeq: payload.listSeq,
                    index: payload.addedIndex
                }
            }

            if (store.state.cardSort.removed && store.state.cardSort.added) {
                if (store.state.cardSort.removed.listSeq == store.state.cardSort.added.listSeq && store.state.cardSort.removed.index == store.state.cardSort.added.index) {
                    return
                }

                var cardSort = Object.assign({}, store.state.cardSort)
                store.commit(StoreConfig.changeCardSort, cardSort)
                store.state.cardSort.removed = null
                store.state.cardSort.added = null

                if (cardSort.updates && cardSort.updates.length > 0) {
                    axios.post(UrlConfig.board.updateCardPrevSeq, cardSort.updates)
                }
            }
        },
        [StoreConfig.deleteBoard]: (store, payload) => {
            var param = {
                boardSeq: store.state.board.boardSeq
            }
            axios.post(UrlConfig.board.deleteBoard, param)
                .then(() => {
                    payload.callback && payload.callback()
                })
                .catch(() => {
                    payload.callback && payload.callback()
                })

            store.commit(StoreConfig.deleteBoard)
        },
        [StoreConfig.setPublic]: (store, payload) => {
            store.commit(StoreConfig.setPublic, payload)
            var param = {
                boardSeq: store.state.board.boardSeq,
                isPublic: payload
            }
            axios.post(UrlConfig.board.updateIsPublic, param)
        }
    }
})