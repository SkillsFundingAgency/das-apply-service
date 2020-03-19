
SonarScanner.MSBuild.exe begin /k:"SkillsFundingAgency_das-apply-service" /o:"educationandskillsfundingagency" /d:sonar.exclusions="**/*.sql"
Nuget Restore c:\projects\das-apply-service\src\SFA.DAS.ApplyService.sln 
MSBuild.exe c:\projects\das-apply-service\src\SFA.DAS.ApplyService.sln /t:Rebuild
SonarScanner.MSBuild.exe end 