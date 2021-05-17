kubectl delete deployments keycloak-db
kubectl delete secrets keycloak-db-secret
kubectl delete pvc keycloak-db-v-claim
kubectl delete pv keycloak-db-volume
kubectl delete services keycloak-db