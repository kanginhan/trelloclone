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
    }
}