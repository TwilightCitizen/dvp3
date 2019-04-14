@echo off

rem Line Label in IPConfig for IP Address

set label="IPv4 Address"

rem Get the IP Address

for /f "usebackq tokens=2 delims=:" %%f in (`ipconfig ^| findstr /c:%label%`) do set address=%%f

rem Get Rid of the Leading Space

set trimmed=%address:~1%

rem Grant Listen Priviledges for Everyone on that IP Address with Port and Prefix:

netsh http add urlacl url=http://%trimmed%:8080/todo/ user=EVERYONE