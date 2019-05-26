SHOWING DECOUPLING (USING BRIGHTER TO HANDLE MESSAGING)
--------------------------------------------------------
We want to lock the table, to show that a timed out Db operation might keep failing despite retries, tying up caller threads.
Start the Product-API (use the cmd line as the exceptions thrown may get interrupted by VS for debugging, destroying timings)
We lock the Products table
	- Use the provided script to create a 2m lock (in Products-Core/Data)
	- We won't return for that 2m, as we are are waiting for the lock
Use Fiddler and the Composer
  - Run the Add Product Query, show that we get a 500 after a delay as we retry
  - Run the Add Product Query again, show that we still get a 429 after a delay as we circuit break
  - Repeat, running a set of new queries that will queue, and fail, until we finally succeed.
This protects the client by failing fast, but wouldn't it be nice if we could actually not lose that work, and instead process it instead once the server comes online?


-->Switch to after 
We use a Post instead of a Send so that the command is handled on the far side of a message queue instead.
Start the Product-API (use the cmd line as the exceptions thrown may get interrupted by VS for debugging, destroying timings)
We lock the Products table
	- Use the provided script to create a 5s lock (in Products-Core/Data)
	- We won't return for that 5s, as we are are waiting for the lock
  (Why so much shorter? We want to show a queue able to handle an outage for us, so we need the outage to dissappear in a time-frame which
  we can retry. What would happen if the queue took longer? We might reject the message, but would be in the store for replay. Show this too?)
Use Fiddler and the Composer
  - Run the Add Product Query, show that we get a 202 Accepted as we take in the work
  - Run the Add Product Query again, show that we still get a 202 as more items are added to the queue
  - Repeat, running a set of new queries that will queue, and fail.
  - Show the Rabbit queue building up at: http://localhost:55672/#/
  (N.B. You have to run the consumer to get the queue. The sender sends to the topic, but the consumer creates the queue.
Now run the Product-Service (use the script)
  - This will eat the queue (you can see this in RMQ, might be worth dialling up the refresh rate of the web page to 1s instead of 5s)
    

SHOWING COMPETING CONSUMERS
---------------------------
We want to show competing consumers. This is simple, we can just copy the bin\Debug folder for Store-Service to create a second instance.
Then, to show how this works, build up a queue, as follows

Use Fiddler and the Composer
  - Run the Add Product Query, show that we get a 202 Accepted as we take in the work
  - Run the Add Product Query again, show that we still get a 202 as more items are added to the queue
  - Repeat, running a set of new queries that will queue, and fail.
  - Show the Rabbit queue building up at: http://localhost:55672/#/
Now run two Product-Service instances (use a script)
  - This will eat the queue quickly
  
