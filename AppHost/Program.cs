var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API_Book>("api-book");

builder.AddProject<Projects.MVC_Book>("mvc-book");

builder.AddProject<Projects.API_Users>("api-users");

builder.AddProject<Projects.API_Gateway>("api-gateway");

builder.Build().Run();
