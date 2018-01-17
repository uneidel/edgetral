#!/bin/bash
NAME=edgemodule
CR=uneidel.azurecr.io
dotnet publish ./edgetorock/edtorock.csproj
dotnet publish 
docker rmi $NAME:latest
sudo docker build -f "/home/batzi/Dokumente/edgerest/Dockerfile" --build-arg EXE_DIR="./bin/Debug/netcoreapp2.0/publish" -t "uneidel.azurecr.io/$NAME:latest" "/home/batzi/Dokumente/edgerest"
#docker push $CR/$NAME:latest
sudo docker run -it -p 80:80 -p 8080:8080 $CR/$NAME:latest /bin/bash
