key_prefix "" {
  policy = "read"
}

key_prefix "dev/" {
  policy = "write"
}
key_prefix "dev/" {
  policy = "list"
}

service_prefix "dev-" {
    policy = "write"
}

service_prefix "" {
    policy = "read"
}

node_prefix "" {
  policy = "read"
}
node  "dc1" {
  policy = "write"
}
