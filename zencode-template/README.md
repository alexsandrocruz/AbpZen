# Running the DemoApp

## Prerequisites
- Docker
- Dotnet
- Yarn

## Running the application
TYE is configured for this demo app to run all the variations of demo app. You can execute the `run-all.ps1` file in this folder.

```bash
run-all.ps1
```

_If you're on MacOS you can try to execute this file with PowerShell_

```bash
pwsh run-all.ps1
```

If you want to setup all the required tools like TYE, MongoDB, ABP CLI, etc. you can execute the `run-all.ps1` file with `-setup` parameter.

```bash
run-all.ps1 -setup
```
_This command installs project tye if it's not installed and runs `abp install-libs` command and runs a mongdb container on docker._

## Running the Angular App
Navigate to the `angular` folder and run `yarn start` command.

_You can visit [localhost:4200](http://localhost:4200) to see the angular app._

## Tracking Running Apps
You can track the running apps on tye dashboard. You can visit [localhost:8000](http://localhost:8000) to see the dashboard.
