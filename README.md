# UmbracoMastodonBot
A bot that boosts and favourites toots that use a specific hashtag

This bot uses the [Mastonet package](https://github.com/glacasa/mastonet)

## How to use
This code has been built to use Umbraco 13 and as part of the setup, it creates a custom database table. This could be changed to suit other needs though so isn't completely tied in to [Umbraco CMS](https://umbraco.com)

You will require a mastodon account and have the API keys, details can be found on the [Mastodon Docs](https://docs.joinmastodon.org/client/authorized/#client)

Clone down this project and add it to your own solution. 

![Image](.github/images/addToSolution.png)

You should then add a reference to this project from your own project `Project > Add > Add Project Reference` Tick the box that says MastodonBot.

## Startup.cs

Make sure you setup the `MastodonService` and the `StatusRecordRepository` in your `Startup.cs` file

```
public void ConfigureServices(IServiceCollection services)
{
	services.AddSingleton<IMastodonService, MastodonService>();
	services.AddSingleton<StatusRecordRepository>();
    ...
}
```

## AppSettings.json

You also need to add your API keys and access token to the `AppSettings.json` file

```
 "Mastodon": {
     "Api": {
         "Instance": "https://umbracocommunity.social/",
         "ClientId": "<<ClientId>>",
         "ClientSecret": "<<ClientSecret>>",
         "AccessToken": "<<AccessToken>>"
     }
 }
```

The `instance` is where your account is located, for this example, my account is on the umbracocommunity.social instance of Mastodon. Replace the `<<VALUE>>` with the keys from your account.

## Customisation

You'll probably want to call your database something different, you can do that via the Migrations/AddMastodonTable.cs file. Change `MastodonToots` to something you want to call it.

## API Endpoints

These endpoints require the user to be logged in to the backoffice - to test, log in to Umbraco and then hit the url in your browser.

The main endpoint is `umbraco/backoffice/boost/hashtag/{hashtag}`and you can change the hashtag to suit your needs, for example, if you want to favourite and boost toots with the hashtag umbraco, just change the parameter to be `/hashtag/umbraco`



There is also an Toot endpoint which allows you to toot from your account via the API. It can be found in the `MastodonController.cs` - `PostToot`


## Pull Requests welcome
If you can improve on this or add extra functionality, feel free to make a Pull Request. I'm happy for any contributions. 