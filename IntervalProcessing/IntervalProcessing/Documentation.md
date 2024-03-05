# What is Interval Processing?

Simply put - this is a server-side c#/.net application that runs a series of processes in a specific order, that can be configured to run at any time intervals with a cloud service of choice.

In the scenario that this project is built for, the solution serves a Health Care based company operating in the payment integrity space. If you are unfamiliar with the term, it is generally used for operations dedicated to the recovery of funds that have been overpayed in claims by health care insurance companies to the various providers in their network. If simple blood work gets accidentally coded as a broken leg treatment, for instance, there is likely a dramatic difference in cost. If you are an insurance company that paid that incorrect amount, you may want to see those funds recovered. The problem is that this is a messy process that is filled with regulations and requirements, and there are precious few standards across the industry.

There are **many organizations** dedicated to performing this work for insurance payers, who run whole operations on health care data.

In this example project, a company has built a platform by which they manage this work. They operate out of a mongo database that serves up data to a UI that users use to ease the burdeon of working with such cumbersome data. This example works with the following:

- **audits**: this represents the work done on a health care claim where funds are recovered
- **medical record requests**: this represents the process by which organizations request and recieve a medical record for a claim, that is used to validate the pursuit of a recovery of overpaid funds

Users tag claims for audit and for pursuit of medical records, and they build an **inventory** for their auditors to work through.

## What are the **Requirements**?

**The general requirements of this process** are to build a system where:
1. files can be generated
2. files can be easily modified, as users have constantly evolving needs/requests
3. the content of the files can be easily modified to include or exclude data conditionally
4. data can be manipulated without building a file, i.e. audit/mrr statuses can change without user interaction
5. the structure of the data can change unexpectedly

**Why use these requirements?**

In my experience, working with various organizations leads to a lot of customization requests. Your leadership may be very open to seemingly excessive amounts of customization, or they may be very inflexible to those types of requests. Regardless, a system can be developed to alleviate those pain points to a decent degree, and your product or implementation cycle may benefit from goodwill gained by being able to accomodate *x amount* of requests.

**Examples of common customization request scenarios**:
1. An organization wants a different name for themselves to appear on the inventory file than what it may be in the system - Ex: 'Transparency Health' wants 'Clear View Health Services' to appear on one specific file because they forward that to a different organization that they work for that requires the name to be that.
2. An organization feeds their inventory files into an internally owned system that requires extra fields with specific values - Ex: add a 'Resource Code' and 'External System Org Id' column to *x* files because out system requires it with *y* and *z* values.
3. An organization needs the full address for a provider in a singular column, despite it being 5 different data points

The specific processes within this system need to be designed to handle these types of requests in a clean, maintainable, and modular way, whereby more changes can happen regularly.

**Here are the specific requirements**:
1. **Daily Audit Inventory**: Every day, over night, generate a file of all audits in the "Pending Review" or "Approved" statuses. Write their "Audit Id", "Claim Number", "Status", "Assigned Organization", "Audit Date", and "Audit Amount" to a pipe-delimited text file. The file needs to be titled "DailyAuditInventory_MMDDYYYY_hhmmss.txt", with the date and time populated to the title in the appropriate places.
2. **Weekly Audit Inventory**: Every Friday, over night between Thursday and Friday, generate a file of all audits NOT in the "Closed" status. Write their "Audit Id", "Claim Number", "Status", "Assigned Organization", "Audit Date", and "Audit Amount" to a pipe-delimited text file. To each row, add two additional fields: "Resource Code" and "External System Org Id" populated every time with the values "RCP-1039bc0" and "8b10YTG" respectively. The file needs to be titled "WeeklyAuditInventory_MMDDYYYY_hhmmss.txt", with the date and time populated to the title in the appropriate places.
3. **Daily Active MRR Inventory**: Every day, over night, generate a file of all medical record requests NOT in the "Closed" status. Write their "Request Id", "Claim Number", "Provider Number", "Provider Name", "Status", "Assigned Organization", and "Request Date" to a pipe-delimited text file. The file needs to be titled "DailyActiveMRRInventory_MMDDYYYY_hhmmss.txt", with the date and time populated to the title in the appropriate places. Additionally, and request for the organization "Transparency Health" need to be reflected as "Clear View Health Services" in the "Assigned Organization" column.
4. **Weekly MRR Inventory**: Every Friday, over night between Thursday and Friday, generate a file of all medical record requests regardless of status. Write their "Request Id", "Claim Number", "Provider Number", "Provider Name", "Provider Full Address", "Status", "Assigned Organization", and "Request Date" to a pipe-delimited text file. The file needs to be titled "WeeklyMRRInventory_MMDDYYYY_hhmmss.txt", with the date and time populated to the title in the appropriate places. The "Provider Full Address" column needs to be the providers full address (address1, address2, city, state, and zip) together in one column, seperated by a space. There should not be a space if there isnt a data point value for one of the address properties.

