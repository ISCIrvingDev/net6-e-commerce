using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ShopOnline.Web;
using ShopOnline.Web.Services;
using ShopOnline.Web.Services.Contracts;
using System.Globalization;

/*
Errror con blazor decimal.tostring("c") not working
Blazor displays ¤ instead of $ when using ToString("C")

// Ref
https://stackoverflow.com/questions/67428902/blazor-displays-%C2%A4-instead-of-when-using-tostringc
https://stackoverflow.com/questions/56993596/decimal-tostringc-produces-%C2%A4-currency-symbol-on-linux

// Solucion
* Agregar la siguiente linea para asignar el "DefaultThreadCurrentCulture"
*/
CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Servicios - Inicio
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// URL hardcodeada del proyecto "ShopOnline.API"
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7061/") });

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
// Servicios - Fin

await builder.Build().RunAsync();
