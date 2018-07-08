#!/usr/bin/env bash

docker build --no-cache -t dotnetusers_test .
docker run -d -p 5000:80 --name dotnetusers dotnetusers_test