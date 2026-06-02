using MicroserviceGen.Domain;

namespace MicroserviceGen.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void GetTextBetween_TextWithoutEnvironment_Correct()
    {
        var start = "#api_begin";
        var end = "#api_end";
        var text = "#api_begin\ndotnet new grpc --name $name.Web\ndone\ncd ..\n#api_end";
        var expectedText = "\ndotnet new grpc --name $name.Web\ndone\ncd ..\n";

        Script.Instance.Initialize(text, Architecture.NLayer);
        var actualText = Script.Instance.GetTextBetween(start, end);
        Assert.That(actualText, Is.EqualTo(expectedText));
    }

    [Test]
    public void GetTextBetween_TextWithEnvironment_Correct()
    {
        var start = "#api_begin";
        var end = "#api_end";
        var text = "qwerr#api_begin\ndotnet new grpc --name $name.Web\ndone\ncd ..\n#api_enddwdwffewfwe";
        var expectedText = "\ndotnet new grpc --name $name.Web\ndone\ncd ..\n";

        Script.Instance.Initialize(text, Architecture.NLayer);
        var actualText = Script.Instance.GetTextBetween(start, end);
        Assert.That(actualText, Is.EqualTo(expectedText));
    }

    [Test]
    public void GetTextBetween_NoMatchText_Correct()
    {
        var start = "#api_begin";
        var end = "#api_end";
        var text = "qwerr\ndotnet new grpc --name $name.Web\ndone\ncd ..\ndwdwffewfwe";
        string? expectedText = null;

        Script.Instance.Initialize(text, Architecture.NLayer);
        var actualText = Script.Instance.GetTextBetween(start, end);
        Assert.That(actualText, Is.EqualTo(expectedText));
    }

    [Test]
    public void ReplaceTriggerCommandsFromAnotherScriptInRegion_Correct()
    {
        var start = "#efcore\n";
        var end = "#endefcore";
        var psqlScript = "echo \"Setting up for PostgreSQL with Npgsql\"\ncd $name.Web\n#specific_provider\ndotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 8.0.2\ndotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.20\ndotnet add reference ../$name.Infrastructure/$name.Infrastructure.csproj\nsed -i '1s|^|using $name.Infrastructure;\\\\n|' Program.cs\nsed -i '1s|^|using Microsoft.EntityFrameworkCore;\\\\n|' Program.cs\n#specific_provider\nsed -i '/var builder = WebApplication.CreateBuilder(args);/a \\\\nbuilder.Services.AddDbContext<PgsqlDbContext>(options => options.UseNpgsql(builder.Configuration[\\\"PgsqlConnectionStrings:DefaultConnection\\\"]));' Program.cs\n#specific_provider\nAPPSETTINGS=\\\"appsettings.json\\\" && APPSETTINGS_DEV=\\\"appsettings.Development.json\\\" && NEW_CONNECTION=\\\"\\\\\\\"PgsqlConnectionStrings\\\\\\\": {\\\\n    \\\\\\\"DefaultConnection\\\\\\\": \\\\\\\"Host=your_host;Database=your_database;Username=your_username;Password=your_password\\\\\\\"\\\\n  },\\\" && sed -i \\\"/^{/a $NEW_CONNECTION\\\" \\\"$APPSETTINGS\\\" && sed -i \\\"/^{/a $NEW_CONNECTION\\\" \\\"$APPSETTINGS_DEV\\\"\ncd ../$name.Infrastructure\ndotnet add package Microsoft.EntityFrameworkCore --version 8.0.20\ncd DatabaseContext\n#specific_provider\ndotnet new class -n PgsqlDbContext --project ../$name.Infrastructure.csproj\n#specific_provider\nFILE=\\\"PgsqlDbContext.cs\\\" && sed -i '1s|^|using Microsoft.EntityFrameworkCore;\\\\n|' \\\"$FILE\\\" && sed -i 's|public class PgsqlDbContext|public class PgsqlDbContext : DbContext|' \\\"$FILE\\\" && sed -i '/^\\\\s*}\\\\s*$/i \\\\    public PgsqlDbContext(DbContextOptions<PgsqlDbContext> options) : base(options) \\\\    { \\\\    } \\\\' \\\"$FILE\\\"\ncd ../..\n";
        var createDataLayerProjectScripts =
            "dotnet new classlib --name $name.DataAccess\ndotnet sln $name.sln add $name.DataAccess\ncd $name.DataAccess\nproj_version=\"net8.0\"\nfind . -name \"$name.DataAccess.csproj\" | while read -r file; do\n    if [ -f \"$file\" ]; then\n        sed -i \"s|<TargetFramework>.*</TargetFramework>|<TargetFramework>$proj_version</TargetFramework>|g\" \"$file\"\n    fi\ndone\nmkdir DatabaseContext\ncd ..\n";
        var currentScript = start + createDataLayerProjectScripts + psqlScript + end;
        Script.Instance.Initialize(currentScript, Architecture.NLayer);
        var newScript = "echo \"Pluggable efcore\"\ncd $name.Web\ndotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.20\n#specific_provider\ndotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.20\ndotnet add reference ../$name.Infrastructure/$name.Infrastructure.csproj\nsed -i '1s|^|using $name.Infrastructure;\\\\n|' Program.cs\nsed -i '1s|^|using Microsoft.EntityFrameworkCore;\\\\n|' Program.cs\n#specific_provider\nsed -i '/var builder = WebApplication.CreateBuilder(args);/a \\\\nbuilder.Services.AddDbContext<SqlservDbContext>(options => options.UseSqlServer(builder.Configuration[\\\"SqlservConnectionStrings:DefaultConnection\\\"]));' Program.cs\n#specific_provider\nAPPSETTINGS=\\\"appsettings.json\\\" && APPSETTINGS_DEV=\\\"appsettings.Development.json\\\" && NEW_CONNECTION=\\\"\\\\\\\"SqlservConnectionStrings\\\\\\\": {\\\\n    \\\\\\\"DefaultConnection\\\\\\\": \\\\\\\"Server=your_server;Database=your_database;User Id=your_username;Password=your_password;\\\\\\\"\\\\n  },\\\" && sed -i \\\"/^{/a $NEW_CONNECTION\\\" \\\"$APPSETTINGS\\\" && sed -i \\\"/^{/a $NEW_CONNECTION\\\" \\\"$APPSETTINGS_DEV\\\"\ncd ../$name.Infrastructure\ndotnet add package Microsoft.EntityFrameworkCore --version 8.0.20\ncd DatabaseContext\n#specific_provider\ndotnet new class -n SqlservDbContext --project ../$name.Infrastructure.csproj\n#specific_provider\nFILE=\\\"SqlservDbContext.cs\\\" && sed -i '1s|^|using Microsoft.EntityFrameworkCore;\\\\n|' \\\"$FILE\\\" && sed -i 's|public class SqlservDbContext|public class SqlservDbContext : DbContext|' \\\"$FILE\\\" && sed -i '/^\\\\s*}\\\\s*$/i \\\\    public SqlservDbContext(DbContextOptions<SqlservDbContext> options) : base(options) \\\\    { \\\\    } \\\\' \\\"$FILE\\\"\ncd ../..\n";
        var triggerCommandLabel = "#specific_provider";
        var result = Script.Instance.TryReplaceTriggerCommandsFromAnotherScriptInRegion(start, end, newScript, triggerCommandLabel);
        var expected = "#efcore\ndotnet new classlib --name $name.DataAccess\ndotnet sln $name.sln add $name.DataAccess\ncd $name.DataAccess\nproj_version=\"net8.0\"\nfind . -name \"$name.DataAccess.csproj\" | while read -r file; do\n    if [ -f \"$file\" ]; then\n        sed -i \"s|<TargetFramework>.*</TargetFramework>|<TargetFramework>$proj_version</TargetFramework>|g\" \"$file\"\n    fi\ndone\nmkdir DatabaseContext\ncd ..\necho \"Setting up for PostgreSQL with Npgsql\"\ncd $name.Web\n#specific_provider\ndotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.20\ndotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.20\ndotnet add reference ../$name.Infrastructure/$name.Infrastructure.csproj\nsed -i '1s|^|using $name.Infrastructure;\\\\n|' Program.cs\nsed -i '1s|^|using Microsoft.EntityFrameworkCore;\\\\n|' Program.cs\n#specific_provider\nsed -i '/var builder = WebApplication.CreateBuilder(args);/a \\\\nbuilder.Services.AddDbContext<SqlservDbContext>(options => options.UseSqlServer(builder.Configuration[\\\"SqlservConnectionStrings:DefaultConnection\\\"]));' Program.cs\n#specific_provider\nAPPSETTINGS=\\\"appsettings.json\\\" && APPSETTINGS_DEV=\\\"appsettings.Development.json\\\" && NEW_CONNECTION=\\\"\\\\\\\"SqlservConnectionStrings\\\\\\\": {\\\\n    \\\\\\\"DefaultConnection\\\\\\\": \\\\\\\"Server=your_server;Database=your_database;User Id=your_username;Password=your_password;\\\\\\\"\\\\n  },\\\" && sed -i \\\"/^{/a $NEW_CONNECTION\\\" \\\"$APPSETTINGS\\\" && sed -i \\\"/^{/a $NEW_CONNECTION\\\" \\\"$APPSETTINGS_DEV\\\"\ncd ../$name.Infrastructure\ndotnet add package Microsoft.EntityFrameworkCore --version 8.0.20\ncd DatabaseContext\n#specific_provider\ndotnet new class -n SqlservDbContext --project ../$name.Infrastructure.csproj\n#specific_provider\nFILE=\\\"SqlservDbContext.cs\\\" && sed -i '1s|^|using Microsoft.EntityFrameworkCore;\\\\n|' \\\"$FILE\\\" && sed -i 's|public class SqlservDbContext|public class SqlservDbContext : DbContext|' \\\"$FILE\\\" && sed -i '/^\\\\s*}\\\\s*$/i \\\\    public SqlservDbContext(DbContextOptions<SqlservDbContext> options) : base(options) \\\\    { \\\\    } \\\\' \\\"$FILE\\\"\ncd ../..\n#endefcore";
        Assert.That(Script.Instance.ScriptText.ToString(), Is.EqualTo(expected));
    }

    [Test]
    public void ReplaceTriggerCommandsFromAnotherScriptInRegion_Correct2()
    {
        var start = "#efcore\n";
        var end = "#endefcore";
        var sqlservScript = "cd $name.Web\ndotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.20\n#specific_provider\ndotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 8.0.20\ndotnet add reference ../$name.Infrastructure/$name.Infrastructure.csproj\nsed -i \"1s|^|using $name.Infrastructure;\\n|\" Program.cs\nsed -i \"1s|^|using Microsoft.EntityFrameworkCore;\\n|\" Program.cs\n#specific_provider\nsed -i \"/var builder = WebApplication.CreateBuilder(args);/a \\\\nbuilder.Services.AddDbContext<SqlservDbContext>(options => options.UseSqlServer(builder.Configuration[\\\"SqlservConnectionStrings:DefaultConnection\\\"]));\" Program.cs\n#specific_provider\nAPPSETTINGS=\"appsettings.json\" && APPSETTINGS_DEV=\"appsettings.Development.json\" && NEW_CONNECTION='  \"SqlservConnectionStrings\": {\\n    \"DefaultConnection\": \"Server=your_server;Database=your_database;User Id=your_username;Password=your_password;\"\\n  },' && sed -i \"/^{/a $NEW_CONNECTION\" \"$APPSETTINGS\" && sed -i \"/^{/a $NEW_CONNECTION\" \"$APPSETTINGS_DEV\"\ncd ../$name.Infrastructure\ndotnet add package Microsoft.EntityFrameworkCore --version 8.0.20\ncd DatabaseContext\n#specific_provider\ndotnet new class -n SqlservDbContext --project ../$name.Infrastructure.csproj\n#specific_provider\nFILE=SqlservDbContext.cs && sed -i '1s/^\\xEF\\xBB\\xBF//' $FILE && sed -i '1i using Microsoft.EntityFrameworkCore;' $FILE && sed -i 's/public class SqlservDbContext/public class SqlservDbContext : DbContext/' $FILE && sed -i '/^}$/i \\    public SqlservDbContext(DbContextOptions<SqlservDbContext> options) : base(options) { }' $FILE\ncd ../..\n";        
        var createDataLayerProjectScripts =
            "dotnet new classlib --name $name.DataAccess\ndotnet sln $name.sln add $name.DataAccess\ncd $name.DataAccess\nproj_version=\"net8.0\"\nfind . -name \"$name.DataAccess.csproj\" | while read -r file; do\n    if [ -f \"$file\" ]; then\n        sed -i \"s|<TargetFramework>.*</TargetFramework>|<TargetFramework>$proj_version</TargetFramework>|g\" \"$file\"\n    fi\ndone\nmkdir DatabaseContext\ncd ..\n";
        var currentScript = start + createDataLayerProjectScripts + sqlservScript + end;
        Script.Instance.Initialize(currentScript, Architecture.NLayer);
        var newScript = "cd $name.Web\ndotnet add package Microsoft.EntityFrameworkCore.Tools --version 8.0.20\n#specific_provider\ndotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 8.0.20\ndotnet add reference ../$name.Infrastructure/$name.Infrastructure.csproj\nsed -i \"1s|^|using $name.Infrastructure;\\n|\" Program.cs\nsed -i \"1s|^|using Microsoft.EntityFrameworkCore;\\n|\" Program.cs\n#specific_provider\nsed -i \"/var builder = WebApplication.CreateBuilder(args);/a \\\\nbuilder.Services.AddDbContext<SqliteDbContext>(options => options.UseSqlite(builder.Configuration[\\\"SqliteConnectionStrings:DefaultConnection\\\"]));\" Program.cs\n#specific_provider\nAPPSETTINGS=\"appsettings.json\" && APPSETTINGS_DEV=\"appsettings.Development.json\" && NEW_CONNECTION='  \"SqliteConnectionStrings\": {\\n    \"DefaultConnection\": \"Data Source=LocalDatabase.db\"\\n  },' && sed -i \"/^{/a $NEW_CONNECTION\" \"$APPSETTINGS\" && sed -i \"/^{/a $NEW_CONNECTION\" \"$APPSETTINGS_DEV\"\ncd ../$name.Infrastructure\ndotnet add package Microsoft.EntityFrameworkCore --version 8.0.20\ncd DatabaseContext\n#specific_provider\ndotnet new class -n SqliteDbContext --project ../$name.Infrastructure.csproj\n#specific_provider\nFILE=SqliteDbContext.cs && sed -i '1s/^\\xEF\\xBB\\xBF//' $FILE && sed -i '1i using Microsoft.EntityFrameworkCore;' $FILE && sed -i 's/public class SqliteDbContext/public class SqliteDbContext : DbContext/' $FILE && sed -i '/^}$/i \\    public SqliteDbContext(DbContextOptions<SqliteDbContext> options) : base(options) { }' $FILE\ncd ../..\n";
        var triggerCommandLabel = "#specific_provider";
        Script.Instance.TryReplaceTriggerCommandsFromAnotherScriptInRegion(start, end, newScript, triggerCommandLabel);
        var expected = start + createDataLayerProjectScripts + newScript + end;
        var actual = Script.Instance.ScriptText.ToString();
        Assert.That(actual, Is.EqualTo(expected));
    }
}
