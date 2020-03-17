# ![crest](https://assets.publishing.service.gov.uk/government/assets/crests/org_crest_27px-916806dcf065e7273830577de490d5c7c42f36ddec83e907efe62086785f24fb.png) Digital Apprenticeships Service

##  RoATP Apply Service

![Build Status](https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/c39e0c0b-7aff-4606-b160-3566f3bbce23/831/badge)

### Developer Setup

#### Requirements

- Install [.NET Core 2.1](https://www.microsoft.com/net/download)
- Install [Azure Storage Emulator](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) (Make sure you are on v5.3)
- Install [Azure Storage Explorer](http://storageexplorer.com/)
- Install the editor of your choice:
  - [Jetbrains Rider](https://www.jetbrains.com/rider/)
  - [Visual Studio Code](https://code.visualstudio.com/)
  - [Visual Studio](https://visualstudio.microsoft.com/)

#### Setup

- Clone this repository

##### Database
- Install the database by running the setup scripts in the SFA.DAS.ApplyService.Database project.

##### Code

- Grab the das-apply-service configuration json file from [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apply-service/SFA.DAS.ApplyService.json)
- Create a Configuration table in your (Development) local Azure Storage account.
- Add a row to the Configuration table with fields: PartitionKey: LOCAL, RowKey: SFA.DAS.ApplyService_1.0, Data: {The contents of the local config json file}.
- Alter the SqlConnectionString value in the json to point to your database.

### Running the code

- `dotnet run` the following projects:
  - SFA.DAS.ApplyService.InternalApi
  - SFA.DAS.ApplyService.Web
- Navigate to (https://localhost:6016) and you should see the start page.

#### Sign Up

If you need to use the Sign Up functionality, there's some additional setup required:

- Install [ngrok](https://ngrok.com/)
- Run `ngrok http 5999` (This will create a secure tunnel to your local Internal API that can be called from the outside world, specifically DfE SignIn)
- Edit your json config in Azure Storage and set the `DfeSignIn.CallbackUri` value to `{the secure url ngrok gave you}/Account/Callback`. For example, `https://c226e61e.ngrok.io/Account/Callback`.
- This will enable DfE SignIn to send a message back when Sign Up is complete with the DfE SignIn Id.

## SonarCloud Analysis

SonarCloud analysis can be performed using a docker container which can be built from the included dockerfile.

    Docker must be running Windows containers in this instance

An example of the docker run command to analyse the code base can be found below. 

For this docker container to be successfully created you will need:
* docker running Windows containers
* a user on SonarCloud.io with permission to run analysis
* a SonarQube.Analysis.xml file in the root of the git repository.

This file takes the format:

```xml
<SonarQubeAnalysisProperties  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns="http://www.sonarsource.com/msbuild/integration/2015/1">
<Property Name="sonar.host.url">https://sonarcloud.io</Property>
<Property Name="sonar.login">[Your SonarCloud user token]</Property>
</SonarQubeAnalysisProperties>
```     

### Example:

_docker run [OPTIONS] IMAGE COMMAND_

[Docker run documentation](https://docs.docker.com/engine/reference/commandline/run/)

```docker run --rm -v c:/projects/das-apply-service:c:/projects/das-apply-service -w c:/projects/das-apply-service 3d9151a444b2 powershell -F c:/projects/das-apply-service/sonarcloud/analyse.ps1```

#### Options:

|Option|Description|
|---|---|
|--rm| Remove any existing containers for this image
|-v| Bind the current directory of the host to the given directory in the container ($PWD may be different on your platform). This should be the folder where the code to be analysed is
|-w| Set the working directory

#### Command:

Execute the analyse.ps1 PowerShell script	  
