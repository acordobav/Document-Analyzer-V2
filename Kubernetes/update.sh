cd keycloak-db          && ./update.sh && cd ..
cd mongodb              && ./update.sh && cd ..
cd postgres             && ./update.sh && cd ..
cd keycloak             && ./update.sh && cd ..
cd websocket            && ./update.sh && cd ..
cd rabbitmq             && ./update.sh && cd ..
cd documentanalyzerapi  && ./update.sh && cd ..
cd employeefinder       && ./update.sh && cd ..
cd entityrecognitionapi && ./update.sh && cd ..
cd offensivecontentapi  && ./update.sh && cd ..
cd sentimentanalysisapi && ./update.sh && cd ..
cd frontend             && ./update.sh && cd ..

kubectl autoscale deployment documentanalyzerapi --cpu-percent=50 --min=1 --max=6