using GqeberhaClinic.Areas.Identity.Data;
using GqeberhaClinic.Data;
using GqeberhaClinic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<GqebheraUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
//emasil services
builder.Services.AddTransient<IEmailSender, EmailSender>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
//created roles default
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Administration", "Patient", "Nurse", "Counsellor", "Doctor" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
           await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

}
app.UseEndpoints(end =>
{
    end.MapControllerRoute(
        name: "Record",
        pattern: "Records/Create/{FileID?}",
        defaults: new { Controller = "Records", action = "Create" }
        );
    end.MapControllerRoute(
        name: "File",
        pattern: "Medical_File/Patient_File/{FileID?}",
        defaults: new { Controller = "Medical_File", action = "Patient_File" }
        );
});

//assigning admin role to one user only
using (var scope = app.Services.CreateScope())
{
    var userManager =

    scope.ServiceProvider.GetRequiredService<UserManager<GqebheraUser>>();
    string firstName = "Micheal";
    string lastName = "Mazaza";
    string email = "MichealM@gmail.com";
    string password = "MichealM@1";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new GqebheraUser();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Administration");
    }

}
using (var scope = app.Services.CreateScope())
{
    var userManager =

    scope.ServiceProvider.GetRequiredService<UserManager<GqebheraUser>>();
    string firstName = "Micheal";
    string lastName = "Mohladi";
    string email = "Mohladi@gmail.com";
    string password = "Mohladi@1";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new GqebheraUser();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Nurse");
    }

}
using (var scope = app.Services.CreateScope())
{
    var userManager =

    scope.ServiceProvider.GetRequiredService<UserManager<GqebheraUser>>();
    string firstName = "Miguel";
    string lastName = "Lathel";
    string email = "Miguel@gmail.com";
    string password = "Miguel@10";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new GqebheraUser();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;
        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Patient");
    }

}
using (var scope = app.Services.CreateScope())
{
    var userManager =

    scope.ServiceProvider.GetRequiredService<UserManager<GqebheraUser>>();
    string firstName = "Liyema";
    string lastName = "Yake";
    string email = "LiyemaY@gmail.com";
    string password = "LiyemaY@1";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new GqebheraUser();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Counsellor");
    }

}

using (var scope = app.Services.CreateScope())
{
    var userManager =

    scope.ServiceProvider.GetRequiredService<UserManager<GqebheraUser>>();
    string firstName = "Thimna";
    string lastName = "Magingxa";
    string email = "thimnamagingxa@gmail.com";
    string password = "Magingxa@17";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new GqebheraUser();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Nurse");
    }

}
using (var scope = app.Services.CreateScope())
{
    var userManager =

    scope.ServiceProvider.GetRequiredService<UserManager<GqebheraUser>>();
    string firstName = "John";
    string lastName = "Doe";
    string email = "johndoe@gmail.com";
    string password = "Magingxa@17";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new GqebheraUser();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Doctor");
    }

}
using (var scope = app.Services.CreateScope())
{
    var userManager =

    scope.ServiceProvider.GetRequiredService<UserManager<GqebheraUser>>();
    string firstName = "James";
    string lastName = "Harden";
    string email = "jamesharden@gmail.com";
    string password = "Magingxa@17";
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new GqebheraUser();
        user.FirstName = firstName;
        user.LastName = lastName;
        user.UserName = email;
        user.Email = email;
        user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Patient");
    }

}
app.Run();
