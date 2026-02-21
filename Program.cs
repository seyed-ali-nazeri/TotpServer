using Microsoft.EntityFrameworkCore;
using OtpNet;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthDb>(options =>
    options.UseSqlite("Data Source=totp.db"));

var app = builder.Build();


// create database automatically
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuthDb>();
    db.Database.EnsureCreated();
}

app.MapGet("/", () => "TOTP Server Running Successful");


// register new account
app.MapPost("/register", async (string username, AuthDb db) =>
{
    var secretBytes = KeyGeneration.GenerateRandomKey(20);

    var secretBase32 =
        Base32Encoding.ToString(secretBytes);

    var user = new User
    {
        Id = Guid.NewGuid(),
        Username = username,
        Secret = secretBase32
    };

    db.Users.Add(user);

    await db.SaveChangesAsync();

    var uri =
        $"otpauth://totp/TotpServer:{username}" +
        $"?secret={secretBase32}&issuer=TotpServer";

    return Results.Ok(new
    {
        user.Id,
        user.Username,
        Secret = secretBase32,
        Uri = uri
    });
});


// verify code
app.MapPost("/verify", async (
    Guid userId,
    string code,
    AuthDb db) =>
{
    var user = await db.Users.FindAsync(userId);

    if (user == null)
        return Results.NotFound();

    var secretBytes =
        Base32Encoding.ToBytes(user.Secret);

    var totp = new Totp(secretBytes);

    var valid =
        totp.VerifyTotp(
            code,
            out long step,
            new VerificationWindow(1, 1));

    return Results.Ok(new
    {
        Valid = valid
    });
});
app.MapPost("/login", async (
    string username,
    string code,
    AuthDb db) =>
{
    var user =
        await db.Users
        .FirstOrDefaultAsync(
            u => u.Username == username);

    if (user == null)
        return Results.NotFound(
            "User not found");

    var secretBytes =
        Base32Encoding.ToBytes(
            user.Secret);

    var totp =
        new Totp(secretBytes);

    var valid =
        totp.VerifyTotp(
            code,
            out long step,
            new VerificationWindow(1, 1));

    if (!valid)
        return Results.Unauthorized();

    return Results.Ok(new
    {
        Message = "Login success",
        Username = username
    });
});

app.Run();



// database classes

class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = "";

    public string Secret { get; set; } = "";
}

class AuthDb : DbContext
{
    public AuthDb(DbContextOptions<AuthDb> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}