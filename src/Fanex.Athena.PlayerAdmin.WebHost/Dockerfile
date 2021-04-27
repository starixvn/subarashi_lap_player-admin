FROM node:10-alpine as frontend-builder

WORKDIR /frontend-out
COPY package.json ./
COPY package-lock.json ./
RUN npm install

#build front-end
COPY .babelrc ./
ADD /ClientApp/webpack ./ClientApp/webpack

#build react
ADD /ClientApp/src/js ./ClientApp/src/js
ADD /ClientApp/src/css ./ClientApp/src/css
RUN npm run build-react-pro

#build jquery
ADD /ClientApp/src/jquery-pages ./ClientApp/src/jquery-pages
RUN npm run build-pro

RUN rm -rf node_modules

FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runner
WORKDIR /app
COPY . .
COPY --from=frontend-builder /frontend-out  .
RUN rm -rf ClientApp

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

ENTRYPOINT ["dotnet", "Fanex.Athena.PlayerAdmin.WebHost.dll"]