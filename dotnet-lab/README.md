# serverless-lab
Serverless TODO App


Requirement
TODO App
	- Aplicacion para crear notas/tareas (TODOs)
		- Por cada nota, se genera una notificacion de creacion/edicion/borrada
		- Cada X tiempo, se moveran tareas/notas completadas a archivo
			* - Se requiere crear una cuenta para acceder

Services
	- Dynamo - Database
	- SES - Notification
	- S3 - Storage
	- CloudWatch Events - Schedule
	- CloudWatch Logs - Logging
		* - Lambda - Authorization
	
Functions
	- READ/CREATE/EDIT/DELETE Notes
	- NOTIFY Note changes
	- Move/Archive Note to Storage as JSON
		* - Authorizer
	
Events
	- API (GET/POST/PUT/DELETE)
	- Database Change
	- Timed Event
		* - Authorize API Calls
	
	
AWS Reference:
https://github.com/aws/aws-sdk-net
	- AWSSDK.DynamoDBv2
	- AWSSDK.SimpleEmail
	- AWSSDK.S3
		
Serverless Reference:
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