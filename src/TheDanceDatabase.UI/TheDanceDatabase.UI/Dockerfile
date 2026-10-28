# syntax=docker/dockerfile:1
# Build context is src/TheDanceDatabase.UI, the UI's container folder, to match the API.
# From that folder: docker build -f TheDanceDatabase.UI/Dockerfile .
FROM node:24-alpine AS build
WORKDIR /src

COPY TheDanceDatabase.UI/package.json TheDanceDatabase.UI/package-lock.json ./
RUN npm ci

COPY TheDanceDatabase.UI/ ./
RUN npm run build

FROM nginxinc/nginx-unprivileged:1.30-alpine AS final
COPY TheDanceDatabase.UI/nginx.conf /etc/nginx/conf.d/default.conf

# A fresh root keeps the base image's sample pages out of the site
COPY --from=build /src/dist /usr/share/nginx/ui
EXPOSE 8080
