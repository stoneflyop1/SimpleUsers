# https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.alpine-x64

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["SimpleUsers.Core/SimpleUsers.Core.csproj","SimpleUsers.Core/"]
COPY ["SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj", "SimpleUsers.WebAPI/"]
RUN dotnet restore "SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj"

# copy everything else and build app
COPY . .
WORKDIR /src/SimpleUsers.WebAPI
RUN dotnet publish "SimpleUsers.WebAPI.csproj" -c Release -o /app
WORKDIR /app



FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine AS runtime
ARG CONN_STR
ENV ConnectionStrings:DefaultConnection=$CONN_STR
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
CMD ["dotnet", "SimpleUsers.WebAPI.dll"]