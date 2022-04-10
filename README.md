# TibiaResults 

<img src="Documentation/Screenshot.png" width="220" height="250">

TibiaResults is a tool initially created to generate monthly summaries of my buddies' progress in the MMORPG [Tibia](https://www.tibia.com). Such a set of statistics allows us to compare our performance and motivate each other to work harder.

The program relies on data in the format returned by the [TibiaData API v3](https://tibiadata.com). I primarily used a simple Azure Logic App to retrieve and save such files every first day of the month, but nothing prevents you from obtaining this data in other ways or at different frequencies.

## Usage

The first step is to get the JSON files from the TibiaData API page. These should be saved locally on disk or in an Azure Blob container in the format `{category}/{yyyy-MM-dd}.json`. Then populate the [`Settings.json`](Source/TibiaResults/Settings.json) file with the appropriate values, e.g:

```
{
    "localPath": "C:/TibiaData"
    "characters": [
        "Tholdrim",
        "Khilleron",
        "Tuuro",
        "Silmare",
        "Kalliore",
        "Cieniaseq",
        "Talisis"
    ],
    "from": "2022-02-01",
    "to": "2022-03-01"
}
```

Finally, go to the `./Source/TibiaResults/` directory and run the program with the following command:

```
dotnet run
```

<sub>Note: To use the `dotnet` command, you must first install [.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) or later.</sub>

## Results

The result is served based on the configuration from the [`CategoryHelper.cs`](Source/TibiaResults/Helpers/CategoryHelper.cs) file. All characters that are present in the high score list for a given category will be listed with their position on the server, current score, and progress against the previous value. Special treatment is given to the Experience category, where the program tries to find an approximate number of experience points based on the other high score lists if the character has too low a level to be included in the official ranking.

If a file representing the current or previous high score list is missing for any particular category, the *Not available* message will be returned instead.

## Customization

The repository offers out of the box two ways to load saved data: from an Azure Blob container or from a local folder, and one way to format the results: as the [Discord](https://discord.com) message content. To add more features, simply implement the [`IHighscoreProvider`](Source/TibiaResults/Interfaces/IHighscoreProvider.cs) or [`IResultFormatter`](Source/TibiaResults/Interfaces/IResultFormatter.cs) interfaces.

## License

It is open-source software licensed under the MIT License. See the [LICENSE.txt](LICENSE.txt) file for more details.
