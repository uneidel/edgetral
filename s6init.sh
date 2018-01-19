#!/usr/bin/with-contenv bash
service nginx start
nohup dotnet /app/edgerest.dll &
#nohup dotnet /synctool/edgetorock.dll &