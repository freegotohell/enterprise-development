var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql").WithLifetime(ContainerLifetime.Persistent);

var database = mysql.AddDatabase("autoservice");

builder.AddProject<Projects.AutoService_Api>("api").WithReference(database).WaitFor(database);

builder.Build().Run();