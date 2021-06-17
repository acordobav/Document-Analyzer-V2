kubectl delete deployment docanalyzer-postgres
kubectl delete service docanalyzer-postgres
kubectl delete configmap docanalyzer-postgres-config
kubectl delete pvc docanalyzer-postgres-v-claim
kubectl delete pv docanalyzer-postgres-volume
kubectl delete secrets docanalyzer-postgres-secret
