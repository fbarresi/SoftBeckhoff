# SoftBeckhoff
![.NET Core](https://github.com/fbarresi/SoftBeckhoff/workflows/.NET%20Core/badge.svg)
![Docker Image CI](https://github.com/fbarresi/SoftBeckhoff/workflows/Docker%20Image%20CI/badge.svg)

This project was inspired by [SoftPlc](https://github.com/fbarresi/SoftPlc) and aims to reach the same goal: a virtual PLC (a Beckhoff in this case) for testing usage.

Beckhoff PLCs communicates with the ADS Protocol. Since there are no opensouce ADS Servers I wrote a simple one with reverse engineering.

Please note that this software is still under development.

## Features

- :heavy_check_mark: Multiplatform + Docker Support (Windows, Linux, Mac OS)
- :heavy_check_mark: Configurable with an Rest API (Swagger)
- :heavy_check_mark: Works and act like a real Beckhoff!
- :heavy_check_mark: All native types are supported
- :heavy_check_mark: Route management on-board and over Rest API

## Still under (heavy) development...

- 🌈 Support for custom compex types and arrays

## How to use it?

A good tool for testing and writing symbols is [TwincatAdsTool](https://github.com/fbarresi/TwincatAdsTool).
This software was event tested with it.

### run with docker

```docker
docker pull fbarresi/softbeckhoff:latest
docker run -p 8080:80 -p 852:852 -p 48898:48898 --name softBeckhoff fbarresi/softbeckhoff:latest
```

### Run it locally
`todo`
`basically with source code and dotnet run`

## Would you like to contribute? 
Yes, please!

- give a ⭐
- make a fork
- use and spread the software!

## Other interesting related projects

- [TwincatAdsTool](https://github.com/fbarresi/TwincatAdsTool) your swiss knife for twincat development.
- [BeckhoffHttpClient](https://github.com/fbarresi/BeckhoffHttpClient), an _unofficial_ TwinCAT function for HTTP requests
- [BeckhoffS7Client](https://github.com/fbarresi/BeckhoffS7Client), an _unofficial_ TwinCAT function for S7 communication

## Credits

Special thanks to [JetBrains](https://www.jetbrains.com/?from=SoftBeckhoff) for supporting this open source project.

<a href="https://www.jetbrains.com/?from=SoftBeckhoff"><img height="200" src="https://www.jetbrains.com/company/brand/img/jetbrains_logo.png"></a>

#### did you understand that you didn't understand?
Don't hesitate to write me or open an issue into this project!