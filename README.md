# Sparkplug B to UNS Bridge PoC

This project runs a complete environment for prototyping Sparkplug B to UNS (Unified Namespace) translation using Ignition, RabbitMQ (with MQTT plugin), and a .NET bridge container.

Build the Docker container
docker-compose build bridge

Start Docker containers
docker-compose up rabbitmq ignition

Add vhost /
docker exec -it rabbitmq rabbitmqctl add_vhost /

Add a user with permissions: user and Password: password
docker exec -it rabbitmq rabbitmqctl add_user user password
docker exec -it rabbitmq rabbitmqctl set_permissions -p / user ".*" ".*" ".*"
docker exec -it rabbitmq rabbitmqctl set_user_tags user administrator

Start the Bridge container and the log
docker-compose up bridge
docker-compose logs -f bridge

Ignition setup
Open a browser and go to http://<localhost-or-ip>:8088/
Klick on "Standard Edition"
Fill in Username, Enter Password and Confirm Password
Start the Gateway
Login
Download MQTT-Transmission-signed
Install MQTT Transmission
Choose file and klick "Install"
Configure MQTT Transmission server url (tcp://rabbitmq:1883)
Check the "Change Password"
Set the user and password to user/password
Look for Successfully updated MQTT Server 

Download Designer and install
Start Designer and Create a "New Project"



Test MQTT
mosquitto_sub -h localhost -p 1883 -u user -P password -t "#" -v
mosquitto_pub -h localhost -p 1883 -u user -P password -t "test" -m "Hello from pub"


Old containers obtaining ports
Killing old containers
docker ps -a
docker-compose down
docker network ls
docker ps -aq | xargs sudo docker stop
docker ps -aq | xargs sudo docker rm
docker network ls
docker network prune
sudo netstat -tulpn | grep <port>

Killing old ports
sudo lsof -i :<port>
sudo kill <pid>


## Running Locally

1. `docker-compose up --build`
2. Visit:
    - [Ignition UI](http://localhost:8088)
    - [RabbitMQ UI](http://localhost:15672) (user/password)
3. The bridge container will listen for Sparkplug B messages and republish them in UNS format to RabbitMQ MQTT.

## GitHub Setup

See instructions in this README for pushing to your own repo.

