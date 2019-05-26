/*
The following example demonstrates using asynchronous methods to
get Domain Name System information for the specified host computer.

 * https://msdn.microsoft.com/en-us/library/ms228967(v=vs.110).aspx
 */

using System;
using System.Net;
using System.Net.Sockets;

namespace ApmDemo {
    public class Program {
        public static void Main(string[] args) {
            var hostname = "www.skillsmatter.com";
            // Make sure the caller supplied a host name.
            // Start the asynchronous request for DNS information.
            // This example does not use a delegate or user-supplied object
            // so the last two arguments are null.
            IAsyncResult result = Dns.BeginGetHostEntry(hostname, null, null);
            Console.WriteLine("Processing your request for information...");
            // Do any additional work that can be done here.
            try {
                // EndGetHostByName blocks until the process completes.
                IPHostEntry host = Dns.EndGetHostEntry(result);
                string[] aliases = host.Aliases;
                IPAddress[] addresses = host.AddressList;
                if (aliases.Length > 0) {
                    Console.WriteLine("Aliases");
                    for (int i = 0; i < aliases.Length; i++) {
                        Console.WriteLine("{0}", aliases[i]);
                    }
                }
                if (addresses.Length > 0) {
                    Console.WriteLine("Addresses");
                    for (int i = 0; i < addresses.Length; i++) {
                        Console.WriteLine("{0}", addresses[i].ToString());
                    }
                }
            } catch (SocketException e) {
                Console.WriteLine("An exception occurred while processing the request: {0}", e.Message);
            }
            Console.ReadKey(false);
        }
    }
}