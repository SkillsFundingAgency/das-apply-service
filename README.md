# ![crest](https://assets.publishing.service.gov.uk/government/assets/crests/org_crest_27px-916806dcf065e7273830577de490d5c7c42f36ddec83e907efe62086785f24fb.png) Digital Apprenticeships Service

## RoATP Apply Service
<img src="https://avatars.githubusercontent.com/u/9841374?s=200&v=4" align="right" alt="UK Government logo">

[![Build Status](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_apis/build/status%2FEndpoint%20Assessment%20Organisation%2Fdas-apply-service?repoName=SkillsFundingAgency%2Fdas-apply-service&branchName=master)](https://dev.azure.com/sfa-gov-uk/Digital%20Apprenticeship%20Service/_build/latest?definitionId=1703&repoName=SkillsFundingAgency%2Fdas-apply-service&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=SkillsFundingAgency_das-apply-service&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=SkillsFundingAgency_das-apply-service)
[![License](https://img.shields.io/badge/license-MIT-lightgrey.svg?longCache=true&style=flat-square)](https://github.com/SkillsFundingAgency/das-apply-service/blob/master/LICENSE)

This repository contains the RoATP Apply Service front-end (SFA.DAS.ApplyService.Web) and the internal API (SFA.DAS.ApplyService.InternalApi). The project targets .NET 10 and uses Razor Pages for the public UI.

## About

RoATP Apply Service lets approved Training Providers register on the RoATP system. The web UI communicates with the internal API and a set of supporting services (QnA API, RoATP service, login service, etc.).

## Developer setup

These instructions are for developers who want to run and contribute to the service locally.

### Requirements

- .NET 10 SDK
- Visual Studio (recommended) or another IDE that supports .NET 10 (VS Code, Rider)
- SQL Server (2017+ recommended)
- SQL Server Management Studio (SSMS)
- Azure storage emulator or Azurite
- Azure Storage Explorer (optional)

### Clone and open

- Clone the repository.
- Open the solution in Visual Studio as an administrator.

### Configuration

We use a configuration row in Azure Table Storage for local development. Obtain the local JSON configuration from the das-employer-config repository and add it to your local Configuration table.

- Das Apply Service config: https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apply-service/SFA.DAS.ApplyService.json
- GovSignIn config: https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-shared-config/SFA.DAS.Apply.GovSignIn.json

Create a table named `Configuration` in your local storage emulator and add rows with:

- PartitionKey: LOCAL
- RowKey: SFA.DAS.ApplyService_1.0 (and SFA.DAS.Apply.GovSignIn_1.0 for GovSignIn)
- Data: {the appropriate JSON from das-employer-config}

Update the `SqlConnectionString` inside the JSON to point to your local database instance.

If you need to enable Sign Up with DfE SignIn locally, expose the internal API using ngrok and set `DfeSignIn.CallbackUri` in the GovSignIn configuration to the ngrok callback URL (https://.../Account/Callback).

### AppSettings.Development.json example

If you prefer file-based configuration for local debug, add `AppSettings.Development.json` to the web project with keys required for local development (example values):

```json
{
  "RoatpApply": {
	"RedisConnectionString": "",
	"DataProtectionKeysDatabase": "",
	"UseDfESignIn": false
  },
  "RoatpOuterApi": {
	"BaseUrl": "http://localhost:5335/",
	"SubscriptionKey": "Key",
	"PingUrl": "http://localhost:5335/"
  }
}
```

### Running locally

##### Publish Database
- Build the solution `SFA.DAS.ApplyService.sln`
- Either use Visual Studio's `Publish Database` tool to publish the database project `SFA.DAS.ApplyService.Database` to name {{database name}} on {{local instance name}}

- Web UI
  - cd `src/SFA.DAS.ApplyService.Web`
  - `dotnet restore`
  - `dotnet run`
  - Open https://localhost:6016

- Internal API
  - cd `src/SFA.DAS.ApplyService.InternalApi`
  - `dotnet restore`
  - `dotnet run`

You may also need the following services running locally for full functionality:

- Login Service: https://github.com/SkillsFundingAgency/das-login-service
- QnA API: https://github.com/SkillsFundingAgency/das-qna-api
- RoATP Service: https://github.com/SkillsFundingAgency/das-roatp-service

To use RoATP admin functionality; you will need to have the following projects running:

- [Admin Service](https://github.com/SkillsFundingAgency/das-admin-service)
- [RoATP Service](https://github.com/SkillsFundingAgency/das-roatp-service)
- [RoATP Gateway](https://github.com/SkillsFundingAgency/das-roatp-gateway)
- [RoATP Assessor](https://github.com/SkillsFundingAgency/das-roatp-assessor)
- [RoATP Oversight](https://github.com/SkillsFundingAgency/das-roatp-oversight)

#### Sign Up

If you need to use the Sign Up functionality, there's some additional setup required:

- Install [ngrok](https://ngrok.com/)
- Run `ngrok http 5999` (This will create a secure tunnel to your local Internal API that can be called from the outside world, specifically DfE SignIn)
- Edit your json config in Azure Storage and set the `DfeSignIn.CallbackUri` value to `{the secure url ngrok gave you}/Account/Callback`. For example, `https://c226e61e.ngrok.io/Account/Callback`.
- This will enable DfE SignIn to send a message back when Sign Up is complete with the DfE SignIn Id.

#### Other Useful Information
[Confluence](https://skillsfundingagency.atlassian.net/wiki/spaces/NDL/pages/304644526/Register+of+Apprenticeship+Training+Providers+RoATP)
[JIRA](https://skillsfundingagency.atlassian.net/secure/RapidBoard.jspa?projectKey=APR&rapidView=453)

Initial application process is as follows:
  - Create Sign In Account
  - Read & accept Conditions of Acceptance
  - Enter UKPRN to find company deals
  - Enter 'Preamble' information
  - Decide which type of Training Provider type you are

You will then be presented with a Task List with a list of sections containing questions, which must be answered appropriately.

Depending on your Training Provider type, some questions & sections may be `Not Required`

Depending on how you answered some questions, you may be presented with a `Shutter Page` which means you cannot proceed futher with your application.

Once you have filled out an application, you will then submit it for Assessment
	- Gateway Assessment - (i.e. legal checks)
	- Financial Assessment
	- Blind Assessor & Moderation
	- Oversight

## Technologies

- .NET 10
- Razor Pages
- NUnit, Moq for tests


