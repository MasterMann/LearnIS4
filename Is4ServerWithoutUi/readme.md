# IdentityServer4 Authentication (Without UI)

### Description:
- This repository is for showing how to sign user into system using [IdentityServer4](http://docs.identityserver.io/en/latest/index.html).
- It uses some code from [IdentityServer4.Quickstart.UI](https://github.com/IdentityServer/IdentityServer4.Quickstart.UI) to allow user login by using their `username` and `password` or their `Google` account.

### How to use `Google authentication`
- Start `Is4ServerWithoutUi` project.
- Type the url [http://localhost:57548/api/login?Provider=Google&ReturnUrl=%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3Dmvc%26redirect_uri%3Dhttp%253A%252F%252Flocalhost%253A4300%26scope%3Dapi1%2520offline_access%2520openid%26response_type%3Dcode%2520token%26response_mode%3Dfragment%26nonce%3D53lyhj2iqlr](http://localhost:57548/api/login?Provider=Google&ReturnUrl=%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3Dmvc%26redirect_uri%3Dhttp%253A%252F%252Flocalhost%253A4300%26scope%3Dapi1%2520offline_access%2520openid%26response_type%3Dcode%2520token%26response_mode%3Dfragment%26nonce%3D53lyhj2iqlr) in browser.
    - **http://localhost:57548**: Project base url.
    - **Provider**: Provider name which has been registered as `services.AddGoogle("Google", options => {...})`
    - **ReturnUrl**: Url that user will be redirected to after make `Google` allow application to use public profile.
        - The url will be constructed as: **/connect/authorize/callback?client_id=mvc&redirect_uri=http%3A%2F%2Flocalhost%3A4300&scope=api1%20offline_access%20openid&response_type=code%20token&response_mode=fragment&nonce=53lyhj2iqlr**
            - [/connect/authorize/callback](/connect/authorize/callback): The callback url which defined by [IdentityServer4](http://docs.identityserver.io/en/latest/index.html) originally.
            - **client_id**: Id of client that has been registered in `LoadClients` method.
            - **redirect_uri**: Url that user will be redirected to after successfully get **access_token**, **id_token**, **token** from `Google`. This must be registered in `RedirectUris` property.
            - **scope**: Scopes that user wants to access in the application.
            - **response_type**: Please refer to [this document](https://stackoverflow.com/questions/51403066/grant-type-vs-response-type-in-oauth2-0-oidc)
            - **response_mode**: Please refer to [this document](https://ldapwiki.com/wiki/Response_mode).
            - **nonce**: Please refer [this document](https://auth0.com/docs/api-auth/tutorials/nonce).
            To build this **ReturnUrl**, you can use [this tool](https://oidcdebugger.com/).


- Allow application to access to `Google` public profile.
- Finally, user will be redirected to `RedirectedUrl`.