## The System Designed

To achieve these requirements, the following features were selected:
1. **Specific Design Patterns**: Template Method, Factory, Singleton
2. **Database Configuration Management**
3. **MongoDB Integration**
4. **Dependency Injection**

Lets delve into why:

### Design Patterns

The **Template Method** design pattern really helps fascilitate customization without sprawling the codebase or creating a lot of repeating code. We very commonly used this in previous roles. How does it work here? Well, we need to write several files. Fortunately they are the same format (pipe-delimited text) but they have differing header columns, different data inputs (audits vs medical record requests), and clients requested custom elements to a few (org name changes, address concatenation, extra columns with hard coded values). Ultimately a File needs to be generated, and needs to be written to. So a BaseWriter abstract class was generated to handle to core functionality of opening a StreamWriter, closing it properly, and writing the lines.

```csharp
public abstract class BaseWriter<T> : IWriter<T>
```
The generic allows for that variable data source, and the abstract methods:

```csharp
protected abstract string Parse(T input);

protected abstract string GetHeader();
```
give the flexibility of custom columns, writing data points, seen here in **DailyAuditInventoryFileWriter**

```csharp
public class DailyAuditInventoryFileWriter<T> : BaseWriter<T> where T : BsonDocument
{
    protected override string Parse(T document) 
    { 
        StringBuilder builder = new StringBuilder();
        builder.Append(document["auditId"].ToString());
        builder.Append(_del);

        builder.Append(document["claimNumber"].AsString);
        builder.Append(_del);

        ...
        return builder.ToString();
    }

    protected override string GetHeader() 
    {
        StringBuilder builder = new StringBuilder();
        builder.Append("Audit Id");
        builder.Append(_del);

        builder.Append("Claim Number");
        builder.Append(_del);

        ...

        return builder.ToString();
    }
}
```

Additionally, the processor was given the same treatment.

The BaseFileGenerationProcessor was created to wrangle the disparate elements of retrieving the data from the db and running that data to the file, then closing the file.

```csharp
public abstract class BaseFileGenerationProcessor : IFileProcessor
{
    ...

    public async Task Execute()
    {
        ...

        _fileWriter = _writerFactory.CreateWriter(_settings.WriterTypeKey, workingFile);

        MongoCursor cursor = new MongoCursor(query, _settings.Projection, _settings.CollectionName, _connection);

        ...
        await cursor.ExecuteCursor(WriteDataToFile);
        ...

        _fileWriter.Close();

        ...
    }

    private void WriteDataToFile(BsonDocument document)
    {
        _fileWriter?.Write(ApplyCustomLogic(document));
    }

    public abstract BsonDocument ApplyCustomLogic(BsonDocument document);
}
```

**ApplyCustomLogic** above is opened up as the place to modify the data as needed before writing to the file, seen here in WeeklyAuditInventoryProcessor:

