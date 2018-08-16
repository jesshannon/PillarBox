# PillarBox
PillarBox is a mock SMTP server that makes email testing easy. Built for developers, designers and testers. It will capture all emails sent by your development or testing systems, and displays them in an easy to user UI with useful testing tools.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

- [Microsoft .NET Core, which is available for Windows, Linux and macOS](https://www.microsoft.com/net/learn/get-started-with-dotnet-tutorial)
- [NodeJS, for compiling Angular app](https://nodejs.org/en/download/)

### Installing

Open a shell or terminal window.

Checkout a copy of this repository.
```
git clone https://github.com/tshannon/PillarBox
```
Navigate into `/src/PillarBox.Web`
```
cd PillarBox/src/PillarBox.Web
```
Run with dotnet, if you see socket exception errors it may be neccessary to run from an Administrator prompt or with `sudo dotnet run`
```
dotnet run
```

On first run it might take a few minutes to restore packages and modules.

Once complete, go to http://localhost:5000/ in your browser.

You should see something like this:
![Powershell screenshot](/screenshots/running-ps.png?raw=true)

## Screenshots

![screenshot](/screenshots/screenshot02.png?raw=true)
![screenshot](/screenshots/screenshot01.png?raw=true)
![screenshot](/screenshots/screenshot03.png?raw=true)

## Development

If extending the Web API, then install [TypeScriptDefinitionGenerator](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.TypeScriptDefinitionGenerator) to keep JS classes in sync with server side code.

## Authors

* **Thom Shannon** - *Initial work* - [tshannon](https://github.com/tshannon)

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* [https://github.com/cosullivan/SmtpServer](https://github.com/cosullivan/SmtpServer)
