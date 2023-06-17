docker build  --force-rm -t xrest/xrest-identity-api -f XRest.Identity.Service/Dockerfile .
docker build  --force-rm -t xrest/xrest-restaurants-api -f XRest.Restaurants.Service/Dockerfile .
docker build  --force-rm -t xrest/xrest-orders-api -f XRest.Orders.Service/Dockerfile .
docker build  --force-rm -t xrest/xrest-images-api -f XRest.Images.Service/Dockerfile .



