cd sentimentanalysisapi && ./delete.sh && cd ..
cd offensivecontentapi  && ./delete.sh && cd ..
cd entityrecognitionapi && ./delete.sh && cd ..
cd frontend             && ./delete.sh && cd ..
cd employeefinder       && ./delete.sh && cd ..
cd websocket            && ./delete.sh && cd ..
cd documentanalyzerapi  && ./delete.sh && cd ..
cd rabbitmq             && ./delete.sh && cd ..
cd keycloak             && ./delete.sh && cd ..
cd postgres             && ./delete.sh && cd ..
cd mongodb              && ./delete.sh && cd ..
cd keycloak-db          && ./delete.sh && cd ..

kubectl delete hpa documentanalyzerapi