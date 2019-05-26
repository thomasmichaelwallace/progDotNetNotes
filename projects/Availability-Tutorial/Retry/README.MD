SHOWING A DB RETRY (USING BRIGHTER TO RETRY THE HANDLER)
--------------------------------------------------------
We want to lock the table, to show that a Db operation ought to time out.
Start the Product-API
We lock the Products table
	- Use the provided script to create a 10s lock (in Products-Core/Data)
	- We won't return for that 10s, as we are are waiting for the lock
	- (The reality is we don't want to retry for 10s, but this is too hard to demo at shorter timescales, so we use this value)
Use Fiddler and the Composer
	- Run the Add Product Query, show that we get a 429 because we timeout on the add product attempt
This locks a thread up whilst we are waiting.

--> Switch to After solution
Start the Product-API (use the cmd line as the exceptions thrown may get interrupted by VS for debugging, destroying timings)
We lock the Products table
	- Use the provided script to create a 10s lock (in Products-Core/Data)
	- We won't return for that 10s, as we are are waiting for the lock
Use Fiddler and the Composer
  - Run the Add Product Query, show that we get a 200 after a delay as we retry
This locks a thread up whilst we are waiting.


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


	
