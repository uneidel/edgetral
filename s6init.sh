#!/usr/bin/with-contenv bash
service nginx start
service redis-server start
nohup dotnet /app/edgerest.dll &
#nohup dotnet /synctool/edgetorock.dll &