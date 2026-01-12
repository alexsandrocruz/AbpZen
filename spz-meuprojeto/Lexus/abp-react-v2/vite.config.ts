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
        configure: (proxy, options) => {
          proxy.on('proxyReq', (proxyReq, req, res) => {
            // Extract XSRF token from cookies and add to headers
            const cookies = req.headers.cookie || '';
            console.log(`[PROXY] Request cookies: ${cookies.substring(0, 50)}...`);
            const xsrfMatch = cookies.match(/XSRF-TOKEN=([^;]+)/);
            if (xsrfMatch) {
              const token = decodeURIComponent(xsrfMatch[1]);
              console.log(`[PROXY] Found XSRF token: ${token.substring(0, 10)}...`);
              proxyReq.setHeader('X-XSRF-TOKEN', token);
              proxyReq.setHeader('RequestVerificationToken', token);
            } else {
              console.log(`[PROXY] No XSRF-TOKEN found in cookies`);
            }

            console.log(`[PROXY] ${req.method} ${req.url} -> ${options.target}${req.url}`);
          });
          proxy.on('proxyRes', (proxyRes, req, res) => {
            console.log(`[PROXY] Response: ${proxyRes.statusCode} for ${req.url}`);
          });
        },
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
