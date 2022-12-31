# ZOA Info Tool
ZOA Info Tool is a desktop application to help the controllers of the [Oakland ARTCC on Vatsim](https://oakartcc.org/) quickly access status, routing and airspace information.

The app is built using WinUI3 – the latest UI framework from Microsoft – and distributed as an unpackaged, self-contained executable + assemblies. Find the latest release: https://github.com/vzoa/info-tool/releases 

# Features
* View real-world D-ATIS for ZOA airports (from https://datis.clowd.io/ API)
* Search for real-world IFR routes betwen two airports (from FlightAware)
* Embedded browser to quickly view routes on SkyVector
* Search LOAs betwen ZOA and neighboring ARTCCs for specific routing rules between 2 airports, or for general rules
* Search the ZOA "alias route" file for prescribed routing between intra-NCT and intra-ZOA airports
* Lookup information based on aircraft, airline or airport ICAO code

# Functionality Tips & Tricks
* You can use "numbered" hotkeys (`Ctrl+Alt+1`, `Ctrl+Alt+2`, `Ctrl+Alt+3`, etc.) to quickly change tabs
* For the routing pages, if you select a row and hit `c` on your keyboard, the route will be copied to your clipboard
* You can use the `Enter` key on your keyboard to "submit" the search / lookup forms on most pages instead of hitting the `Go` button
* ZOA Info Tool saves window size and position when you resize or move the window, and restores it when you start the program

# Todos
* View VATSIM D-ATIS for ZOA and neighboring ARTCCs

# Bugs?
Submit issues on GitHub: https://github.com/vzoa/info-tool/issues
