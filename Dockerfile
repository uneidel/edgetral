#FROM microsoft/dotnet:2.0.0-runtime-stretch
FROM microsoft/dotnet:2.0-sdk-stretch
RUN apt-get update && apt-get install -y nginx bash
COPY index.html /var/www/html
COPY ./Docker/s6-overlay-amd64.tar.gz /tmp/ 
RUN tar xvfz /tmp/s6-overlay-amd64.tar.gz -C /
COPY /services /etc/s6/services/
COPY s6init.sh /etc/cont-init.d/
COPY ./Docker/reverseproxy /etc/nginx/sites-available/
COPY ./Docker/reverseproxy /etc/nginx/sites-enabled/
ARG EXE_DIR=.

WORKDIR /app
COPY $EXE_DIR/ ./


COPY ../edgetorock/bin/Debug/netcoreapp2.0/publish/ /synctool
# testing
#ADD edgetorock/bin/Debug/amd64/* /synctool/

#Do Cleanup
#RUN apt-get -y autoremove && apt-get clean && rm -rf /var/lib/apt/lists/*
WORKDIR /synctool
ENTRYPOINT ["/init"]


#CMD service nginx start
#CMD ["dotnet", "edgerest.dll"]