```csharp
public class WeeklyAuditInventoryProcessor : BaseFileGenerationProcessor
{
    ...
    public override BsonDocument ApplyCustomLogic(BsonDocument document)
    {
        BsonDocument customizedDoc = document.DeepClone().AsBsonDocument;

        customizedDoc.Add("resourceCode", "RCP-1039bc0");
        customizedDoc.Add("externalId", "8b10YTG");
        
        return customizedDoc;
    }
}
```
In both of these examples, the Base abstract class acts as a super class to add some level of conformity around the elements that should not be customizable, while the inheriting sub classes utilize their playground methods to perform their custom logic (configuring the data in the processor, building the lines that are written as headers and rows).

Aside from this, factories are lightly used assign the correct file writer instance to the calling process but ultimately one was not needed for the processors since they could be called directly from the transient pool due to their uniqueness.

That brings us around to Dependency Injection. I elected to use DI for this project because I percieve it as more of the standard by which these types of processes are written today. In my previous experience a more dotnet framework architecture was utilized, and we suffered through it. We had major issues around decouping and spawling layers of inefficiency due to needing to instantiate new instances of classes 'all over tarnation' in the applications. Imagine having 20-30 synchronous database connections when it could have been a singleton that self-managed what collection it read from. So with that, I designed the database connection class to be flixible to collection changes (mongo collections, specifically), and built some manager classes that handles changing the collection and pulling the relevant data to the application:

```csharp
public class MongoConnection<T> : IMongoConnection<T>
{
    ...

    public void SetDatabase(string databaseName)
    {
        _databaseName = databaseName;
        Database = Client.GetDatabase(_databaseName);
    }

    public void SetCollection(string collectionName)
    {
        _collectionName = collectionName;
        Collection = Database.GetCollection<T>(_collectionName);
    }
}

public class StoredQueryManager : IStoredQueryManager
{
    private IMongoConnection<BsonDocument> _connection;

    public StoredQueryManager(IMongoConnection<BsonDocument> connection)
    {
        _connection = connection;
    }

    public async Task<string> GetQueryAsync(string queryName)
    {
        _connection.SetCollection(StoredQueries);

        FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("name", queryName);

        BsonDocument result = await _connection.Collection.Find(filter).FirstOrDefaultAsync();

        return result != null ? result["query"].AsString : string.Empty;
    }
}

public class FileProcessorConfigManager : IFileProcessorConfigManager
{
    private IMongoConnection<BsonDocument> _connection;

    public Dictionary<string, FileProcessorSpecification> Settings { get; private set; }

    public FileProcessorConfigManager(IMongoConnection<BsonDocument> connection)
    {
        _connection = connection;
    }

    private async Task<Dictionary<string, FileProcessorSpecification>> GetSettingsAsync()
    {
        if (Settings == null)
        {
            _connection.SetCollection(IntervalProcessingConfigs);

            BsonDocument result = (await _connection.Collection.FindAsync(BsonSerializer.Deserialize<BsonDocument>("{}"))).FirstOrDefault();

            try
            {
                FileProcessorConfig allSettings = BsonSerializer.Deserialize<FileProcessorConfig>(result);

                return Settings = allSettings.Processes
                    .Where(setting => setting.Name != null && setting.Configurations != null)
                    .GroupBy(setting => setting.Name!)
                    .ToDictionary(
                        group => group.Key,
                        group => group.First().Configurations!
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deserializing FileProcessorConfig " + ex.ToString());
                throw;
            }
        }
        else
        {
            return Settings;
        }
    }

    public async Task<FileProcessorSpecification> GetFileProcessorSpecification(Type processorType)
    {
        if (Settings == null)
        {
            await GetSettingsAsync();
        }

        return Settings.ContainsKey(processorType.Name) ? Settings[processorType.Name] : throw new KeyNotFoundException();
    }
}
```
This gets into the configuration elements of the design, and truthfully im not immensely happy with these classes but they work for what is needed here well, and the Settings element in the class above illustrates coding the process for singleton use because it gets the data once when its first called, and then doesnt needlessly hit the database again when its needed in other file processes. Because this project is modular, it works well as a singleton here since only file generation based processes will interact with it.

### Database Configuration Management

