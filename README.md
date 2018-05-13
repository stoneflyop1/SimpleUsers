# 一个简单的用户系统

## VS Code准备

安装如下扩展(一般打开过C#文件的话，前两个扩展应该安装好了)

- C#
- C# Extensions
- C# FixFormat
- C# XML Documentation Comments

## 创建步骤

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