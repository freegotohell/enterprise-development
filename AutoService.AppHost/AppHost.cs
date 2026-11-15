IDistributedApplicationBuilder builder = DistributedApplication.CreateBuilder(args);

IResourceBuilder<MySqlServerResource> mysql = builder.AddMySql("mysql").WithLifetime(ContainerLifetime.Persistent);

IResourceBuilder<MySqlDatabaseResource> database = mysql.AddDatabase("autoservice");

builder.AddProject<Projects.AutoService_Api>("api").WithReference(database).WaitFor(database);

builder.Build().Run();