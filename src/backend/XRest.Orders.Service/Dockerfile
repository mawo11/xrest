FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine  as build
WORKDIR /app

COPY XRest.Authentication  /app/XRest.Authentication
COPY XRest.Consul /app/XRest.Consul
COPY XRest.Shared /app/XRest.Shared

COPY XRest.Orders.App /app/XRest.Orders.App
COPY XRest.Orders.Contracts /app/XRest.Orders.Contracts
COPY XRest.Orders.Infrastructure /app/XRest.Orders.Infrastructure
COPY XRest.Orders.Service /app/XRest.Orders.Service
COPY XRest.Orders.Contracts /app/XRest.Orders.Contracts
COPY XRest.Restaurants.Contracts /app/XRest.Restaurants.Contracts
COPY XRest.Identity.Contracts /app/XRest.Identity.Contracts

WORKDIR /app/XRest.Orders.Service
RUN dotnet restore
RUN dotnet publish -c RELEASE  -o /app/published-app


FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app /app
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false 
ENV TZ=Europe/Warsaw 
RUN apk add --no-cache icu-libs krb5-libs libgcc libintl libssl1.1 libstdc++ zlib tzdata
EXPOSE 80
ENTRYPOINT ["dotnet", "/app/XRest.Orders.Service.dll"]