# Sparkplug B to UNS Bridge PoC

This project runs a complete environment for prototyping Sparkplug B to UNS (Unified Namespace) translation using Ignition, RabbitMQ (with MQTT plugin), and a .NET bridge container.

## Running Locally

1. `docker-compose up --build`
2. Visit:
    - [Ignition UI](http://localhost:8088)
    - [RabbitMQ UI](http://localhost:15672) (user/password)
3. The bridge container will listen for Sparkplug B messages and republish them in UNS format to RabbitMQ MQTT.

## GitHub Setup

See instructions in this README for pushing to your own repo.
