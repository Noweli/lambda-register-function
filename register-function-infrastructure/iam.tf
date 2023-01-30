data "aws_iam_policy_document" "register-function_assume-role_policy-document" {
  statement {
    actions = ["sts:AssumeRole"]

    principals {
      type        = "Service"
      identifiers = ["lambda.amazonaws.com"]
    }

    effect = "Allow"
  }
}

data "aws_iam_policy_document" "register-function_cloudwatch_policy-document" {
  statement {
    actions = [
      "logs:CreateLogGroup",
      "logs:CreateLogStream",
      "logs:PutLogEvents"
    ]
    effect    = "Allow"
    resources = [aws_cloudwatch_log_group.lambda_log_group.arn]
  }
}

data "aws_secretsmanager_secret" "user-pool_secret" {
  name = local.user_pool_credentials_secret_name
}

data "aws_iam_policy_document" "register-function_secrets-manager_policy-document" {
  statement {
    actions = [
      "secretsmanager:DescribeSecret",
      "secretsmanager:GetSecretValue"
    ]
    effect    = "Allow"
    resources = [aws_secretsmanager_secret.user-pool_secret.arn]
  }
}

resource "aws_iam_role" "register-function_iam-role" {
  name               = "register-function_role"
  assume_role_policy = data.aws_iam_policy_document.register-function_assume-role_policy-document.json
}

resource "aws_iam_policy" "lambda_logging" {
  name   = "register-function_cloudwatch-logging-policy"
  path   = "/"
  policy = data.aws_iam_policy_document.register-function_cloudwatch_policy-document.json
}

resource "aws_iam_policy" "lambda_secrets-manager" {
  name   = "register-function_secrets-manager_policy"
  path   = "/"
  policy = data.aws_iam_policy_document.register-function_secrets-manager_policy-document.json
}

resource "aws_iam_role_policy_attachment" "register-function_cloudwatch-logging_attachment" {
  role       = aws_iam_role.register-function_iam-role.name
  policy_arn = aws_iam_policy.lambda_logging.arn
}

resource "aws_iam_role_policy_attachment" "register-function_secrets-manager_attachment" {
  role       = aws_iam_role.register-function_iam-role.name
  policy_arn = aws_iam_policy.lambda_secrets-manager.arn
}

