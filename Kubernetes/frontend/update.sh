./create-configmap.sh

kubectl apply -f configmap.yml

kubectl apply -f service.yml

kubectl apply -f deployment.yml