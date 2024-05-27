using CourseProvider.Data.Data.Contexts;
using CourseProvider.Data.GraphQL;
using CourseProvider.Data.GraphQL.Mutations;
using CourseProvider.Data.GraphQL.ObjectTypes;
using CourseProvider.Data.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddPooledDbContextFactory<DataContext>(x =>
        {
            x.UseCosmos(Environment.GetEnvironmentVariable("COSMOS_URI")!, Environment.GetEnvironmentVariable("COSMOS_DB")!)
            .UseLazyLoadingProxies();

        });

        services.AddScoped<ICourseService, CourseService>();

        services.AddGraphQLFunction()
        .AddQueryType<Query>()
        .AddMutationType<CourseMutation>()
        .AddType<CourseType>();


        var sp = services.BuildServiceProvider();
        using var scope = sp.CreateScope();
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<DataContext>>();
        using var context = dbContextFactory.CreateDbContext();
        context.Database.EnsureCreated();
    })
    .Build();

host.Run();
