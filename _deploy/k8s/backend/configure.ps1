$consul_dir = ".\consul_bin"
$env:path += ";${consul_dir}"

$data = kubectl get secrets/consul-bootstrap-acl-token  --namespace=common-services -o json | ConvertFrom-Json
$masterTokenEncoded  = $data.data.token
$masterToken = [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($masterTokenEncoded))
$env:CONSUL_HTTP_TOKEN="${masterToken}"

$data = kubectl get service ingress-nginx-controller --namespace=ingress-nginx -o json | ConvertFrom-Json
$ip  = $data.status.loadBalancer.ingress.ip
$env:CONSUL_HTTP_ADDR="${ip}:31001"

$consul_link = "https://releases.hashicorp.com/consul/1.15.2+ent/consul_1.15.2+ent_windows_amd64.zip"

if(!(Test-Path $consul_dir)) {
    New-Item -ItemType Directory $consul_dir
}

kubectl apply -f dev.yaml
write-host "pobieranie consul"
$WebClient = New-Object System.Net.WebClient

$WebClient.DownloadFile("${consul_link}","$consul_dir\consul_windows_amd64.zip")
Expand-Archive "${consul_dir}/consul_windows_amd64.zip" "$consul_dir" -Force

write-host "wgrywanie polityk"
consul acl policy create -name="dev-kv-write" -description="Odczyt/Zapis ustawien dla DEV" -rules @dev_settings\dev-write-policy.hcl
$secretData = consul acl token create -description "dev-kv-write" -policy-name dev-kv-write -format=json | ConvertFrom-Json
$devToken = $secretData.SecretID
kubectl create secret generic consul-dev-token --from-literal=token=${devToken} --namespace=backend

$settings = Get-Content "dev_settings\settings.json" | ConvertFrom-Json
foreach($setting in $settings) 
{
    $key = $setting.key
    $value = $setting.value

    consul.exe kv put "dev/${key}" "${value}"
}

kubectl delete -f dev.yaml