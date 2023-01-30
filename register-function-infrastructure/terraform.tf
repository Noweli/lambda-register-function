provider "aws" {
  region = var.region
}

terraform {
  backend "s3" {
    bucket = local.s3_infrastructure-bucket_name
    key    = "authorization/register/function/state/register-function.tfstate"
    region = var.region
  }
}

variable "environment" {
  description = "Environment name - dev, sbx, prd"
}

variable "region" {
  description = "AWS Region"
}


locals {
  s3_infrastructure-bucket_name     = "aspharax-infrastructure"
  product_name                      = "UserRegisterFunction"
  function_name                     = "${var.environment}_${var.region}_register-function"
  s3_lambda-function_key            = "authorization/register/function/code/UserRegisterFunction.zip"
  user_pool_credentials_secret_name = "MatchUpSiteUserPoolCredentials"
}
