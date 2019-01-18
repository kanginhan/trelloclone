import Vue from 'vue'
import './plugins/vuetify'
import App from './App.vue'
import router from './routes'
import store from './store'
import VeeValidate from 'vee-validate'
import Lodash from 'lodash'
import axios from 'axios'
import NProgress from 'nprogress'

import 'nprogress/nprogress.css'

// axios
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

Vue.config.productionTip = false

Vue.use(Lodash);
Vue.use(VeeValidate, {
    classes: true,
    classNames: {
        valid: 'is-valid',
        invalid: 'is-invalid'
    }
});

new Vue({
    router,
    store,
    render: h => h(App)
}).$mount('#app')