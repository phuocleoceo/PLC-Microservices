# Microservices with .NET 6

# Command : 

## Compose Up
docker-compose -f .\docker-compose.yml up -d

## Compose Down
docker-compose -f .\docker-compose.yml down

## Stop/Remove All Container 
docker stop/rm $(docker ps -aq)

## Remove All Images 
docker rmi $(docker ps -aq)