# ZOA Info Tool
ZOA Info Tool is a desktop application to help the controllers of the [Oakland ARTCC on Vatsim](https://oakartcc.org/) quickly access status, routing and airspace information.

<img src="https://user-images.githubusercontent.com/34892440/210297905-652a97d7-ab4f-4788-8f7f-07e419f5ab4c.gif" width=800 />

The app is built using WinUI3 – the latest UI framework from Microsoft – and distributed as an unpackaged, self-contained executable + assemblies.

# Download and Installation
Download the latest Zip file containing the application: https://github.com/vzoa/info-tool/releases/download/v1.0.0/ZoaInfoTool.v1.0.0.zip

Unzip the folder anywhere you choose and run `ZoaInfo.exe` to start the program. The app is self-contained (i.e., includes all dependencies) does not need any installation or separate downloads.

**Requires Windows 10, October 2018 Update or newer**. Requires Windows 11, October 2021 Update for customized title bar.

# Features
* View real-world D-ATIS for ZOA airports (from https://datis.clowd.io/ API)
* Search for real-world IFR routes between two airports (from FlightAware)
* Embedded browser to quickly view routes on SkyVector
* Search LOAs between ZOA and neighboring ARTCCs for specific routing rules between 2 airports, or for general rules
* Search the ZOA "alias route" file for prescribed routing between intra-NCT and intra-ZOA airports
* Get links for all FAA charts for an airport, grouped by chart type
* Lookup information based on aircraft, airline or airport ICAO code
* Checks GitHub for new versions and prompts user to download if available

# Functionality Tips & Tricks
* You can use "numbered" hotkeys (`Ctrl+Alt+1`, `Ctrl+Alt+2`, `Ctrl+Alt+3`, etc.) to quickly change tabs
* For the routing pages, if you select a row and hit `c` on your keyboard, the route will be copied to your clipboard
* You can use the `Enter` key on your keyboard to "submit" the search / lookup forms on most pages instead of hitting the `Go` button
* ZOA Info Tool saves window size and position when you resize or move the window, and restores it when you start the program

# Todos
* Quickly show general LOA information
* Add quick links to SOPs
* View VATSIM D-ATIS for ZOA and neighboring ARTCCs

# Bugs?
Submit issues on GitHub (https://github.com/vzoa/info-tool/issues) or on the ZOA Discord.
