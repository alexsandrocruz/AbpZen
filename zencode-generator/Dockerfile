# Stage 1: Build Frontend
FROM node:20-alpine AS frontend-builder
WORKDIR /app
COPY zencode-generator/package*.json ./
RUN npm ci
COPY zencode-generator/ .
RUN npm run build

# Stage 2: Runtime
FROM node:20-alpine
WORKDIR /app

# Install Git and OpenSSH (Required for generator features)
RUN apk add --no-cache git openssh-client

# Install production dependencies
COPY zencode-generator/package*.json ./
RUN npm ci --omit=dev

# Copy backend files
COPY zencode-generator/bridge.js zencode-generator/injector.js zencode-generator/db-importer.js ./
COPY --from=frontend-builder /app/dist ./dist

# [NEW] Copy templates folder to a sibling location inside the container
COPY zencode-template /zencode-template

ENV PORT=80
EXPOSE 80

CMD ["node", "bridge.js"]
