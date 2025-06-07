# Sparkplug B to UNS Bridge PoC

This project runs a complete environment for prototyping Sparkplug B to UNS (Unified Namespace) translation using Ignition, RabbitMQ (with MQTT plugin), and a .NET bridge container.

Build the Docker container
docker compose build bridge
docker compose up rabbitmq ignition
docker compose up bridge  # docker-compose up --build bridge

Start the ignition containers
docker compose up ignition
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

Start the rabbitmq container
docker compose up rabbitmq

Check listening ports
sudo ss -tulpn | grep ':1883\b

Check if port is mapped (e.g. 1883)
nc -zv 127.0.0.1 <port>

Check all Vhosts in the container (e.g. rabbitmq)
docker exec <container_name> rabbitmqctl list_vhosts

Add vhost /
docker exec -it rabbitmq rabbitmqctl add_vhost /

To change User/Password and permissions
Add a user with permissions: user and Password: password
docker exec -it rabbitmq rabbitmqctl add_user user password
docker exec -it rabbitmq rabbitmqctl set_permissions -p / user ".*" ".*" ".*"
docker exec -it rabbitmq rabbitmqctl set_user_tags user administrator

Start the Bridge container and the log
docker compose up bridge / docker compose logs -f bridge

Check Container Logs
docker logs <container_name> 2>&1 | grep -i mqtt
Test MQTT
mosquitto_sub -h localhost -p 1883 -u user -P password -t "#" -v
mosquitto_pub -h localhost -p 1883 -u user -P password -t "test" -m "Hello from pub"

GC logs in ignition container
docker exec -it <container_name> tail -f /var/log/ignition/gc.log

If port is taken
Old containers obtaining ports
Killing old containers
docker ps -a
docker compose down
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
4. https://sparkplug.eclipse.org/
## GitHub Setup

See instructions in this README for pushing to your own repo.

//ToDo
Copy files from and into athe container
ex.
docker cp ignition:/usr/local/bin/ignition/data/ignition.conf ./ignition.conf
docker cp ./ignition.conf ignition:/usr/local/bin/ignition/data/ignition.conf
Restart the container
docker restart <container_name>

To enable logs in ignition and to retain changes across container rebuilds, mount ignition.conf 
and the log directory as volumes run docker exec -it <container_name> tail -f /var/log/ignition/gc.log

yaml
services:
  ignition:
    image: your-ignition-image
    volumes:
      # Mount ignition.conf
      - ./ignition.conf:/usr/local/ignition/ignition.conf
      # Mount GC logs directory
      - ./logs:/var/log/ignition
    environment:
      # Ensure Ignition user has write permissions
      - USER_ID=1000
      - GROUP_ID=1000

docker-compose down ignition
docker-compose up -d ignition
