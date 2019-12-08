# Getting Ready
Open Visual Studio 2019
Create a new project > ASP.NET Core Web Application [Next]
Project name: SimpleCrudWithAspNetCore2_2MvcAndEfCore2_2_6 [Create]
Select: .NET Core | ASP.NET Core 2.2 | Web Application (Model-View-Controller) [Create]

Open Package Manager Console and type:
Install-Package Microsoft.EntityFrameworkCore -Version 2.2.6

# Publish to GitHub
File Menu > Add to Source Control
(Now, you should see blue lock symbols next to each item on the solution explorer)

Open the following address on your browser and create an empty repository:
https://github.com/new

Copy the git address of the newly created empty repository.
(For me, it's https://github.com/yigith/SimpleCrudWithAspNetCore2_2MvcAndEfCore2_2_6.git)

Open Team Explorer - Home > Sync > Push to Remote Repository > Click on Publish Git Repo
Enter the URL of the empty Git repo and click Publish


