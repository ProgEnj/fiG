FROM node:slim as build

WORKDIR /usr/src/app

COPY package.json package-lock.json ./ 

RUN npm install
COPY . . 
RUN npm run build

FROM nginx as run

COPY nginx/nginx.conf /etc/nginx/conf.d/default.conf
COPY --from=build /usr/src/app/dist/frontend /usr/share/nginx/html