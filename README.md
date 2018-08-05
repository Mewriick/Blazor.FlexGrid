# Blazor.FlexGrid
GridView component for Blazor

> Easy way for displaying lits of items in table

<img src="/docs/table_gif.gif" alt="table_gif"/>

## IMPORTANT!
**Still development not completely finished and rapidly continue. Next versions can containt breaking changes** 

# Instalation
[![NuGet Pre Release](https://img.shields.io/badge/nuget-0.2.0-orange.svg)](https://www.nuget.org/packages/Blazor.FlexGrid)

After nuget instalation you must create in Blazor.Client app Linker.xml file because nuget use some features which are not supported in default mono managed interpreter from WebAssembly
(https://github.com/mono/mono/issues/8872)

```cs
<linker>
  <assembly fullname="mscorlib">
    <!-- Preserve all methods on WasmRuntime, because these are called by JS-side code
    to implement timers. Fixes https://github.com/aspnet/Blazor/issues/239 -->
    <type fullname="System.Threading.WasmRuntime" />
  </assembly>
  <assembly fullname="System.Core">
    <!-- This is required by JSon.NET and any expression.Compile caller -->
    <type fullname="System.Linq.Expressions*" />
    <type fullname="System.Linq.EnumerableRewriter*" />
    <type fullname="System.Linq.Queryable*" />
    <type fullname="System.Linq.Enumerable*" />
  </assembly>
  <!-- Name of the entry point assembly -->
  <assembly fullname="Blazor.FlexGrid.Demo.Client" />
</linker>
```

# Setup
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddFlexGrid();
}
```

# Setup ServerSide Blazor App
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddFlexGridServerSide();
}
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

# Master / Detail 
You can have multiple **DataSets** related together and rendered only with one **FlexGrid** component. If you have object that have property which is collection
**FlexGrid** component automatically find out this and will render this kind of object as Master / Detail grid.

Example configuration
```cs
public void Configure(EntityTypeBuilder<Customer> builder)
{
    builder.IsMasterTable();
    builder.HasDetailRelationship<Order>(c => c.Id, o => o.CustomerId)
        .HasCaption("Orders")
        .HasPageSize(10);

    builder.HasDetailRelationship<CustomerAddress>(c => c.Id, o => o.CustomerId)
        .HasCaption("Customer addresses");
}

public void Configure(EntityTypeBuilder<Order> builder)
{
    builder.IsMasterTable();
    builder.HasDetailRelationship(o => o.OrderItems)
        .HasCaption("Order products");
}
```

For correct working of Master / Detail grid you must configure relation ships between objects. Also you can define some additional options for related grid component.

Example page with Master/Detail grid

```cs
@addTagHelper *, Blazor.FlexGrid
@using Blazor.FlexGrid.Demo.Shared
@using Blazor.FlexGrid.DataAdapters
@page "/masterdetailgrid"
@inject CustomerService CustomerService
@inject OrderService OrderService
@inject MasterTableDataAdapterBuilder<Customer> MasterAdapterBuilder

<h1>Customers</h1>

<GridView DataAdapter="@customersMasterDataAdapter" PageSize="5"></GridView>

@functions
{
	CollectionTableDataAdapter<Customer> customerDataAdapter;
	CollectionTableDataAdapter<CustomerAddress> customerAddressesDataAdapter;
	CollectionTableDataAdapter<Order> ordersDataAdapter;
	MasterTableDataAdapter<Customer> customersMasterDataAdapter;

	protected override void OnInit()
	{
		var customers = CustomerService.Customers();
		var customersAddresses = CustomerService.CustomersAddresses();
		var orders = OrderService.Orders();
		customerDataAdapter = new CollectionTableDataAdapter<Customer>(customers);
		ordersDataAdapter = new CollectionTableDataAdapter<Order>(orders);
		customerAddressesDataAdapter = new CollectionTableDataAdapter<CustomerAddress>(customersAddresses);

		customersMasterDataAdapter = MasterAdapterBuilder
			.MasterTableDataAdapter(customerDataAdapter)
			.WithDetailTableDataAdapter(ordersDataAdapter)
			.WithDetailTableDataAdapter(customerAddressesDataAdapter)
			.Build();
	}
}
```
For building Master / Detail **TableDataSet** is used **MasterTableDataAdapter** which is register in DI container. Master / Detail now can fully work only with 
**CollectionTableDataAdapter** as detail data adapters. In future also **LazyLoadedTableDataAdapter** will be supported.

# Design
You can override some default CssClasses by your own CssClasses by using fluent api configuration
```cs
public void Configure(EntityTypeBuilder<WeatherForecast> builder)
{  
	builder.UseCssClasses(conf =>
	{
		conf.Table = "my-table";
		conf.TableBody = "my-table-body";
		conf.TableCell = "my-table-cell";
		conf.TableHeader = "my-table-header";
		conf.TableHeaderCell = "my-table-header-cell";
		conf.TableHeaderRow = "my-table-header-row";
		conf.TableRow = "my-table-row";
	});
}
```

# Contributions and feedback
Please feel free to use the component, open issues, fix bugs or provide feedback.

## RoadMap
``Add UnitTests``
``More fluent API configuration``
``Filtration support``
``Inline editing``
``Show detatil of item support``
