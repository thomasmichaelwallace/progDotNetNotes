SHOWING A DB TIMEOUT
--------------------
We want to lock the table, to show that a Db operation ought to time out.
We set the ProductsDAO ConnectionTimeout value in the ConnectionString to 0; this means wait indefinitely (we want to show this behaviour)
Use the provided .cmd files
Start the Product-API
Use Fiddler and the Composer
  - Run the Add Product Query, show that we get a 200 as we create the entity
  - Optionally, show the entity in the Db
We lock the Products table
	- Use the provided script to create a 1 minute lock (in Products-Core/Data)
	- We won't return for that 1 minutes, as we are are waiting for the lock
	- Note that we use a value of 0 to set 'infinite timeout'
	- Use the SQL Activity Monitor to show the Lock, on which we are waiting and the long running WAITFOR process
	- Show in Fiddler that we are waiting
	- Whilst we are waiting, discuss
		- Default SQL Timeout of 30s is too long, in fact the minimum timeout of 1s is too long for most use cases.
		- Also discuss the difference between how long we wait before timing out for a connection (connection string), and for a command to run (SqlCommand)
This locks a thread up whilst we are waiting.

--> Switch to After solution
Start the Product-API
We lock the Products table
	- Use the provided script to create a 1 minute lock (in Products-Core/Data)
	- We won't return for that 1 minute, as we are are waiting for the lock
	- Use the SQL Activity Monitor to show the Lock, on which we are waiting and the long running WAITFOR process
Use Fiddler and the Composer
  - Run the Add Product Query, show that we get a 429 because we timeout on the add product attempt
This locks a thread up whilst we are waiting.
	

SHOWING AN HTTP TIMEOUT
-----------------------
Demonstrates what happens when we don't have a timeout (or in this case a timeout so long we might as well not have one).
Use the provided .cmd files
Start the Product-API
	- The Feed service has been 'sabotaged' and will spin, never returning a response to the caller
Start Store-Service
	- The service has such a long timeout (essentially none) that we just sit there waiting for a response, consuming a resource, and not notifying that there is a fault which potentially causes an issue as we are not updating reference data so our system is becoming inconsistent
Note that Store-Service just hangs, we get no feedback

-->Switch to the After solution
Use the provided .cmd files
Start the Product-API
	- The Feed service has been 'sabotaged' and will spin, never returning a response to the caller
Start Store-Service
	- The service has a much lower value (the after solution contains this, but its trivial in this case)
Note that we now get feedback that it has failed
