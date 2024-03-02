//// AUDIT SETUP
var collectionExists = db.getCollectionNames().indexOf("audits") > -1;

if (!collectionExists) {
    db.createCollection("audits");
    print("audits Collection created.");
} else {
    print("audits Collection already exists.");
}

var defaultAudits = [
    {
        "auditId": NumberInt(10000),
        "claimNumber": "123456789",
        "status": "Pending Review",
        "organization": "HealthCare 123",
        "date": ISODate("2024-02-24T00:00:00.000+0000"),
        "amount": NumberInt(1250000)
    },
    {
        "auditId": NumberInt(10001),
        "claimNumber": "112345678",
        "status": "Pending Approval",
        "organization": "Transparency Health",
        "date": ISODate("2024-02-25T00:00:00.000+0000"),
        "amount": NumberInt(125000)
    },
    {
        "auditId": NumberInt(10002),
        "claimNumber": "111234567",
        "status": "Reconciliation",
        "organization": "Optimal Health",
        "date": ISODate("2024-02-25T00:00:00.000+0000"),
        "amount": NumberInt(155500)
    },
    {
        "auditId": NumberInt(10003),
        "claimNumber": "111123456",
        "status": "Audit Letter Sent",
        "organization": "PI Services",
        "date": ISODate("2024-02-26T00:00:00.000+0000"),
        "amount": NumberInt(3650000)
    },
    {
        "auditId": NumberInt(10004),
        "claimNumber": "111112345",
        "status": "Pending Invoicing",
        "organization": "HealthCare 123",
        "date": ISODate("2024-02-29T00:00:00.000+0000"),
        "amount": NumberInt(12250000)
    },
    {
        "auditId": NumberInt(10005),
        "claimNumber": "111112234",
        "status": "Closed",
        "organization": "HealthCare 123",
        "date": ISODate("2024-01-22T00:00:00.000+0000"),
        "amount": NumberInt(1220000)
    },
    {
        "auditId": NumberInt(10006),
        "claimNumber": "112122934",
        "status": "Approved",
        "organization": "Optimal Health",
        "date": ISODate("2024-02-29T00:00:00.000+0000"),
        "amount": NumberInt(120000)
    },
    {
        "auditId": NumberInt(10007),
        "claimNumber": "111112239",
        "status": "Approved",
        "organization": "Optimal Health",
        "date": ISODate("2024-02-29T00:00:00.000+0000"),
        "amount": NumberInt(100000)
    },
    {
        "auditId": NumberInt(10008),
        "claimNumber": "111112235",
        "status": "Approved",
        "organization": "Optimal Health",
        "date": ISODate("2024-02-29T00:00:00.000+0000"),
        "amount": NumberInt(150000)
    },
    {
        "auditId": NumberInt(10009),
        "claimNumber": "121212233",
        "status": "Approved",
        "organization": "Transparency Health",
        "date": ISODate("2024-02-29T00:00:00.000+0000"),
        "amount": NumberInt(165000)
    }
];


db.getCollection("audits").insertMany(defaultAudits);

// STOREDQUERY SETUP

var collectionExists = db.getCollectionNames().indexOf("storedqueries") > -1;

if (!collectionExists) {
    db.createCollection("storedqueries");
    print("storedqueries Collection created.");
} else {
    print("storedqueries Collection already exists.");
}

var defaultStoredQueries = [
    {
        "name": "dailyAuditInventory",
        "query": "{ 'status' : { $in : [ 'Pending Review', 'Approved' ] } }"
    },
    {
        "name": "weeklyAuditInventory",
        "query": "{ 'status' : { $ne : 'Closed' } }"
    }
]

db.getCollection("storedqueries").insertMany(defaultStoredQueries);

// INTERVALPROCESSINGCONFIGS SETUP

var collectionExists = db.getCollectionNames().indexOf("intervalprocessingconfigs") > -1;

if (!collectionExists) {
    db.createCollection("intervalprocessingconfigs");
    print("intervalprocessingconfigs Collection created.");
} else {
    print("intervalprocessingconfigs Collection already exists.");
}

var defaultIntervalProcessingConfigs = [
    {
        "processes": [
            {
                "name": "DailyAuditInventoryProcessor",
                "configurations": {
                    "queryName": "dailyAuditInventory",
                    "collectionName": "audits",
                    "projection": "{}",
                    "fileNameBase": "DailyAuditInventory",
                    "writerTypeKey": "DailyAuditInventoryFileWriter"
                }
            },
            {
                "name": "WeeklyAuditInventoryProcessor",
                "configurations": {
                    "queryName": "weeklyAuditInventory",
                    "collectionName": "audits",
                    "projection": "{}",
                    "fileNameBase": "WeeklyAuditInventory",
                    "writerTypeKey": "WeeklyAuditInventoryFileWriter"
                }
            }
        ]
    }
]

db.getCollection("intervalprocessingconfigs").insertMany(defaultIntervalProcessingConfigs);