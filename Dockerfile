# https://github.com/dotnet/dotnet-docker/blob/master/samples/aspnetapp/Dockerfile.alpine-x64

FROM microsoft/dotnet:2.1-sdk-alpine AS build
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.sln .
COPY SimpleUsers.Core/*.csproj ./SimpleUsers.Core/
COPY SimpleUsers.WebAPI/*.csproj ./SimpleUsers.WebAPI/
COPY SimpleUsers.Tests/*.csproj ./SimpleUsers.Tests/
RUN dotnet restore

# copy everything else and build app
COPY SimpleUsers.Core/. ./SimpleUsers.Core/
COPY SimpleUsers.WebAPI/. ./SimpleUsers.WebAPI/
COPY SimpleUsers.Tests/. ./SimpleUsers.Tests/
WORKDIR /app/SimpleUsers.WebAPI
RUN dotnet publish -c Release -o out


FROM microsoft/dotnet:2.1-aspnetcore-runtime-alpine AS runtime
WORKDIR /app
COPY --from=build /app/SimpleUsers.WebAPI/out ./
ENTRYPOINT ["dotnet", "SimpleUsers.WebAPI.dll"]