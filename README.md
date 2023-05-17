# Migrations

# Migrations.EPiServer

This package allows you to create and run Entity Framework like migrations. All you need to do is implement `Forte.Migrations.IMigration` interface and mark your class with `Forte.Migrations.MigrationAttribute`

### Setup

```c#
public class Startup
{
    public void ConfigureServices(IServiceCollection services) 
    {
        /*...*/
        services.AddMigrations();
        /*...*/
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        /*...*/
        app.RunMigrations(typeof(Startup).Assembly);
        /*...*/
    }
}
```

By default, `RunMigrations` will run all migrations in the assembly as an `Administrator` user. If you want to change this behavior you can create your own service, instantiate new `MigrationRunnerBuilder` instance and specify principle before building the runner.

### Usage

```c#
[Migration("4201F11D-3939-44C2-853E-F918739628C8")]
public class MigrationExample : IMigration
{
    private readonly IContentRepository _contentRepository;

    public MigrationExample(IContentRepository contentRepository)
    {
        _contentRepository = contentRepository;
    }

    public async Task ExecuteAsync()
    {
        //move content from old, deprecated field to a new one
        var articles = _contentRepository.GetChildren<Article>(ContentReference.StartPage)

        foreach (var article in articles)
        {
            var articleClone = article.CreateWritableClone() as Article;
            articleClone.NewField = article.OldField;
            _contentRepository.Save(articleClone, AccessLevel.Publish);
        }
    }
}

```
