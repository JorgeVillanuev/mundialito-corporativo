# Contexto de build: la raíz del repo (no docker/), mismo criterio que api.Dockerfile.
FROM node:20-alpine AS build
WORKDIR /app

COPY frontend/package.json frontend/package-lock.json ./
RUN npm ci

# Incluye frontend/.env.local (no está en .gitignore a propósito): next build necesita
# NEXT_PUBLIC_API_URL en tiempo de compilación, no de ejecución.
COPY frontend/ .
RUN npm run build

FROM node:20-alpine AS runtime
WORKDIR /app
ENV NODE_ENV=production

COPY --from=build /app/package.json ./package.json
COPY --from=build /app/node_modules ./node_modules
COPY --from=build /app/.next ./.next
COPY --from=build /app/public ./public
COPY --from=build /app/next.config.ts ./next.config.ts

EXPOSE 3000
CMD ["npm", "start"]
