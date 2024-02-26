using System.Text;
using System.Xml.Linq;

var appBuilder = WebApplication.CreateBuilder(args);

appBuilder.Configuration.AddJsonFile("configuration.json");

Library appLibrary = appBuilder.Configuration.GetSection("Library").Get<Library>() ?? new Library();

var appInstance = appBuilder.Build();

Profile defaultProfile = new Profile { Name = "Default mame", Surname = "Default surname" };
appInstance.MapGet("/library", () => "Hello World!");

appInstance.MapGet("/library/books", () =>
{
    StringBuilder booksInfoBuilder = new StringBuilder();
    foreach (var bookItem in appLibrary.books)
    {
        booksInfoBuilder.Append($"Title - {bookItem.Title}\nAuthor - {bookItem.Author}\n");
    }
    return booksInfoBuilder.ToString();
});

appInstance.MapGet("/library/profile/{id?}", (int? id) =>
{
    Profile userProfile;
    if (id is null)
    {
        userProfile = defaultProfile;
    }
    else
    {
        userProfile = appLibrary.profiles[(int)id];
    }
    return $"Profile name - {userProfile.Name}\nProfile surname - {userProfile.Surname}";
});

appInstance.Run();
