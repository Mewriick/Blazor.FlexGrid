# Blazor.FlexGrid
GridView component for Blazor

> Easy way for displaying lits of items in table

<img src="/docs/ReadmeImages/table_gif.gif" alt="table_gif"/>

## IMPORTANT!
**Still development not completely finished and rapidly continue. Next versions can containt breaking changes** 

**Breaking change after support .NET Core 3.1 Preview 1**
> In Blazor WASM you have to manually add into index.html links to the css and js FlexGrid files
```cs
<link href="_content/Blazor.FlexGrid/FlexGridStyles.css" rel="stylesheet" />
<link rel="stylesheet" href="_content/Blazor.FlexGrid/fontawesome/css/fontawesome.css" />
<link rel="stylesheet" href="_content/Blazor.FlexGrid/fontawesome/css/all.css" />
<link rel="stylesheet" href="_content/Blazor.FlexGrid/fontawesome/css/regular.css" />
<link rel="stylesheet" href="_content/Blazor.FlexGrid/fontawesome/css/solid.css" />
<link rel="stylesheet" href="_content/Blazor.FlexGrid/fontawesome/css/svg-with-js.css" />
<link rel="stylesheet" href="_content/Blazor.FlexGrid/fontawesome/css/v4-shims.css" />
<script src="_content/Blazor.FlexGrid/flexGridIntereop.js"></script>
```

**Version 0.11.0 contains new Triggers feature more info in [wiki](https://github.com/Mewriick/Blazor.FlexGrid/wiki/Triggers)** 

# Instalation
[![NuGet Pre Release](https://img.shields.io/badge/nuget-0.11.0-orange.svg)](https://www.nuget.org/packages/Blazor.FlexGrid)

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

And Add this into csproj of client project
```cs
<ItemGroup>
	<BlazorLinkerDescriptor Include="Linker.xml" />
</ItemGroup>
```

# Setup
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddFlexGrid();
}
```

# Setup Blazor App as RazorComponents AKA ServerSide Blazor App
```cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddFlexGridServerSide();
}
```
Note: Add the following line to the Configure(...) method of your Startup.cs file.

```cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    app.UseFlexGrid(env.WebRootPath);
}
```
From your index.(cs)html add references
```cs
<link rel="stylesheet" href="Blazor.FlexGrid/FlexGridStyles.css" />
<link rel="stylesheet" href="Blazor.FlexGrid/fontawesome/css/fontawesome.css" />
<link rel="stylesheet" href="Blazor.FlexGrid/fontawesome/css/all.css" />
<link rel="stylesheet" href="Blazor.FlexGrid/fontawesome/css/regular.css" />
<link rel="stylesheet" href="Blazor.FlexGrid/fontawesome/css/solid.css" />
<link rel="stylesheet" href="Blazor.FlexGrid/fontawesome/css/svg-with-js.css" />
<link rel="stylesheet" href="Blazor.FlexGrid/fontawesome/css/v4-shims.css" />
<script src="Blazor.FlexGrid/flexGridIntereop.js"></script>
```

For properly working of **LazyLoaded** functionallity some services must be registered in IoC. Because in web scenario **FlexGrid** uses **Http** services which are provided in IoC
by default, but in server side scenario you have to provide these services.

```cs
public void ConfigureServices(IServiceCollection services)
{    
	services.TryAddScoped(typeof(ILazyDataSetLoader<>), typeof(NullLazyDataSetLoader<>));
    services.TryAddScoped(typeof(ILazyDataSetItemManipulator<>), typeof(NullLazyDataSetItemManipulator<>));
    services.TryAddScoped(typeof(ICreateItemHandle<,>), typeof(NullCreateItemHandler<,>));
}
```

These services are registered by defaul in IoC if you want for your grid use funtionality like lazy loading data, inlinde editing or create item form
you have to provide these services for your models.

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
<img src="/docs/ReadmeImages/flextable.png" alt="table"/>

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

# Permission restriction
You can configure permission restricion of showing/editing values for each column in table for current logged user. You only have to do 3 things.
First create class which implements interface **ICurrentUserPermission**. Second provide configuration for permission restriction for exmaple:

```cs
public void Configure(EntityTypeBuilder<Customer> builder)
{
	builder.Property(c => c.Email)
		.HasReadPermissionRestriction(perm => perm.IsInRole("Read"))
		.HasWritePermissionRestriction(perm => perm.IsInRole("Write"));
}
```

And last thing is register **ICurrentUserPermission** into DI container as **Singleton**.

# Blazor components in column
You can configure column value for rendering **Blazor** component this way:
Fisrt add using **Blazor.FlexGrid.Components.Configuration**.
Second inject required service **BlazorComponentColumnCollection<T>** into HTML of component where you use **FlexGrid**
And the last thing you have to provide **RenderFragment** for columns

```cs
@inject BlazorComponentColumnCollection<WeatherForecast> Collection

