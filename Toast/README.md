#Toast!
A small web application I've written to send and receive notifications using SignalR and windows notification API.

Instructions-
Build the application locally.
Navigate to .vs/config/application.config
Find the single line matching "<binding protocol="http" bindingInformation="*:54842:localhost" />" within the configuration/system.applicationHost/sites/site/bindings node.
Add a new node "<binding protocol="http" bindingInformation="*:54842:<network-ip-for-your-computer>" />"
Run the application.

Other people on the network can also connect to the site and send and receive toast notifications.