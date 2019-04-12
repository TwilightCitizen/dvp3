# SimpleAPI

SimpleAPI encapsulates the basic functionality of a simple web API.  Given a valid URL prefix to listen on, it can be started and stopped.  When running, it listens for requests on the API and notifies subscribers of the request.  Subscribers can process the request which is provided as a list of the request URLs segments—less the prefix segments—and pass a text reply back, completing the exchange.  If subscribers provide a text reply, the server processes asynchronously.

SimpleAPI is not a standalone application, but a class library.  A client application should reference the library, instantiating an instance of the server, likely wiring up some sort of controls to the server's start and stop methods, and register an event handler to properly handle incoming requests.

There are probably much better ways of doing this, but this is meant to be stupidly simple.
