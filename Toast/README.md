#Toast!
A small web application I've written to send and receive notifications using SignalR and windows notification API.

1. Instructions-
2. Build the application locally.
3. Navigate to .vs/config/application.config
4. (Optional) Find the single line matching "binding protocol="http" bindingInformation="*:54842:localhost"" within the configuration/system.applicationHost/sites/site/bindings node.
5. (Optional) Add a new node with this content "binding protocol="http" bindingInformation="*:54842:network-ip-for-your-computer""
6. Run the application.

- Note : Steps 4 and 5 allow other people on the network to also connect to your site, and send and receive toast notifications.