docker-compose -f .\docker-compose.yml up -d
docker exec rabbitmq rabbitmq-plugins enable rabbitmq_stream