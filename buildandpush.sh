#!/bin/bash
NAME=rmmodule
CR=uneidel.azurecr.io
TAG=$1
CONTAINERNAME=testing

echo "-> compiling edgerest"
dotnet publish $PWD/edgerest.csproj
echo "-> compiling edgetoredis"
dotnet publish $HOME/Dokumente/edgetoredis/edgetorock.csproj
echo "-> copying edgetoredis to edgerest"
cp -R --force $HOME/Dokumente/edgetoredis/bin/Debug/netcoreapp2.0/publish/* $PWD/synctool
echo "-> removing docker image"
docker rmi -f $CR/$NAME:latest
echo "-> building new image"
sudo docker rm -f $CONTAINERNAME
sudo docker build -f "$HOME/Dokumente/edgerest/Dockerfile" --build-arg EXE_DIR="./bin/Debug/netcoreapp2.0/publish" -t "$CR/$NAME:$TAG" "$HOME/Dokumente/edgerest"
sudo docker push $CR/$NAME:latest
#sudo docker run -it -p 80:80 -p 8080:8080 --name $CONTAINERNAME $CR/$NAME:$TAG /bin/bash

echo "Current Build Version: " $CR/$NAME:$TAG
