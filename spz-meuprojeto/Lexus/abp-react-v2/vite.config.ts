import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import tailwindcss from '@tailwindcss/vite'
import path from 'path'

// https://vite.dev/config/
export default defineConfig({
  plugins: [
    react(),
    tailwindcss(),
  ],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, './src'),
    },
  },
  server: {
    port: 3000,
    proxy: {
      '/api': {
        target: 'https://localhost:44322',
        changeOrigin: true,
        secure: false,
      },
      '/connect': {
        target: 'https://localhost:44322',
        changeOrigin: true,
        secure: false,
      },
      '/Account': {
        target: 'https://localhost:44322',
        changeOrigin: true,
        secure: false,
      },
      '/.well-known': {
        target: 'https://localhost:44322',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
