import {defineConfig} from 'vite';
import tailwindcss from '@tailwindcss/vite';

export default defineConfig({
    plugins: [tailwindcss()],
    build:{
            rollupOptions: {
        input: {
        main: 'index.html',
        login: 'src/pages/login/login.html',
        register: 'src/pages/register/register.html',
        todo: 'src/pages/todo/todo.html',
        }
    }
    }
})