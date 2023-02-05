FROM rabbitmq:management

RUN rabbitmq-plugins enable --offline \
    rabbitmq_mqtt \
    rabbitmq_federation_management \
    rabbitmq_federation \
    rabbitmq_management \
    rabbitmq_web_stomp \
    rabbitmq_stomp \
    rabbit_stream

EXPOSE 5672 15672 61613 15674 5552