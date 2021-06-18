file=./configmap.yml

# Recreate config file
rm -rf $file
touch $file

# Insert default content in the file
echo 'apiVersion: v1
kind: ConfigMap
metadata:
  name: backend-endpoints
data:' >> $file

# Obtain external ip values from kubectl and store 
# them into the config file
kubectl get services | 
while read name type clusterip externalip port age;
do
    if [ $name == "documentanalyzerapi" ] || 
       [ $name == "keycloak" ] || 
       [ $name == "websocket" ];
    then
        echo '  '$name': "'$externalip'"' >> $file
    fi
done