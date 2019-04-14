# Keep'n Tabs Web API

The Keep'n Tabs Web API is the backend to the entire Keep'n Tabs experience.  It leverages the SimpleAPI class library, hooking API resource endpoints to methods that essentially forward user valid  requests to the MySql backing store, replying back with the results as simple XML payloads.

In order for the Keep'n Tabs Web API to work in the Windows VM, users must run the accompanying batch file as an administrator, which adds listening privileges for the server on the machine's IP address for all users.

Alternatively, as an administrator, users must discover their primary IP address by running `ipconfig` on the command line, and then run `netsh http add urlacl url=http://THE.IP.ADD.RESS:8080/todo/ user=EVERYONE`, where THE.IP.ADD.RESS is the machine's IP address.
