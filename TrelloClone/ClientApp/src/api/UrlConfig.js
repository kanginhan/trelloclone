var BASE_URL = "/api";

export default {
    test: {
        whoami: BASE_URL + "/values"
    },
    auth: {
        register: BASE_URL + "/auth/register",
        login: BASE_URL + "/auth/login",
        checkToken: BASE_URL + "/auth/checkToken",
        logout: BASE_URL + "/auth/logout",
        checkDuplication: BASE_URL + "/auth/duplication",
    },
    board: {
        getBoard: BASE_URL + "/board/getBoard",
        saveList: BASE_URL + "/board/saveList",
        saveBoardTitle: BASE_URL + "/board/saveBoardTitle",
        saveCard: BASE_URL + "/board/saveCard",
        deleteList: BASE_URL + "/board/deleteList",
        deleteCard: BASE_URL + "/board/deleteCard",
        updateListPrevSeq: BASE_URL + "/board/updateListPrevSeq",
        updateCardPrevSeq: BASE_URL + "/board/updateCardPrevSeq",
        saveListTitle: BASE_URL + "/board/saveListTitle",
        saveCardContent: BASE_URL + "/board/saveCardContent",
        getBoardList: BASE_URL + "/board/getBoardList",
        deleteBoard: BASE_URL + "/board/deleteBoard",
        saveBoard: BASE_URL + "/board/saveBoard",
        updateIsPublic: BASE_URL + "/board/updateIsPublic"
    }
}