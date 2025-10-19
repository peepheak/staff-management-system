import { createRouter, createWebHistory } from 'vue-router';
import StaffListPage from '../pages/StaffListPage.vue';
import StaffCreatePage from '../pages/StaffCreatePage.vue';
import StaffEditPage from '../pages/StaffEditPage.vue';


const routes = [
    { path: '/', component: StaffListPage },
    { path: '/create', component: StaffCreatePage },
    { path: '/edit/:id', component: StaffEditPage },
];

export const router = createRouter({
    history: createWebHistory(),
    routes,
});
