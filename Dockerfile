FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-nodejs-v20 AS builder

WORKDIR /src

RUN dotnet nuget add source http://nuget.nexdev.net/api/v2

COPY src/Fanex.Athena.PlayerAdmin.Core/Fanex.Athena.PlayerAdmin.Core.csproj ./Fanex.Athena.PlayerAdmin.Core/
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore Fanex.Athena.PlayerAdmin.Core/Fanex.Athena.PlayerAdmin.Core.csproj -v n

COPY src/Fanex.Athena.PlayerAdmin.MVC/Fanex.Athena.PlayerAdmin.MVC.csproj ./Fanex.Athena.PlayerAdmin.MVC/
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore Fanex.Athena.PlayerAdmin.MVC/Fanex.Athena.PlayerAdmin.MVC.csproj -v n

COPY src/Fanex.Athena.PlayerAdmin.API/Fanex.Athena.PlayerAdmin.API.csproj ./Fanex.Athena.PlayerAdmin.API/
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore Fanex.Athena.PlayerAdmin.API/Fanex.Athena.PlayerAdmin.API.csproj -v n

COPY src/Modules/Fanex.Athena.PlayerAdmin.BatchUpdate/Fanex.Athena.PlayerAdmin.BatchUpdate.csproj ./Modules/Fanex.Athena.PlayerAdmin.BatchUpdate/
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore Modules/Fanex.Athena.PlayerAdmin.BatchUpdate/Fanex.Athena.PlayerAdmin.BatchUpdate.csproj -v n

COPY src/Modules/Fanex.Athena.PlayerAdmin.BetLimit/Fanex.Athena.PlayerAdmin.BetLimit.csproj ./Modules/Fanex.Athena.PlayerAdmin.BetLimit/
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore Modules/Fanex.Athena.PlayerAdmin.BetLimit/Fanex.Athena.PlayerAdmin.BetLimit.csproj -v n

COPY src/Modules/Fanex.Athena.PlayerAdmin.Commission/Fanex.Athena.PlayerAdmin.Commission.csproj ./Modules/Fanex.Athena.PlayerAdmin.Commission/
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore Modules/Fanex.Athena.PlayerAdmin.Commission/Fanex.Athena.PlayerAdmin.Commission.csproj -v n

COPY src/Fanex.Athena.PlayerAdmin.WebHost/Fanex.Athena.PlayerAdmin.WebHost.csproj ./Fanex.Athena.PlayerAdmin.WebHost/
RUN --mount=type=cache,target=/root/.nuget/packages \
  dotnet restore Fanex.Athena.PlayerAdmin.WebHost/Fanex.Athena.PlayerAdmin.WebHost.csproj -v n

COPY src .

WORKDIR /src/Fanex.Athena.PlayerAdmin.WebHost

COPY src/Fanex.Athena.PlayerAdmin.WebHost/package.json ./
COPY src/Fanex.Athena.PlayerAdmin.WebHost/package-lock.json ./

RUN node -v
RUN npm -v
RUN npm install

#build jquery
RUN npm run build-pro

#build react
RUN npm run build-react-pro

RUN dotnet publish "Fanex.Athena.PlayerAdmin.WebHost.csproj" -c Release -o /publish \
  --runtime linux-x64 \
  --self-contained false

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine-25h2 AS final

ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
ENV ASPNETCORE_HTTP_PORTS=5000
EXPOSE 5000

# Dont run as root
RUN adduser --disabled-password \
  --home /app \
  --gecos '' dotnetuser && chown -R dotnetuser /app && chown -R dotnetuser /tmp
USER dotnetuser
#

WORKDIR /app

COPY --from=builder /publish .
ENV TZ="America/La_Paz"

ENTRYPOINT ["dotnet", "Fanex.Athena.PlayerAdmin.WebHost.dll"]
