# Stage 1: Build Frontend
FROM node:20-alpine AS frontend-builder
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

# Stage 2: Runtime
FROM node:20-alpine
WORKDIR /app

# Install Git and OpenSSH (Required for generator features)
RUN apk add --no-cache git openssh-client

# Install production dependencies
COPY package*.json ./
RUN npm ci --omit=dev

# Copy backend files
COPY bridge.js injector.js db-importer.js ./
COPY --from=frontend-builder /app/dist ./dist

ENV PORT=80
EXPOSE 80

CMD ["node", "bridge.js"]
