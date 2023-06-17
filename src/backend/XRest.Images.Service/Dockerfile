FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine  as build
WORKDIR /app

COPY XRest.Authentication  /app/XRest.Authentication
COPY XRest.Consul /app/XRest.Consul
COPY XRest.Shared /app/XRest.Shared

COPY XRest.Images.App /app/XRest.Images.App
COPY XRest.Images.Infrastructure /app/XRest.Images.Infrastructure
COPY XRest.Images.Service /app/XRest.Images.Service

WORKDIR /app/XRest.Images.Service
RUN dotnet restore
RUN dotnet publish -c RELEASE  -o /app/published-app

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false 
ENV TZ=Europe/Warsaw 
RUN apk add --no-cache icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib tzdata
EXPOSE 80
ENTRYPOINT ["dotnet", "/app/XRest.Images.Service.dll"]