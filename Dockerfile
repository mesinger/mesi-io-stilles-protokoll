FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "/app/silent-protocol.dll", "--urls", "http://0.0.0.0:5000"]
