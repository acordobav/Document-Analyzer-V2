FROM python:3.8-slim-buster

WORKDIR /app

# Install the required libraries
RUN pip3 install pika websockets

COPY . .

EXPOSE 8765

# Execute the python file
CMD [ "python3", "broker.py"]


# docker image build . -t arturocv/docanalyzer-websocket
# docker push arturocv/docanalyzer-websocket
# docker run -d -p 8765:8765 arturocv/docanalyzer-websocket

