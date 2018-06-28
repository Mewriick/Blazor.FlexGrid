# Blazor.FlexGrid
GridView component for Blazor

> Easy way for displaying lits of items in table

<img src="/docs/table_gif.gif" alt="table_gif"/>

## IMPORTANT!
**Still development not completely finished. Nuget package coming soon.** 

# Setup
```cs
var serviceProvider = new BrowserServiceProvider(services =>
{
    services.AddFlexGrid(cfg =>
    {
        cfg.ApplyConfiguration();
    });
});
```

In your Blazor component add Tag helper and required usings
```cs
@addTagHelper *, Blazor.FlexGrid
```

# Example
```cs
@addTagHelper *, Blazor.FlexGrid
@using Blazor.FlexGrid.Demo.Shared
@using Blazor.FlexGrid.DataAdapters
@inject HttpClient Http
@page "/grid"

<h1>Weather forecast</h1>

<GridView DataAdapter="@dataAdapter" PageSize="5"></GridView>

@functions{
    CollectionTableDataAdapter<WeatherForecast> dataAdapter;

    protected override async Task OnInitAsync()
    {
        var forecast = await Http.GetJsonAsync<WeatherForecast[]>("/api/SampleData/WeatherForecastsSimple");
        dataAdapter = new CollectionTableDataAdapter<WeatherForecast>(forecast);
    }
}
```

Result 
<img src="/docs/flextable.png" alt="table"/>

# Configuration
You do not need define informations about columns and component will render columns by **properties** of object type which is associated
with **table data adapter** which provide *data set* for **Table** component. Value for column is provided by **ToString** method on the property type.
Or you can configure some behavior for table and columns by using fluent api which is supported in classes where interface **IEntityTypeConfiguration<TItem>** is implemented.
```cs
public class WeatherForecastGridConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
    public void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        builder.Property(e => e.Date)
            .HasCaption("Date")
            .HasValueFormatter(d => d.ToShortDateString());

        builder.Property(e => e.Summary)
            .HasCaption("MySummary")
            .HasOrder(1)
            .HasValueFormatter(s => $"{s}!");

        builder.Property(e => e.TemperatureC)
            .IsSortable();

        builder.Property(e => e.TemperatureF)
            .IsSortable();
    }
}
```
And provide this configuration 
```cs
var serviceProvider = new BrowserServiceProvider(services =>
{
    services.AddFlexGrid(cfg =>
    {
        cfg.ApplyConfiguration(new WeatherForecastGridConfiguration());
    });
});
```

# Data Adapters
You can use simple **CollectionTableDataAdapter** which requires collection of items. 
```cs
@functions{
    CollectionTableDataAdapter<WeatherForecast> dataAdapter;

    protected override async Task OnInitAsync()
    {
        var forecast = await Http.GetJsonAsync<WeatherForecast[]>("/api/SampleData/WeatherForecastsSimple");
        dataAdapter = new CollectionTableDataAdapter<WeatherForecast>(forecast);
    }
}
```

Another data adapter type is **LazyLoadedTableDataAdapter<TItem>** which support lazy loading data from API. This type of adapter 
is registered in dependency injection conatiner and you only must provide **LazyLoadingOptins** to table component. 
```cs
@addTagHelper *, Blazor.FlexGrid
@using Blazor.FlexGrid.Demo.Shared
@using Blazor.FlexGrid.DataAdapters
@using Blazor.FlexGrid.DataSet.Options
@page "/lazyloadedgrid"
@inject HttpClient Http
@inject LazyLoadedTableDataAdapter<WeatherForecast> forecastAdapter

<GridView DataAdapter="@forecastAdapter" LazyLoadingOptions="@(new LazyLoadingOptions() { DataUri = "/api/SampleData/WeatherForecasts" })" PageSize="10"></GridView>
```
Also you must provide the server side part

```cs
public IActionResult WeatherForecasts(int pageNumber, int pageSize, SortingParams sortingParams)
{
    var rng = new Random();

    var items = Enumerable.Range(1, 100).Skip(pageSize * pageNumber).Take(pageSize).Select(index =>
                new WeatherForecast
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = rng.Next(-20, 55),
                    Summary = Summaries[rng.Next(Summaries.Length)]

                });

    items = string.IsNullOrEmpty(sortingParams.SortExpression)
        ? items
        : items.AsQueryable().OrderBy(sortingParams.SortExpression).ToList();

    return Ok(
        new
        {
            Items = items,
            TotalCount = 100
        });
}
```
After that you have fully pageable and sortable table with lazy loaded data after you select new page

## RoadMap
``More fluent API configuration``
``Filtration support``
``Inline editing``
``Show detatil of item support``
