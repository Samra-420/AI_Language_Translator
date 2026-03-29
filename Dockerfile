FROM mcr.microsoft.com/dotnet/sdk:7.0
WORKDIR /app
COPY . .
RUN dotnet restore
RUN dotnet build -c Release
CMD ["dotnet", "run", "--project", "AI_Language_Translator.csproj"]