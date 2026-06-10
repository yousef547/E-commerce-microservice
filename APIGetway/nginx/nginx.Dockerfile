FROM nginx

COPY APIGetway/nginx/nginx.local.conf /etc/nginx/nginx.conf
COPY APIGetway/nginx/id-local.crt /etc/ssl/certs/id-local.eshopping.com.crt
COPY APIGetway/nginx/id-local.key /etc/ssl/private/id-local.eshopping.com.key
