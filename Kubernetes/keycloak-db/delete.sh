kubectl delete deployments keycloak-db
kubectl delete secrets keycloak-db-secret
kubectl delete -f persistent-volume.yml
kubectl delete services keycloak-db