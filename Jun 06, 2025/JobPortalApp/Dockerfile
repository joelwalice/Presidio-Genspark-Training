FROM node:alpine

WORKDIR /App

COPY . .

RUN npm install -g @angular/cli

RUN npm install

RUN ng build --configuration production

CMD ["ng", "serve", "--host", "0.0.0.0"]

# FROM nginx:alpine
# COPY nginx.conf /etc/nginx/nginx.conf
# COPY . /usr/share/nginx/html
# EXPOSE 80
# CMD ["nginx", "-g", "daemon off;"]