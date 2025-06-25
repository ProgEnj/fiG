FROM node:slim

WORKDIR /usr/src/app

COPY package.json /usr/src/app
COPY package-lock.json /usr/src/app

RUN npm install -g @angular/cli
RUN npm install

COPY . /usr/src/app
COPY ./src /usr/src/app/

CMD ["ng", "serve", "--host", "0.0.0.0"]