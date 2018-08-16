# Serverless Framework Cheat Sheet
AWS Reference:
https://docs.aws.amazon.com/AWSJavaScriptSDK/latest/index.html
https://docs.aws.amazon.com/sdk-for-javascript/v2/developer-guide/welcome.html

Resources: https://docs.aws.amazon.com/AWSCloudFormation/latest/UserGuide/resources-section-structure.html
https://docs.aws.amazon.com/AWSCloudFormation/latest/UserGuide/aws-template-resource-type-ref.html

IAM: https://docs.aws.amazon.com/IAM/latest/UserGuide/reference_policies_elements.html


Serverless Framework Reference:
Events: https://serverless.com/framework/docs/providers/aws/events/
Resources: https://serverless.com/framework/docs/providers/aws/guide/resources/
Variables: https://serverless.com/framework/docs/providers/aws/guide/variables/
IAM: https://serverless.com/framework/docs/providers/aws/guide/iam/


		
	Serverless Sample:
		service:
			name: myService

		frameworkVersion: ">=1.0.0 <2.0.0"

		provider:
			name: aws
			runtime: nodejs6.10
			stage: dev # Set the default stage used. Default is dev
			region: us-east-1 # Overwrite the default region used. Default is us-east-1
			environment: # Service wide environment variables
				serviceEnvVar: 123456789
			iamRoleStatements: # IAM role statements so that services can be accessed in the AWS account
				- Effect: 'Allow'
				Action:
					- 's3:ListBucket'
				Resource:
					Fn::Join:
					- ''
					- - 'arn:aws:s3:::'
					- Ref: ServerlessDeploymentBucket

			tags: # Optional service wide function tags
				foo: bar
				baz: qux

		Events:
			events: # The Events that trigger this Function
			  - http: # This creates an API Gateway HTTP endpoint which can be used to trigger this function.  Learn more in "events/apigateway"
				  path: users/create # Path for this endpoint
				  method: get # HTTP method for this endpoint
				  cors: true # Turn on CORS for this endpoint, but don't forget to return the right header in your response
				  private: true # Requires clients to add API keys values in the `x-api-key` header of their request
				  authorizer: # An AWS API Gateway custom authorizer function
					name: authorizerFunc # The name of the authorizer function (must be in this service)
					arn:  xxx:xxx:Lambda-Name # Can be used instead of name to reference a function outside of service
					resultTtlInSeconds: 0
					identitySource: method.request.header.Authorization
					identityValidationExpression: someRegex
			  - s3:
				  bucket: photos
				  event: s3:ObjectCreated:*
				  rules:
					- prefix: uploads/
					- suffix: .jpg
			  - schedule:
				  name: my scheduled event
				  description: a description of my scheduled event's purpose
				  rate: rate(10 minutes)
				  enabled: false
				  input:
					key1: value1
					key2: value2
					stageParams:
					  stage: dev
				  inputPath: '$.stageVariables'
			  - stream:
				  arn: arn:aws:kinesis:region:XXXXXX:stream/foo
				  batchSize: 100
				  startingPosition: LATEST
				  enabled: false
			  - cloudwatchEvent:
				  event:
					source:
					  - "aws.ec2"
					detail-type:
					  - "EC2 Instance State-change Notification"
					detail:
					  state:
						- pending
				  # Note: you can either use "input" or "inputPath"
				  input:
					key1: value1
					key2: value2
					stageParams:
					  stage: dev
				  inputPath: '$.stageVariables'
			  - cloudwatchLog:
				  logGroup: '/aws/lambda/hello'
				  filter: '{$.userIdentity.type = Root}'
				  
		Resources:
			LogicalName:
			Type: AWS::DynamoDB::Table
			Properties:
				TableName: my-new-table
				AttributeDefinitions:
					- AttributeName: data
					AttributeType: S
				KeySchema:
					- AttributeName: data
					KeyType: HASH
				ProvisionedThroughput:
					ReadCapacityUnits: 1
					WriteCapacityUnits: 1
				StreamSpecification:
					StreamViewType: NEW_IMAGE
			  
			LogicalName:			
				Type: AWS::S3::Bucket
				Properties:
					BucketName: my-new-bucket
