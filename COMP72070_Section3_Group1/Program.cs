using COMP72070_Section3_Group1.Controllers;
using COMP72070_Section3_Group1.Models;
using COMP72070_Section3_Group1.Comms;
using System.Net.Sockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

Account accountInstance = new Account();

builder.Services.AddSingleton<Account>(accountInstance);

// add visitor manager to track visitors
VisitorManager VISITORMANAGER = new VisitorManager();
builder.Services.AddSingleton<VisitorManager>(VISITORMANAGER);

// add client for communicating with the server
Client CLIENT = new Client();
CLIENT.Connect();
CLIENT.FetchImages();
builder.Services.AddSingleton<Client>(CLIENT);

// for identifying sessions and visitors
builder.Services.AddSession(); // options => { //some stuff }

// enable only on yao computer v
// builder.WebHost.UseUrls("http://192.168.1.10:5128", "https://192.168.1.10:7048"); // used for vm connection 
// enable only on yao computer ^

// store posts to be displayed
List<Post> POSTLIST = new List<Post>();// fetch the posts from the server
builder.Services.AddSingleton<List<Post>>(POSTLIST);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.UseWebSockets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Start}/{action=Index}/{id?}");

app.Run();
