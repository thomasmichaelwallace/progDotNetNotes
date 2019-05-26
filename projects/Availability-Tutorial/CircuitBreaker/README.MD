SHOWING A DB RETRY (USING BRIGHTER TO RETRY THE HANDLER)
--------------------------------------------------------
We want to lock the table, to show that a timed out Db operation might keep failing despite retries, tying up caller threads.
Start the Product-API (use the cmd line as the exceptions thrown may get interrupted by VS for debugging, destroying timings)
We lock the Products table
	- Use the provided script to create a 2m lock (in Products-Core/Data)
	- We won't return for that 2m, as we are are waiting for the lock
Use Fiddler and the Composer
  - Run the Add Product Query, show that we get a 500 after a delay as we retry
  - Run the Add Product Query again, show that we still get a 500 after a delay as we retry
  - Repeat, running a set of new queries that will queue, and fail, until we finally succeed.
  Note how we tied up resources doing retries on a service that was not going to respond
  
-->Switch to after solution
We add a circuit breaker attribute via BrighterCommand's UsePolicy statement
-- We catch the Broken Circuit Exception in the controller, and use it to return a Too Many Requests and Retry-After hint
-- This lets the client 'back-off' for a period of time
We lock the Products table
	- Use the provided script to create a 5m lock (in Products-Core/Data)
	- We won't return for that 5m, as we are are waiting for the lock
	- We use 5m here and not 2m as we want long enough to see the results of failing retries
Use Fiddler and the Composer
  - Run the Add Product Query, show that we get a 500 after a delay as we retry
  - Run the Add Product Query again, show that we still get a 500 after a delay as we retry
  - Repeat, running a set of new queries that will queue, and fail, until we finally succeed.
  Note how we tied up resources doing retries on a service that was not going to respond




SHOWING AN HTTP RETRY (USING POLLY TO RETRY THE CALL)
-----------------------------------------------------
Use the provided .cmd files
Start the Product-API
	- The Feed service has been 'sabotaged' and will spin, but only until it hits a count
Start Store-Service
	- The service has a low timeout value
    - When we time out we throw an exception and die
Note that we now get feedback that it has failed

-->Switch to after solution
Start the Product-API
	- The Feed service has been 'sabotaged' and will spin, but only until it hits a count
Start Store-Service
	- The service has a low timeout value
    - When we time out we throw an exception
	- NOW Polly retries for us
	- And we succeed instead of failing


	