For configs, I leaned heavily into database configurations. I have experience with both database and configuration file (appsettings.json/config.json) approaches and I find database configs to be the shortest and easiest win to handling requests like "can we add this to the file name?" and "can we exclude this status from this file now?". It allows for quick easy changes without needing to change and redeploy the code, making for fast turnarounds and quick wins when they might be needed.

I created two mongo collections for configuration for this project: "storedqueries" and "intervalprocessingconfigs". storedqueries retains the actual mongo database query, stringified, with a name key. intervalprocessingconfigs houses objects that hold values and keys tied to the processes they represent.

```json
{
    "name" : "dailyAuditInventory",
    "query" : "{ 'status' : { $in : [ 'Pending Review', 'Approved' ] } }"
}
{
    "name" : "weeklyAuditInventory",
    "query" : "{ 'status' : { $ne : 'Closed' } }"
}
{
    "name" : "dailyActiveMRRInventory",
    "query" : "aggregate([{$match:{'status':{$ne:'Closed'}}},{$lookup:{from:'providers',localField:'provider_id',foreignField:'_id',as:'provider'}},{$unwind:'$provider'}])"
}
{
    "name" : "weeklyMRRInventory",
    "query" : "aggregate([{$lookup:{from:'providers',localField:'provider_id',foreignField:'_id',as:'provider'}},{$unwind:'$provider'}])"
}
```
as you can see above, both standard and aggregate queries are supported. This offers an insane amount of flexibility in what can be represented in the data - as there isnt really much that *cant* be done with an aggregate. Its used sparingly here just to return joined data for medicalrecordrequests, and no joins were needed for the audit files. The benefit here is that the query can be easily modified to fascilite customization requests.
```json
{
    "processes" : [
        {
            "name" : "DailyAuditInventoryProcessor",
            "configurations" : {
                "queryName" : "dailyAuditInventory",
                "collectionName" : "audits",
                "projection" : "{}",
                "fileNameBase" : "DailyAuditInventory",
                "writerTypeKey" : "DailyAuditInventoryFileWriter"
            }
        },
        {
            "name" : "WeeklyAuditInventoryProcessor",
            "configurations" : {
                "queryName" : "weeklyAuditInventory",
                "collectionName" : "audits",
                "projection" : "{}",
                "fileNameBase" : "WeeklyAuditInventory",
                "writerTypeKey" : "WeeklyAuditInventoryFileWriter"
            }
        },
        {
            "name" : "DailyActiveMRRInventoryProcessor",
            "configurations" : {
                "queryName" : "dailyActiveMRRInventory",
                "collectionName" : "medicalrecordrequests",
                "projection" : "{}",
                "fileNameBase" : "DailyActiveMRRInventory",
                "writerTypeKey" : "DailyActiveMRRInventoryFileWriter"
            }
        },
        {
            "name" : "WeeklyMRRInventoryProcessor",
            "configurations" : {
                "queryName" : "weeklyMRRInventory",
                "collectionName" : "medicalrecordrequests",
                "projection" : "{}",
                "fileNameBase" : "WeeklyMRRInventory",
                "writerTypeKey" : "WeeklyMRRInventoryFileWriter"
            }
        }
    ]
}
```
above here we have the file processing configs, housing the key for the query, a custom projection (efficiency gain here), the relevant collection for the connection, file base name for what gets generated as the filename, and a key for the writer factory. the name is the actual class name of the processor, ensuring no mishaps around configurations going to the wrong processor.

### MongoDB Integration

So why mongo? This is a question I get a ton. Truthfully, I've used it for 5 years and grown quite fond of its strengths and leery of its weaknesses. It was primarily used because of its schema flexibility - it doesnt require one for collections - and this can be a boon to a project that may have many moving parts around data point additions, or when you are at the start of a project and dont yet know where its going to grow to. Mongo auto scales nicely, so it works well with large samples of data, but is quite expensive.

