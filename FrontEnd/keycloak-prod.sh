#!/bin/bash

file=./keycloak.json

# Recreate config file
rm -rf $file
touch $file

content=()

# Read each line in keycloak.env file
# Each line represents key=value pairs
while read -r line || [[ -n "$line" ]];
do
    # Split env variables by character `=`
    if printf '%s\n' "$line" | grep -q -e '='; then
        varname=$(printf '%s\n' "$line" | sed -e 's/=.*//')
        varvalue=$(printf '%s\n' "$line" | sed -e 's/^[^=]*=//')
    fi

    # Read value of current variable if exists as Environment variable
    value=$(printf '%s\n' "${!varname}")
    # Otherwise use value from .env file
    [[ -z $value ]] && value=${varvalue}

    # Append configuration value to array
    content+=($value)

done < keycloak.env

realm=${content[0]}         # First item is realm's name
serverurl=${content[1]}     # Second item is auth server url
serverport=${content[2]}    # Third item is auth server port

# Builds the json configuration file
echo '{' >> $file
echo '    "realm": "'$realm'",' >> $file
echo '    "auth-server-url": "http://'$serverurl':'$serverport'/auth/",' >> $file
echo '    "ssl-required": "external",' >> $file
echo '    "resource": "docanalyzer-react-client",' >> $file
echo '    "public-client": true,' >> $file
echo '    "confidential-port": 0' >> $file
echo '}' >> $file