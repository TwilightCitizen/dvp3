# SimpleAPI

SimpleAPI encapsulates the basic functionality of a simple web API.  Given a valid URL prefix to listen on, it can be started and stopped.  When running, it listens for requests on the API and notifies subscribers of the request.  Subscribers can process the request which is provided as a list of the request URLs segments—less the prefix segments—and pass a text reply back, completing the exchange.  If subscribers provide a text reply, the server processes asynchronously.
