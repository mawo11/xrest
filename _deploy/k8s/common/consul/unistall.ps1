kubectl delete crd --selector app=consul
kubectl config set-context --current --namespace=common-services-consul
helm uninstall consul
kubectl delete pvc --selector="chart=consul-helm"
kubectl delete secret consul-bootstrap-acl-token
kubectl config set-context --current --namespace=default
kubectl delete ns consul

