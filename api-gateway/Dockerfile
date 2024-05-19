FROM nginx:alpine

COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
# EXPOSE 443 if you are using SSL

CMD ["nginx", "-g", "daemon off;"]
