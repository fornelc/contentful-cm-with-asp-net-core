# Using Contentful CMS with ASP.Net Core

In this project we make use of 'contentful.aspnetcore' package to provide the  Contentful integration within our .net core mvc website.

## Configuration

Use the appsetttings.json file to configure Contentful by using your space id and access token

```json

"ContentfulOptions": {
    "DeliveryApiKey": "<access_token>",
    "ManagementApiKey": "<cma_access_token>",
    "PreviewApiKey": "<preview_access_token>",
    "SpaceId": "<space_id>",
    "UsePreviewApi": false,
    "MaxNumberOfRateLimitRetries": 0
  }
```

## Register Services

It is needed to register the Contentful services that will use the configuration settings in your **Startup.cs** class. This is done by adding in the `ConfigureServices` method the following line. `services.AddContentful(Configuration);`
