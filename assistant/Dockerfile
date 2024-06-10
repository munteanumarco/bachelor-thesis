FROM node:16-alpine
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
EXPOSE 3001
ENV NODE_ENV production
CMD ["node", "index.js"]
