kubectl apply -f ns.yaml
kubectl create secret generic seq-api-key-token --from-literal=xrest-images=0xpKglVqwLpFKdCpu9zD  --from-literal=xrest-identity=a00ZKNwD1A2AFz3c8rkm --from-literal=xrest-orders=BhKdpTyjvrNkan4WYZQB --from-literal=xrest-restaurants=CBZvgJIP1Ar3rQu8aT6v  --namespace=backend
kubectl apply -f xrest-identity-api.yaml
kubectl apply -f xrest-images-api.yaml
kubectl apply -f xrest-orders-api.yaml
kubectl apply -f xrest-restaurants-api.yaml
