import os
import pika
import json
import asyncio
import websockets
import threading
import logging

logging.basicConfig()

RESULTS_EXCHANGE = 'analysis_results'
FINDER_EXCHANGE = 'finder_results'

FINDER_QUEUE_NAME = 'finder_result_queue'
SENTIMENT_QUEUE_NAME = 'offensive_result_queue'
OFFENSIVE_QUEUE_NAME = 'sentiment_result_queue'

FINDER_ROUTING_KEY = 'finder'
SENTIMENT_ROUTING_KEY = 'sentiment'
OFFENSIVE_ROUTING_KEY = 'offensive'

receivedFromFinder = False
receivedFromSentiment = False
receivedFromOffensive = False

result_dict = {}

send_data = False


def check_for_data():
    if receivedFromFinder and receivedFromSentiment and receivedFromOffensive:
        return True
    return False


async def data_loop(websocket, path):
    global result_dict

    loop = asyncio.get_running_loop()
    while True:
        is_data_complete = await loop.run_in_executor(None, check_for_data)
        if is_data_complete:
            await websocket.send(json.dumps(result_dict))
            restart_variables()


def restart_variables():
    global receivedFromFinder, receivedFromSentiment, receivedFromOffensive

    receivedFromFinder = False
    receivedFromSentiment = False
    receivedFromOffensive = False


def process_finder_result(result):
    finder_dict = json.loads(result)

    result_dict['id'] = 1
    result_dict['title'] = finder_dict['Title']
    result_dict['url'] = finder_dict['Url']
    result_dict['status'] = True

    ref_list = []

    for ref in finder_dict['Matches']:
        curr_ref_dict = {}
        curr_ref_dict['name'] = ref['employeeName']
        curr_ref_dict['qty'] = ref['qty']
        ref_list.append(curr_ref_dict)

    result_dict['userDocumentReferences'] = ref_list

    print(result_dict)


def process_sentiment_result(result):
    sentiment_dict = json.loads(result)
    result_dict['feelings'] = sentiment_dict
    print(result_dict)


def process_offensive_result(result):
    offensive_result = json.loads(result)

    result_dict['obscene_language'] = offensive_result

    print(result_dict)


# create a function which is called on incoming messages
def callback_finder(ch, method, properties, body):
    global receivedFromFinder, send_data
    receivedFromFinder = True

    process_finder_result(body)

    if receivedFromFinder and receivedFromSentiment and receivedFromOffensive:
        send_data = True


def callback_sentiment(ch, method, properties, body):
    global receivedFromSentiment, send_data
    receivedFromSentiment = True

    process_sentiment_result(body)

    if receivedFromFinder and receivedFromSentiment and receivedFromOffensive:
        send_data = True


def callback_offensive(ch, method, properties, body):
    global receivedFromOffensive, send_data
    receivedFromOffensive = True

    process_offensive_result(body)

    if receivedFromFinder and receivedFromSentiment and receivedFromOffensive:
        send_data = True


def start_mq_listening():
    # Access the CLODUAMQP_URL environment variable and parse it (fallback to localhost)
    url = os.environ.get('CLOUDAMQP_URL',
                         'amqps://ayfpwbex:M_cMEhP-zE1VKSduyZa02bcck07zC8-d@orangutan.rmq.cloudamqp.com/ayfpwbex')
    params = pika.URLParameters(url)
    connection = pika.BlockingConnection(params)

    # Starts the communication channel
    channel = connection.channel()

    # Exchange declaration
    channel.exchange_declare(exchange=RESULTS_EXCHANGE, exchange_type='direct')
    channel.exchange_declare(exchange=FINDER_EXCHANGE, exchange_type='direct')

    # Queue declaration
    channel.queue_declare(queue=FINDER_QUEUE_NAME)
    channel.queue_declare(queue=SENTIMENT_QUEUE_NAME)
    channel.queue_declare(queue=OFFENSIVE_QUEUE_NAME)

    # Bindings declaration
    channel.queue_bind(exchange=FINDER_EXCHANGE, queue=FINDER_QUEUE_NAME, routing_key=FINDER_ROUTING_KEY)
    channel.queue_bind(exchange=RESULTS_EXCHANGE, queue=SENTIMENT_QUEUE_NAME, routing_key=SENTIMENT_ROUTING_KEY)
    channel.queue_bind(exchange=RESULTS_EXCHANGE, queue=OFFENSIVE_QUEUE_NAME, routing_key=OFFENSIVE_ROUTING_KEY)

    # set up subscription on the queue
    channel.basic_consume(queue=FINDER_QUEUE_NAME, on_message_callback=callback_finder, auto_ack=True)
    channel.basic_consume(queue=SENTIMENT_QUEUE_NAME, on_message_callback=callback_sentiment, auto_ack=True)
    channel.basic_consume(queue=OFFENSIVE_QUEUE_NAME, on_message_callback=callback_offensive, auto_ack=True)

    # start consuming (blocks)
    channel.start_consuming()
    connection.close()


if __name__ == "__main__":
    mq_thread = threading.Thread(target=start_mq_listening, args=())
    mq_thread.start()

    start_server = websockets.serve(data_loop, "localhost", 8765)
    asyncio.get_event_loop().run_until_complete(start_server)
    asyncio.get_event_loop().run_forever()
