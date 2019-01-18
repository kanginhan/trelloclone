import Vue from 'vue'
import axios from 'axios'
import NProgress from 'nprogress'

import 'nprogress/nprogress.css'

const instanceAxios = axios.create()
instanceAxios.interceptors.request.use(config => {
    NProgress.start()
    return config
})
instanceAxios.interceptors.response.use(response => {
    NProgress.done()
    return response
}, error => {
    NProgress.done()
    return Promise.reject(error)
})
instanceAxios.defaults.withCredentials = true;


Vue.prototype.$axios = instanceAxios;
Vue.prototype.$nprogress = NProgress;