using DataGenerator;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .SetBasePath("/Users/tchaura/RiderProjects/ProjectPilot/ProjectPilotApi")
    .AddJsonFile("appsettings.json")
    .Build();

var tasksGenerator = new TasksGenerator(configuration, "tasks.json");
tasksGenerator.AddTasksToJira().GetAwaiter().GetResult();