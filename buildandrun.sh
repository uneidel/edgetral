#!/bin/bash
NAME=foo
CR=uneidel.azurecr.io

echo "-> compiling edgerest"
dotnet publish $PWD/edgerest.csproj
echo "-> compiling edgetorock"
dotnet publish /home/batzi/Dokumente/edgetorock/edgetorock.csproj
echo "-> removing docker image"
docker rmi -f $CR/$NAME:latest
echo "-> building new image"
sudo docker build -f "/home/batzi/Dokumente/edgerest/Dockerfile" --build-arg EXE_DIR="./bin/Debug/netcoreapp2.0/publish" -t "$CR/$NAME:latest" "/home/batzi/Dokumente/edgerest"
#docker push $CR/$NAME:latest
sudo docker run -it -p 80:80 -p 8080:8080 $CR/$NAME:latest /bin/bash

echo "Current Build Version: " $CR/$NAME:latest
