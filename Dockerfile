# https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.alpine-x64

FROM microsoft/dotnet:2.2-sdk-alpine3.8 AS build
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



FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine3.8 AS runtime
ARG CONN_STR
ENV ConnectionStrings:DefaultConnection=$CONN_STR
WORKDIR /app
EXPOSE 80
COPY --from=build /app .
CMD ["dotnet", "SimpleUsers.WebAPI.dll"]