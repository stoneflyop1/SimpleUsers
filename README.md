# 一个简单的用户系统

以WebAPI方式实现了一个简单的用户系统，包括用户的注册、登录、获取或更新用户信息等功能。

- 以用户系统为例的增改查操作(EF，数据库使用mysql，采用docker镜像，见[sql镜像](sql/))
- API文档生成使用Swagger

关于.NET core的详细资料，建议看看[.NET Core官方文档](https://docs.microsoft.com/en-us/dotnet/core/), 以及[Introduction to ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2)。

[MIT授权](LICENSE)

## VS Code准备

此项目完全使用vs code开发。

安装如下扩展(一般打开过C#文件的话，前两个扩展应该安装好了)

- C#
- C# Extensions
- C# FixFormat
- C# XML Documentation Comments
- Rest Client [使用RestClient提交API请求](test.http)

## 创建步骤

注意：

- dotnet build貌似不会拷贝appSettings.json等配置文件到生成目录，需要用dotnet publish

如何使用dotnet cli可以参考[官方文档](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet?tabs=netcore2x)

### 创建项目文件

1. 创建核心类库
    ```sh
    dotnet new classlib -f netstandard2.0 -o SimpleUsers.Core
    ```
2. 创建webapi项目
    ```sh
    dotnet new webapi -o SimpleUsers.WebAPI
    ```
3. 添加引用关系(WebAPI引用Core)
    ```sh
    dotnet add SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj reference SimpleUsers.Core/SimpleUsers.Core.csproj
    ```
4. 在项目文件中添加如下代码(以SimpleUsers.WebAPI项目为例)，为项目启用XML的注释功能(方便Swagger制作API文档)
    ```xml
    <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
        <DocumentationFile>bin\Debug\netcoreapp2.2\SimpleUsers.WebAPI.xml</DocumentationFile>
    </PropertyGroup>
    <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
        <DocumentationFile>bin\Release\netcoreapp2.2\SimpleUsers.WebAPI.xml</DocumentationFile>
    </PropertyGroup>
    ```
5. 为VS添加sln文件(可选)
    ```sh
    dotnet new sln
    dotnet sln SimpleUsers.sln add SimpleUsers.Core/SimpleUsers.Core.csproj SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj
    ```

### 为Core项目添加引用

1. 添加EF Core
    ```sh
    dotnet add SimpleUsers.Core/SimpleUsers.Core.csproj package Microsoft.EntityFrameworkCore
    ```
1. 添加dynamic支持
    ```sh
    dotnet add SimpleUsers.Core/SimpleUsers.Core.csproj package Microsoft.CSharp
    ```
1. 添加日志支持
    ```sh
    dotnet add SimpleUsers.Core/SimpleUsers.Core.csproj package Microsoft.Extensions.Logging.Abstractions
    ```

### 为WebAPI项目添加引用

1. 添加EF Core & Sqlite
    ```sh
    dotnet add SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj package Microsoft.EntityFrameworkCore.Sqlite
    ```
1. 添加[Swagger.net](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-2.2&tabs=visual-studio%2Cvisual-studio-xml)以便自动生成API文档
    ```sh
    dotnet add SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj package Swashbuckle.AspNetCore
    ```
1. 添加[NLog日志](https://github.com/NLog/NLog.Web/wiki/Getting-started-with-ASP.NET-Core-2)支持
    ```sh
    dotnet add SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj package NLog.Web.AspNetCore
    ```
2. 若EF版本与Core不一致，可以添加EF
    ```sh
    dotnet add SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj package Microsoft.EntityFrameworkCore
    ```

## .net core

.net core 相比.net framework更加模块化。

- 自带依赖注入(IServiceCollection)
- 全新的程序配置方式(IConfiguration)，支持多种文件格式配置以及环境变量配置等
- 自带日志功能(ILoggerFactory)

Startup类：

```csharp
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // 程序的配置
        public IConfiguration Configuration { get; }

        // 引入(Add)需要的服务(EF,MVC,Identity,Authentication etc)，包括数据库上下文、依赖注入等
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            ...
            services.AddMvc();
            ...
        }
        // 启动(Use)需要的服务，如：(EF,MVC,Identity,Authentication, Logging etc)
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ...
            app.UseMvc();
            ...
        }

    }
```

## 代码规范和最佳实践检查

可以使用[Microsoft.CodeAnalysis.FxCopAnalyzers](https://github.com/dotnet/roslyn-analyzers/wiki)来做初期的代码审查。

## 单元测试

- [dotnetcore中使用xUnit进行单元测试](https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-with-dotnet-test)
- [使用InMemory的EF进行单元测试](https://garywoodfine.com/entity-framework-core-memory-testing-database/)

步骤：

1. 创建单元测试文件夹

    ```sh
    mkdir SimpleUsers.Tests
    ```
2. 添加单元测试项目以及引用Core类库

    ```sh
    cd SimpleUsers.Tests
    dotnet new xunit
    dotnet add reference ../SimpleUsers.Core/SimpleUsers.Core.csproj
    ```
3. 添加单元测试类
4. 执行单元测试

    ```sh
    cd ..
    dotnet test
    ```

## docker支持

[Dockerfile](Dockerfile)为生成docker镜像的脚本文件，一些常用的.net的Dockerfile可以参考[微软官方的示例](https://github.com/dotnet/dotnet-docker/)。

最简单的使用方式是：`docker-compose`

```sh
docker-compose build
docker-compose up
...
docker-compose down
```

1. 通过带有SDK的基础镜像(`Build`)还原nuget包、编译、发布DLL
2. 把`Build`镜像中发布的DLL拷贝到只有运行时的镜像(`Runtime`)，设置docker的入口脚本
3. 通过生成脚本生成和执行docker（[Windows](build.ps1)以及[Linux](build.sh)）

注：[删除不再使用的镜像](https://stackoverflow.com/questions/40084044/how-to-remove-docker-images-based-on-name)

```ps1
docker rmi $(docker images --format "{{.Repository}}:{{.Tag}}"|findstr "dotnetusers")
```

## Linux部署

详细指南可参考[微软官方文档](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/linux-nginx?tabs=aspnetcore2x&view=aspnetcore-2.1)。

以Ubuntu为例：

### 准备Linux环境

Linux环境的依赖安装可以参考[微软官方文档](https://docs.microsoft.com/en-us/dotnet/core/linux-prerequisites?tabs=netcore2x)。

依赖库可以通过各个Linux发行版的包管理命令行查看，若没有安装，需要先安装依赖库。比如libcurl：

```sh
dpkg -l | grep libcurl
# if not exist, search package by name
apt-cache search libcurl | grep ^libcurl
sudo apt-get install libcurl4
```

[安装dotnet core runtime/sdk](https://dotnet.microsoft.com/download/linux-package-manager/ubuntu18-04/sdk-current)

1. 注册微软的Key和Feed

    ```sh
    wget -q https://packages.microsoft.com/config/ubuntu/18.04/packages-microsoft-prod.deb
    sudo dpkg -i packages-microsoft-prod.deb
    ```
2. 安装.net sdk

    ```
    sudo add-apt-repository universe
    sudo apt-get install apt-transport-https
    sudo apt-get update
    sudo apt-get install dotnet-sdk-2.2
    ```



### 发布和执行dotnet core程序

1. dotnet core程序发布

    ```sh
    dotnet publish -c Release # bin/Release/netcoreapp2.1/publish
    ```
2. 拷贝发布好的程序到需要部署的文件夹

    ```
    cd bin/Release/netcoreapp2.2/publish
    mkdir ~/dotnetapps/SimpleUsers && cp -r . ~/dotnetapps/SimpleUsers
    ```
3. 执行dotnet core程序

    ```sh
    dotnet SimpleUsers.WebAPI.dll
    ```

### 使用systemd制作系统服务

#### 1.安装nginx服务器

```sh
sudo apt-get install nginx
```

#### 2.准备系统服务

service(SimpleUsers.servie)的定义文件如下：

```ini
[Unit]
Description=SimpleUsers WebAPI running on Ubuntu

[Service]
WorkingDirectory=/home/test/dotnetapps/SimpleUsers
# you can exec `which dotnet` to get the dotnet exe path
ExecStart=/usr/bin/dotnet /home/test/dotnetapps/SimpleUsers/SimpleUsers.WebAPI.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=SimpleUsers-WebAPI
# must be in nginx worker-process usergroup
User=test
# add some Environment Variables
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

启用服务

```sh
sudo cp SimpleUsers.service /etc/systemd/system/
sudo systemctl enable SimpleUsers.servie # 启用
sudo systemctl start SimpleUsers.servie # 启动
sudo systemctl status SimpleUsers.servie # 查看状态
sudo journalctl -fu SimpleUsers.servie --since "2018-9-23" --until "2018-10-18 04:00" # 查看日志
```

注：为了更新部署的程序方便，我们把dotnet应用程序放到了当前用户的目录下，而且程序执行也以当前用户的身份执行。但为了使用nginx反向代理，我们需要把当前用户加入到nginx服务的用户组里：

```sh
sudo usermod -a -G www-data $(whoami)
```

#### 3.使用nginx反向代理

nginx配置文件(`/etc/nginx/sites-available/default`)

```nginx
server {
    listen        80 default_server;
    server_name   _; # example.com *.example.com;
    location / {
        proxy_pass         http://localhost:5000;
        proxy_http_version 1.1;
        proxy_set_header   Upgrade $http_upgrade;
        proxy_set_header   Connection keep-alive;
        proxy_set_header   Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header   X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header   X-Forwarded-Proto $scheme;
    }
}
```

```sh
# after reconfig, restart nginx
sudo service nginx restart
```

## 后续

- [EF数据库迁移(DbMigration)](https://msdn.microsoft.com/en-us/data/jj591621.aspx#initializer)
- 使用Autofac依赖注入，可以参考我在读一本书时的[示例](https://github.com/stoneflyop1/MuscleFellow)
- [使用razor pages制作页面](https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/razor-pages-start?view=aspnetcore-2.1)
- [使用IdentityServer4实现OAuth及第三方登录](https://github.com/IdentityServer/IdentityServer4)
- 微服务化，[微软的官方示例指南](https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/)，[一个开源的微服务框架](https://github.com/ThreeMammals/Ocelot)，[gRPC中使用C#](https://grpc.io/docs/quickstart/csharp.html)
