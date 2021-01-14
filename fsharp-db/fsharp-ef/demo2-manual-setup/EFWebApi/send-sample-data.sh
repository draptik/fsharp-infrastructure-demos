#!/bin/sh

## -k, --insecure: trust self-signed certificates

curl -k -X POST -d "@sample-data-post.json" \
    https://localhost:5001/person \
    --header "Content-Type:application/json" \
    -v
    
