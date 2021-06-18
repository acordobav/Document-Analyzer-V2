# => Build container
FROM node:10 as builder
RUN mkdir /app
WORKDIR /app
COPY package.json /app
RUN npm install
COPY . /app
RUN npm run build

# => Run container
FROM nginx:1.15.2-alpine

# Nginx config
RUN rm -rf /etc/nginx/conf.d
COPY conf /etc/nginx

# Static build
COPY --from=builder /app/build /usr/share/nginx/html/

# Default port exposure
EXPOSE 80

# Copy .env file and shell script to container
WORKDIR /usr/share/nginx/html
COPY ./env-prod.sh .
COPY .env .

# Copy keycloak .env and bash script to container
COPY ./keycloak-prod.sh .
COPY ./keycloak.env .

# Add bash
RUN apk add --no-cache bash

# Make our shell script executable
RUN chmod +x env-prod.sh
RUN chmod +x keycloak-prod.sh

# Start Nginx server
CMD ["/bin/bash", "-c", "/usr/share/nginx/html/env-prod.sh && /usr/share/nginx/html/keycloak-prod.sh && nginx -g \"daemon off;\""]


# docker image build . -t arturocv/docanalyzer-frontend
# docker push arturocv/docanalyzer-frontend
# docker run -d -p 6413:80 arturocv/docanalyzer-frontend
# docker run -e DOCANALYZER_HOST=10.99.126.214 -e DOCANALYZER_PORT=80 -e WEBSOCKET_HOST=10.105.211.239 -e WEBSOCKET_PORT=8765 -e REALM=docanalyzer -e AUTHSERVERURL=10.102.164.168 -e AUTHSERVERPORT=8080 -p 3000:80 arturocv/docanalyzer-frontend