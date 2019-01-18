import Landing from '../views/Landing.vue'
import Home from '../views/Home.vue'
import Register from '../views/Register.vue'
import Login from '../views/Login.vue'
import Boards from '../views/Boards.vue'

var routes = [{
        path: '/',
        name: 'landing',
        component: Landing
    },
    {
        path: '/home',
        name: 'home',
        component: Home
    },
    {
        path: '/register',
        name: 'register',
        component: Register
    },
    {
        path: '/login',
        name: 'login',
        component: Login
    },
    {
        path: '/boards',
        name: 'boards',
        component: Boards
    }
]

export default routes