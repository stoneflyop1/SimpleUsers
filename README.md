# 一个简单的用户系统

以WebAPI方式实现了一个简单的用户系统，包括用户的注册、登录、获取或更新用户信息等功能。

- 以用户系统为例的增改查操作(EF，数据库使用Sqlite)
- API文档生成使用Swagger

关于.NET core的详细资料，建议看看[.NET Core官方文档](https://docs.microsoft.com/en-us/dotnet/core/), 以及[Introduction to ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.1)。

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

- 微软已发布dotnetcore 2.1，需要从2.0升级到2.1版，因为2.0并不是长期支持版本。
- dotnet build貌似不会拷贝appSettings.json等配置文件到生成目录，需要用dotnet publish

如何使用dotnet cli可以参考[官方文档](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet?tabs=netcore2x)

### 创建项目文件

1. 创建核心类库
    ```sh
    dotnet new classlib -f netstandard2.0 -o SimpleUsers.Core
    ```
1. 创建webapi项目
    ```sh
    dotnet new webapi -o SimpleUsers.WebAPI
    ```
1. 添加引用关系(WebAPI引用Core)
    ```sh
    dotnet add SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj reference SimpleUsers.Core/SimpleUsers.Core.csproj
    ```
1. 在项目文件中添加如下代码(以SimpleUsers.WebAPI项目为例)，为项目启用XML的注释功能(方便Swagger制作API文档)
    ```xml
    <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Debug|AnyCPU'">
        <DocumentationFile>bin\Debug\netcoreapp2.1\SimpleUsers.WebAPI.xml</DocumentationFile>
    </PropertyGroup>
    <PropertyGroup Condition="'$(Configuration)|$(Platform)'=='Release|AnyCPU'">
        <DocumentationFile>bin\Release\netcoreapp2.1\SimpleUsers.WebAPI.xml</DocumentationFile>
    </PropertyGroup>
    ```
1. 为VS添加sln文件(可选)
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

### 为WebAPI项目添加引用

1. 添加EF Core & Sqlite
    ```sh
    dotnet add SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj package Microsoft.EntityFrameworkCore.Sqlite
    ```
1. 添加Swagger.net
    ```sh
    dotnet add SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj package Swashbuckle.AspNetCore
    ```
1. 若EF版本与Core不一致，可以添加EF
    ```sh
    dotnet add SimpleUsers.WebAPI/SimpleUsers.WebAPI.csproj package Microsoft.EntityFrameworkCore
    ```

## .net core

.net core 相比.net framework更加模块化。

- 自带依赖注入(IServiceCollection)
- 全新的程序配置方式(IConfiguration)，支持多种文件格式配置以及环境变量配置等

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
    dotnet add reference ..\SimpleUsers.Core\SimpleUsers.Core.csproj
    ```
3. 添加单元测试类
4. 执行单元测试
    ```sh
    cd ..
    dotnet test
    ```

## docker支持

[Dockerfile](Dockerfile)为生成docker镜像的脚本文件

1. 通过带有SDK的基础镜像(`Build`)还原nuget包、编译、发布DLL
2. 把`Build`镜像中发布的DLL拷贝到只有运行时的镜像(`Runtime`)，设置docker的入口脚本
3. 通过生成脚本生成和执行docker（[Windows](build.ps1)以及[Linux](build.sh)）

## 后续

- [EF数据库迁移(DbMigration)](https://msdn.microsoft.com/en-us/data/jj591621.aspx#initializer)
- 使用Autofac依赖注入，可以参考我在读一本书时的[示例](https://github.com/stoneflyop1/MuscleFellow)
- [使用razor pages制作页面](https://docs.microsoft.com/en-us/aspnet/core/tutorials/razor-pages/razor-pages-start?view=aspnetcore-2.1)
- [使用IdentityServer4实现OAuth及第三方登录](https://github.com/IdentityServer/IdentityServer4)
