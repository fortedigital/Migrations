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

### Disable default initialization

By default, when you install this nuget package it will be initialized with default settings, i.e. it will be run as `Administrator` etc. (see [MigrationsModule](Migrations.EPiServer/MigrationsModule.cs)).
If you want to setup an initialization module by your own you can disable default one by adding `fMigrationsDisableInit` flag with value of `true` to your `AppSettings`