@{
    RenderFragment<WeatherForecast> weatherTemp = (weather) => @<BlazorButton>@weather.Summary</BlazorButton>;
	Collection.AddColumnValueRenderFunction(w => w.Summary, weatherTemp);
}
```

Also you can use registration of fragment in startup configuration
```cs
RenderFragment<Customer> customerEmailComponent = (Customer customer) => delegate (RenderTreeBuilder rendererTreeBuilder)
{
    var internalBuilder = new BlazorRendererTreeBuilder(rendererTreeBuilder);
    internalBuilder
        .OpenElement(HtmlTagNames.H4)
        .AddContent(customer.Email)
        .CloseElement();
};

builder.Property(c => c.Email)
    .HasBlazorComponentValueRender(customerEmailComponent);
```

# Filtering
You can define which column can be filtered.

```cs
builder.Property(e => e.TemperatureF)
    .IsFilterable();    
```

For filter exists two modes, for standard **TableDataSet** is filtering done on client side across whole collection of items.
For **LazyTableDataSet** is filter object sended to the server. More info how it is works is [here](https://github.com/Mewriick/Blazor.FlexGrid/wiki/LazyTableDataSet---Client-Side-Blazor#filtering)

# Grouping
You can enable dynamic grouping functinallity for table
```cs
builder.EnableGrouping(options =>
{
    options.GroupPageSize = 15;
});
```

You can define page size for grouped items. Grouping works in two modes, for standard **TableDataSet** is grouping done on client side across whole collection of items.
For **LazyTableDataSet** is **GroupExpression** sended to the server. More info how it is works is [here](https://github.com/Mewriick/Blazor.FlexGrid/wiki/LazyTableDataSet---Client-Side-Blazor#grouping)

# Create item form
You can have very easily **Create item form** for your models. You can configure even different type of model for create item form
than is used for rendering in **FlexGrid**. You can also specify the return type from your Api which can be different than input.
Now the restriction for model is that have **default constructor**
Configuration example:

```cs
builder.AllowCreateItem<WeatherForecastCreateModel, WeatherForecast>(conf =>
{
    conf.CreatePermissionRestriction = p => p.IsInRole("TestRole");
    conf.CreateUri = "/api/SampleData/WeatherForecast";
});
```

You can also restrict the creation of items only for some users. Important property is **CreateUri** which must be filled. Form includes also 
validations which are run after every change. Submit can be done only if form is in **Valid** state. For rendering are used two layouts types.
You can have one column layout or you can have two columns layout. Now the behavior is that every model that have more than 6 properties the
two column layout is used. You can also write your own layout. You only have to inherit from **BaseCreateItemFormLayout** and also you must
create your own layout provider by creating class which implements **IFormLayoutProvider** and register this provider to IoC. 
Properties are in default layouts rendered in order that they are write in class.
After item is succesfully created event **NewItemCreated** is fired.
You can also change the modal dialog css classes by using:

```
builder.AppendCssClasses(conf =>
{
    conf.CreateFormCssClasses.ModalSize = "modal-lg";
});
```

# Master / Detail 
You can have multiple **DataSets** related together and rendered only with one **FlexGrid** component. If you have object that have property which is collection
**FlexGrid** component automatically find out this and will render this kind of object as Master / Detail grid.

Example configuration
```cs
public void Configure(EntityTypeBuilder<Customer> builder)
{
    builder.IsMasterTable();
    builder.HasDetailRelationship<Order>(c => c.Id, o => o.CustomerId)
        .HasLazyLoadingUrl("/api/Order/Orders")
        .HasUpdateUrl("/api/Order/Update")
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
If you want use **LazyTableDataSet** you must provide url address for loading. This is not required in ServerSide solution because you have to create your own DataSet
which implements **ILazyDataSetLoader**.

Example page with Master/Detail grid

```cs
@addTagHelper *, Blazor.FlexGrid
@using Blazor.FlexGrid.Demo.Shared
@using Blazor.FlexGrid.DataAdapters
@inject HttpClient Http
@inject MasterTableDataAdapterBuilder<Customer> MasterAdapterBuilder
@inject LazyLoadedTableDataAdapter<Order> ordersAdapter
@page "/masterdetailgrid"

<h1>Customers</h1>

<GridView DataAdapter="@customersMasterDataAdapter" PageSize="5"></GridView>

@functions{
    CollectionTableDataAdapter<Customer> customerDataAdapter;
    CollectionTableDataAdapter<CustomerAddress> customerAddressesDataAdapter;
    MasterTableDataAdapter<Customer> customersMasterDataAdapter;

    protected override async Task OnInitAsync()
    {
        var customers = await Http.GetJsonAsync<Customer[]>("/api/Customer/Customers");
        var customersAddresses = await Http.GetJsonAsync<CustomerAddress[]>("/api/Customer/CustomersAddresses");
        customerDataAdapter = new CollectionTableDataAdapter<Customer>(customers);
        customerAddressesDataAdapter = new CollectionTableDataAdapter<CustomerAddress>(customersAddresses);

        customersMasterDataAdapter = MasterAdapterBuilder
            .MasterTableDataAdapter(customerDataAdapter)
            .WithDetailTableDataAdapter(ordersAdapter)
            .WithDetailTableDataAdapter(customerAddressesDataAdapter)
            .Build();
    }
}
```
For building Master / Detail **TableDataSet** is used **MasterTableDataAdapter** which is register in DI container. 
Master / Detail usage you can find in ServerSide Blazor demo project

# Inline editing
You can use inline editing feature by configuring grid
```cs
public void Configure(EntityTypeBuilder<Order> builder)
{
	builder.AllowInlineEdit();
	// Or
    builder.AllowInlineEdit(conf =>
    {
        conf.AllowDeleting = true;
        conf.DeletePermissionRestriction = perm => perm.IsInRole("TestRole");
    });
}
```
You can also configure which columns will be editable for current logger user, see **Permission restriction** section. If you are using 
**CollectionTableDataAdapter** chagnes are saved only into local object in list. For saving to the server you have to write your own functionallity.
If you are using **LazyLoadedTableDataAdapter** and Client/Server mode you must provide url for updating of item.

```cs
<GridView DataAdapter="@forecastAdapter"
          LazyLoadingOptions="@(new LazyLoadingOptions() {
                                    DataUri = "/api/SampleData/WeatherForecasts",
                                    PutDataUri = "/api/SampleData/UpdateWeatherForecast",
                                    DeleteUri = "/api/SampleData/Delete/{Id}" })"
          PageSize="10"
          SaveOperationFinished="@ItemSavedOperationFinished"
          DeleteOperationFinished="@ItemDeletedOperationFinished">
</GridView>
```
And the Http request will be send to the server. For fully working delete feature you have to set properly **DeleteUri** of **LazyLoadingOptions**. 
In url is a template where at end you must specify the name of object property in { } (for example {Id}) which is key and this key is send to the action method on server side.

Or if the Grid is used as detail 

```cs
builder.HasDetailRelationship<Order>(c => c.Id, o => o.CustomerId)
    .HasLazyLoadingUrl("/api/Order/Orders")
    .HasUpdateUrl("/api/Order/UpdateOrder")
    .HasCaption("Orders")
    .HasPageSize(10);
```

# Events
You can subscribe some events which **FlexGrid** provides only things which you must do are add using
**@using Blazor.FlexGrid.Components.Events** and register **EventHandler** in HTML of Grid component.

**Suppoerted events:**  
``SaveOperationFinished``  
``DeleteOperationFinished``  
``NewItemCreated``  
``OnItemClicked``  

# Design
You can override some default CssClasses by your own CssClasses by using fluent api configuration.
```cs
public void Configure(EntityTypeBuilder<WeatherForecast> builder)
{  
	builder.AppendCssClasses(conf =>
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
``Create proper Docs``
``Add UnitTests``
``More fluent API configuration``
``Filtration support``

