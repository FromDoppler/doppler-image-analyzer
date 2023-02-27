#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Doppler.Image.Analyzer.Api/Doppler.Image.Analyzer.Api.csproj", "Doppler.Image.Analyzer.Api/"]
RUN dotnet restore "Doppler.Image.Analyzer.Api/Doppler.Image.Analyzer.Api.csproj"
COPY . .
WORKDIR "/src/Doppler.Image.Analyzer.Api"
RUN dotnet build "Doppler.Image.Analyzer.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Doppler.Image.Analyzer.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ARG version=unknown
RUN echo $version > /app/wwwroot/version.txt
ENTRYPOINT ["dotnet", "Doppler.Image.Analyzer.Api.dll"]