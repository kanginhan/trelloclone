import Vue from 'vue'
import './plugins/vuetify'
import App from './App.vue'
import router from './routes'
import store from './store'
import VeeValidate from 'vee-validate'
import Lodash from 'lodash'

require('./api')

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