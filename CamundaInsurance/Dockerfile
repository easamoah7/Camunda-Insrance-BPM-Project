#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["CamundaInsurance/CamundaInsurance.csproj", "CamundaInsurance/"]
RUN dotnet restore "CamundaInsurance/CamundaInsurance.csproj"
COPY . .
WORKDIR "/src/CamundaInsurance"
RUN dotnet build "CamundaInsurance.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CamundaInsurance.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CamundaInsurance.dll"]