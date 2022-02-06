# Microservices with .NET 6

# Command : 

## Compose Up
docker-compose up -d

## Compose Down
docker-compose down

## Stop/Remove All Container 
docker stop/rm $(docker ps -aq)

## Remove All Images 
docker rmi $(docker ps -aq)