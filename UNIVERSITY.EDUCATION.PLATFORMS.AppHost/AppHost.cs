var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.UNIVERSITY_EDUCATION_PLATFORMS>("university-education-platforms");

builder.Build().Run();
