FROM microsoft/azure-functions-dotnet-core2.1:2.1
ENV AzureWebJobsScriptRoot=/home/site/wwwroot
COPY . /home/site/wwwroot