Ultimately, mongo is 'spiderman tech' to me - "With great power comes great responsibility..." and unfortunately I haven't seen it used responsibly. I've seen megastructures and data needlessly nested and the subsequent datafixes required to update field values because of this. In this project I seek to use it responsibly, and for the very real reason that the MongoDB nuget packages are quite good. They offer reflection incarnate in the form of the 'BsonDocument' class, allowing for an incredible amount of data flexibility. To be clear, this is dangerous. On the other hand, it allowed me to use the BsonDocument class instead of 4 custom models to handle needing to add data points for the file, manipulate the address, etc. This is helpful for this project, but the data points ultimately need to be wrapped in an extension method that ensures the property is there to avoid unexpected exceptions.

### Dependency Injection
Finally, the piece that ties everything together is the use of dependency injection. The services are setup in the Program file here:

```csharp
private static void ConfigureServices(IServiceCollection services) 
{
    // configuration settings
    CoreConfig coreConfig = new CoreConfig("config.json");
    services.AddSingleton<IConfig>(coreConfig);

    // database connection
    services.AddSingleton<IMongoConnection<BsonDocument>>(provider =>
        new MongoConnection<BsonDocument>(provider.GetRequiredService<IConfig>(), StoredQueries));

    // factory and manager singletons
    services.AddSingleton<IWriterFactory, WriterFactory>();
    services.AddSingleton<IStoredQueryManager, StoredQueryManager>();
    services.AddSingleton<IFileProcessorConfigManager, FileProcessorConfigManager>();

    // processor transients
    services.AddTransient<DailyAuditInventoryProcessor>();
    services.AddTransient<WeeklyAuditInventoryProcessor>();
    services.AddTransient<DailyActiveMRRInventoryProcessor>();
    services.AddTransient<WeeklyMRRInventoryProcessor>();

    // app transient
    services.AddTransient<App>();
}
```

that then get called properly in App.cs:

```csharp
public async Task Run() 
{
    DailyAuditInventoryProcessor dailyAuditInventoryProcessor = (DailyAuditInventoryProcessor)_serviceProvider.GetService(typeof(DailyAuditInventoryProcessor));
    await dailyAuditInventoryProcessor?.Execute();

    DailyActiveMRRInventoryProcessor dailyActiveMRRInventoryProcessor = (DailyActiveMRRInventoryProcessor)_serviceProvider.GetService(typeof(DailyActiveMRRInventoryProcessor));
    await dailyActiveMRRInventoryProcessor?.Execute();

    if (DateTime.Now.DayOfWeek == DayOfWeek.Friday) 
    { 
        WeeklyAuditInventoryProcessor weeklyAuditInventoryProcessor = (WeeklyAuditInventoryProcessor)_serviceProvider.GetService(typeof(WeeklyAuditInventoryProcessor));
        await weeklyAuditInventoryProcessor?.Execute();
            
        WeeklyMRRInventoryProcessor weeklyMRRInventoryProcessor = (WeeklyMRRInventoryProcessor)_serviceProvider.GetService(typeof(WeeklyMRRInventoryProcessor));
        await weeklyMRRInventoryProcessor?.Execute();
    }
}
```

The database connection, configurations, and factory all are set as singletons because they do not need new instances to be used by these processes, cutting down on needly database querying since these processes run back to back rather than parallel. The Processors are all Transients because they need new instances to be used per call

## How It All Comes Together

The above requirements are highly representative of actual work request types very common in previous roles. As new clients would be implemented, and new requests would fly in with those, we didnt have the benefit of planning and architecting something scalable because we already had existing applications that we had to fit the mold of, and no technical debt budget/generous implementation timeline to accomodate a true rewrite. Naturally, this was a frustrating experience. Every dev I've had the pleasure of working with has had one thing in common: they want to build something the right way. The reality is that business often does not afford this, so understand that this little side project/example is really coming from a place of privaledge. It did not have a deadline. There is no monetization situation here. Peoples' livelihoods did not depend on this project completing by *x* date. And this project isnt complete - it only represents one specific scenario among dozens that I've spend the last 5 years tackling.
