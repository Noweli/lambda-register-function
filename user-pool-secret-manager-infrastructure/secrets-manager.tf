resource "aws_secretsmanager_secret" "user-pool_secret" {
  name = local.secret-name
}
