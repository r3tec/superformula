## Policy API
### The target of this project is an API that allows to create and retrieve insurance policies.
The API comes with postman collecton. Exported collection is in the root folder of the project

## Project onboarding for API consumers
The API domain includes Policy and Vehicle classes. Definitions for both classes can be seen in the postman collection.
The API implements endoints to create and retrieve policies. Please follow the postman requests as they describe ways to consume the API.


# How to run the API
Please execute Ping request to initilaize the db before running any other requests.

## Possible enhancements
### State Regulations implementation
It is recommended to encapsulate state regulations check in a separate REST API. 
- The API will be deployed inside the corporate firewall
- The API will have an authorization by app key
- The API will expose a method that consumes a Policy object
- The API will return 200 on success is the policy passes state regulations test
- If policy does not pass the test the API will return 400 error code and a collection of strings each describing a particular regulations error
### Productionizing the API
- Add authorization middleware. The authorization method will depend on whether the API is exposed to clients outside of the corporate firewall
- Add monitoring i.e Application Insights for Azure
- Add diagnostic logging 
- For critical errors add email/text/etc. notifications to the level 1 support team
- Consider storing PII as encrypted strings. EF has provisions for encryption/decryption
- Discuss with product owners a necessity to add the following methods:
  - Update Policy
  - Deactivate Policy
  - Renew Policy
