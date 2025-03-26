var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API_Book>("api-book");

builder.AddProject<Projects.MVC_Book>("mvc-book");

builder.Build().Run();
