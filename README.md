### .Net Microservices
The purpose of this project is for me to understand how Microservice Architecture systems work. 
I created 2 simple services. then, I deployed them to docker kubernetes. I also used Nginx as a load balancer and to handle external requests.
For communication between the 2 services I used 2 abroaches: 
- Message Bus by RabbitMQ: a message-queueing software that allows communication between services without them knowing each other. It uses the Publish/Subscriber pattern to achieve this.
- gRPC: a high-performance Remote Procedure Call protocol.
