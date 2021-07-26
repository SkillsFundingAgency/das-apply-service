# ![crest](https://assets.publishing.service.gov.uk/government/assets/crests/org_crest_27px-916806dcf065e7273830577de490d5c7c42f36ddec83e907efe62086785f24fb.png) Digital Apprenticeships Service

##  RoATP Apply Service
Licensed under the [MIT license](https://github.com/SkillsFundingAgency/das-apply-service/blob/master/LICENSE)

|               |               |
| ------------- | ------------- |
|![crest](https://assets.publishing.service.gov.uk/government/assets/crests/org_crest_27px-916806dcf065e7273830577de490d5c7c42f36ddec83e907efe62086785f24fb.png)|RoATP Apply Service|
| Info | A service which allows an 'approved' Training Provider (ATP) to register (on RoATP). |
| Build |![Build Status](https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/c39e0c0b-7aff-4606-b160-3566f3bbce23/831/badge) |
| Web | https://localhost:6016/ |

## Description

### Developer Setup

#### Requirements

- Install [.NET Core 2.2 SDK](https://www.microsoft.com/net/download)
- Install [SQL Server 2017 Developer Edition](https://go.microsoft.com/fwlink/?linkid=853016)
- Install [SQL Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms)
- Install [Azure Storage Emulator](https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409) (Make sure you are on v5.3)
- Install [Azure Storage Explorer](http://storageexplorer.com/)
- Install the editor of your choice:
  - [Jetbrains Rider](https://www.jetbrains.com/rider/)
  - [Visual Studio Code](https://code.visualstudio.com/)
  - [Visual Studio 2019](https://www.visualstudio.com/downloads/) with these workloads:
    - ASP.NET and web development

#### Setup

- Clone this repository
- Open Visual Studio as an administrator

##### Publish Database
- Build the solution SFA.DAS.ApplyService.sln
- Either use Visual Studio's `Publish Database` tool to publish the database project SFA.DAS.ApplyService.Database to name {{database name}} on {{local instance name}}

##### Config

- Grab the das-apply-service configuration json file from [das-employer-config](https://github.com/SkillsFundingAgency/das-employer-config/blob/master/das-apply-service/SFA.DAS.ApplyService.json)
- Create a Configuration table in your (Development) local Azure Storage account.
- Add a row to the Configuration table with fields: PartitionKey: LOCAL, RowKey: SFA.DAS.ApplyService_1.0, Data: {The contents of the local config json file}.
- Alter the SqlConnectionString value in the json to point to your database.

##### To run a local copy you will also require 

- [Login Service](https://github.com/SkillsFundingAgency/das-login-service)
- [QnA API](https://github.com/SkillsFundingAgency/das-qna-api)
- [RoATP Service](https://github.com/SkillsFundingAgency/das-roatp-service)

To use RoATP admin functionality; you will need to have the following projects running:

- [Admin Service](https://github.com/SkillsFundingAgency/das-admin-service)
- [RoATP Service](https://github.com/SkillsFundingAgency/das-roatp-service)
- [RoATP Gateway](https://github.com/SkillsFundingAgency/das-roatp-gateway)
- [RoATP Assessor](https://github.com/SkillsFundingAgency/das-roatp-assessor)
- [RoATP Oversight](https://github.com/SkillsFundingAgency/das-roatp-oversight)

#### Running the code

The default JSON configuration was created to work with dotnet run:

- Navigate to src/SFA.DAS.ApplyService.Web/
  - run `dotnet restore`
  - run `dotnet run`
  - Open https://localhost:6016

- Navigate to src/SFA.DAS.ApplyService.InternalApi/
  - run `dotnet restore`
  - run `dotnet run`

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
	
#### Work In Progress
Due to COVID-19 we have had to suspend work on this project. Current development on the following branch:
[Assessor Submaster](https://github.com/SkillsFundingAgency/das-apply-service/tree/Assessor_Submaster)

This will need to be merged back into `master` once tested
