helm repo add hashicorp https://helm.releases.hashicorp.com
heml repo update

helm install consul hashicorp/consul  --namespace common-services --values values.yaml
kubectl delete MutatingWebhookConfiguration consul-connect-injector