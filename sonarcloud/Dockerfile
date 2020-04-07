
FROM mcr.microsoft.com/dotnet/framework/sdk

ENV DOTNET2_1_URL https://download.visualstudio.microsoft.com/download/pr/e571ccd9-4aaa-49be-bd94-da08b7b39dd4/45d4a2d7e585f98a00fec2c7daadff8b/dotnet-sdk-2.1.804-win-x64.zip
RUN $client = New-Object System.Net.WebClient; \
    $client.DownloadFile("$env:DOTNET2_1_URL", \"\dotnet-sdk-2.1.804-win-x64.zip\"); \
    Expand-Archive dotnet-sdk-2.1.804-win-x64.zip; \
    powershell cp -Path "c:/dotnet-sdk-2.1.804-win-x64/sdk/2.1.804" -Destination "c:/Program`` Files/dotnet/sdk/2.1.804" -Recurse; \
    powershell cp -Path "c:/dotnet-sdk-2.1.804-win-x64/shared/Microsoft.AspNetCore.All/2.1.16" -Destination "c:/Program`` Files/dotnet/shared/Microsoft.AspNetCore.All/2.1.16" -Recurse; \
    powershell cp -Path "c:/dotnet-sdk-2.1.804-win-x64/shared/Microsoft.NETCore.App/2.1.16" -Destination "c:/Program`` Files/dotnet/shared/Microsoft.NETCore.App/2.1.16" -Recurse; \
    powershell cp -Path "c:/dotnet-sdk-2.1.804-win-x64/shared/Microsoft.AspNetCore.App/2.1.16" -Destination "c:/Program`` Files/dotnet/shared/Microsoft.AspNetCore.App/2.1.16" -Recurse;

ENV DOTNET2_2_URL https://download.visualstudio.microsoft.com/download/pr/e0d4bd70-9dd2-40a3-9e6e-64af9721f3e3/2324e93d2152efd009f242a1723685c3/dotnet-sdk-2.2.207-win-x64.zip
RUN $client = New-Object System.Net.WebClient; \
    $client.DownloadFile("$env:DOTNET2_2_URL", \"\dotnet-sdk-2.2.207-win-x64.zip\"); \
    Expand-Archive dotnet-sdk-2.2.207-win-x64.zip; \
    powershell cp -Path "c:/dotnet-sdk-2.2.207-win-x64/sdk/2.2.207" -Destination "c:/Program`` Files/dotnet/sdk/2.2.207" -Recurse; \
    powershell cp -Path "c:/dotnet-sdk-2.2.207-win-x64/shared/Microsoft.AspNetCore.All/2.2.8" -Destination "c:/Program`` Files/dotnet/shared/Microsoft.AspNetCore.All/2.2.8" -Recurse; \
    powershell cp -Path "c:/dotnet-sdk-2.2.207-win-x64/shared/Microsoft.NETCore.App/2.2.8" -Destination "c:/Program`` Files/dotnet/shared/Microsoft.NETCore.App/2.2.8" -Recurse; \
    powershell cp -Path "c:/dotnet-sdk-2.2.207-win-x64/shared/Microsoft.AspNetCore.App/2.2.8" -Destination "c:/Program`` Files/dotnet/shared/Microsoft.AspNetCore.App/2.2.8" -Recurse;

# Download OpenJDK
ENV JAVA_URL https://download.java.net/java/GA/jdk13.0.2/d4173c853231432d94f001e99d882ca7/8/GPL/openjdk-13.0.2_windows-x64_bin.zip   
RUN $client = New-Object System.Net.WebClient; \
    $client.DownloadFile("$env:JAVA_URL", \"\openjdk-13.0.2_windows-x64_bin.zip\")

# Install Java
RUN Expand-Archive openjdk-13.0.2_windows-x64_bin.zip
RUN SetX /M PATH "\"\openjdk-13.0.2_windows-x64_bin\jdk-13.0.2\bin;$env:PATH\""
RUN SetX /M JAVA_HOME "\"\openjdk-13.0.2_windows-x64_bin\jdk-13.0.2\""

ENV SONARSCANNER_URL https://github.com/SonarSource/sonar-scanner-msbuild/releases/download/4.7.1.2311/sonar-scanner-msbuild-4.7.1.2311-net46.zip
RUN $client = New-Object System.Net.WebClient; \
    $client.DownloadFile("$env:SONARSCANNER_URL", \"\sonar-scanner-msbuild-4.7.1.2311-net46.zip\")

RUN Expand-Archive sonar-scanner-msbuild-4.7.1.2311-net46.zip
RUN SetX /M PATH "\"\sonar-scanner-msbuild-4.7.1.2311-net46;$env:PATH\""

# Copy SonarScanner settings to container
COPY SonarQube.Analysis.xml /sonar-scanner-msbuild-4.7.1.2311-net46/SonarQube.Analysis.xml