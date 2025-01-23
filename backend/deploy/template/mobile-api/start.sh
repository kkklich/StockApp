cd $(dirname $0)
docker stop af_mobile_net_api 2>/dev/null
docker rm af_mobile_net_api 2>/dev/null

docker build  -t af_mobile_net_api .
docker run -d -e TZ=Europe/Warsaw  -e  ASPNETCORE_URLS="http://[0.0.0.0]:6231" --name=af_mobile_net_api --net=host --restart=always af_mobile_net_api
