#Toast!

1. Build the application locally.
2. Navigate to .vs/config/application.config
3. (Optional) Find the single line matching "binding protocol="http" bindingInformation="*:54842:localhost"" within the configuration/system.applicationHost/sites/site/bindings node.
4. (Optional) Add a new node with this content "binding protocol="http" bindingInformation="*:54842:network-ip-for-your-computer""
5. Run the application.

- Note : Steps 4 and 5 allow other people on the network to also connect to your site, and send and receive toast notifications.