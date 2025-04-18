ARG CI_REGISTRY_IMAGE

FROM $CI_REGISTRY_IMAGE:builder as deps

RUN mkdir -p /src/packages

FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-nodejs-v20 AS builder

ENV NUGET_PACKAGES=/src/packages

COPY --from=deps $NUGET_PACKAGES/. $NUGET_PACKAGES/

WORKDIR /src/Fanex.Athena.PlayerAdmin.WebHost

COPY src/Fanex.Athena.PlayerAdmin.WebHost/package.json ./
COPY src/Fanex.Athena.PlayerAdmin.WebHost/package-lock.json ./

RUN node -v
RUN npm -v
RUN npm install

WORKDIR /src

COPY NuGet.Config .
RUN dotnet nuget add source http://nuget.nexdev.net/api/v2

COPY src/Fanex.Athena.PlayerAdmin.Core/Fanex.Athena.PlayerAdmin.Core.csproj ./Fanex.Athena.PlayerAdmin.Core/
RUN dotnet restore Fanex.Athena.PlayerAdmin.Core/Fanex.Athena.PlayerAdmin.Core.csproj -v n

COPY src/Fanex.Athena.PlayerAdmin.MVC/Fanex.Athena.PlayerAdmin.MVC.csproj ./Fanex.Athena.PlayerAdmin.MVC/
RUN dotnet restore Fanex.Athena.PlayerAdmin.MVC/Fanex.Athena.PlayerAdmin.MVC.csproj -v n

COPY src/Fanex.Athena.PlayerAdmin.API/Fanex.Athena.PlayerAdmin.API.csproj ./Fanex.Athena.PlayerAdmin.API/
RUN dotnet restore Fanex.Athena.PlayerAdmin.API/Fanex.Athena.PlayerAdmin.API.csproj -v n

COPY src/Modules/Fanex.Athena.PlayerAdmin.BatchUpdate/Fanex.Athena.PlayerAdmin.BatchUpdate.csproj ./Modules/Fanex.Athena.PlayerAdmin.BatchUpdate/
RUN dotnet restore Modules/Fanex.Athena.PlayerAdmin.BatchUpdate/Fanex.Athena.PlayerAdmin.BatchUpdate.csproj -v n

COPY src/Modules/Fanex.Athena.PlayerAdmin.BetLimit/Fanex.Athena.PlayerAdmin.BetLimit.csproj ./Modules/Fanex.Athena.PlayerAdmin.BetLimit/
RUN dotnet restore Modules/Fanex.Athena.PlayerAdmin.BetLimit/Fanex.Athena.PlayerAdmin.BetLimit.csproj -v n

COPY src/Modules/Fanex.Athena.PlayerAdmin.Commission/Fanex.Athena.PlayerAdmin.Commission.csproj ./Modules/Fanex.Athena.PlayerAdmin.Commission/
RUN dotnet restore Modules/Fanex.Athena.PlayerAdmin.Commission/Fanex.Athena.PlayerAdmin.Commission.csproj -v n

COPY src/Fanex.Athena.PlayerAdmin.WebHost/Fanex.Athena.PlayerAdmin.WebHost.csproj ./Fanex.Athena.PlayerAdmin.WebHost/
RUN dotnet restore Fanex.Athena.PlayerAdmin.WebHost/Fanex.Athena.PlayerAdmin.WebHost.csproj -v n

COPY src .

WORKDIR /src/Fanex.Athena.PlayerAdmin.WebHost

#build jquery
RUN npm run build-pro

#build react
RUN npm run build-react-pro

RUN dotnet publish "Fanex.Athena.PlayerAdmin.WebHost.csproj" -c Release -o /publish \
  --runtime linux-x64 \
  --self-contained false

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine

ENV ASPNETCORE_URLS=http://*:5000
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
EXPOSE 5000


WORKDIR /app

COPY --from=builder /publish .

ENTRYPOINT ["dotnet", "Fanex.Athena.PlayerAdmin.WebHost.dll"]
