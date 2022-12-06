FROM hub.nexdev.net/base/dotnet/core/sdk:5.0.102-ca-patch-buster-slim AS builder

RUN apt-get update  \
	&& curl --silent --location https://deb.nodesource.com/setup_14.x | bash -  \
	&& apt-get install -y nodejs

RUN node -v
RUN npm -v

WORKDIR /src/Fanex.Athena.PlayerAdmin.WebHost

COPY src/Fanex.Athena.PlayerAdmin.WebHost/package.json ./
COPY src/Fanex.Athena.PlayerAdmin.WebHost/package-lock.json ./

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
  --runtime alpine-x64 \
  --self-contained false

FROM hub.nexdev.net/base/dotnet/core/aspnet:5.0-buster-slim

ENV ASPNETCORE_URLS=http://*:5000
EXPOSE 5000

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

WORKDIR /app

COPY --from=builder /publish .

ENTRYPOINT ["dotnet", "Fanex.Athena.PlayerAdmin.WebHost.dll"]
