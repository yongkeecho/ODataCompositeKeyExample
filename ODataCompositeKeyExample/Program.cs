using ODataCompositeKeyExample.Data;
using ODataCompositeKeyExample.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.OData.Batch;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var connectionString = configuration.GetConnectionString("Admin");
builder.Services.AddDbContext<AdminDbContext>(o => o.UseNpgsql(connectionString), ServiceLifetime.Transient);

//builder.Services.AddEndpointsApiExplorer();
var modelBuilder = new ODataConventionModelBuilder();
modelBuilder.EnableLowerCamelCase();
modelBuilder.EntitySet<User>("Users").EntityType.HasKey(user => user.Id);
modelBuilder.EntitySet<Department>("Departments").EntityType.HasKey(department => department.Id);
modelBuilder.EntitySet<Asset>("Assets").EntityType.HasKey(asset => asset.Id);
modelBuilder.EntitySet<AssetSme>("AssetSmes").EntityType.HasKey(sme => new { sme.AssetId, sme.UserId });
builder.Services.AddControllers().AddOData(
    options => options.Select().Filter().OrderBy().Expand().Count().SetMaxTop(null).AddRouteComponents(
        "odata",
        modelBuilder.GetEdmModel(),
        new DefaultODataBatchHandler()));

var app = builder.Build();
app.UseODataRouteDebug();
//app.UseODataQueryRequest();
app.UseODataBatching();
app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());
app.Run();