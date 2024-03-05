// AUDIT SETUP
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

// PROVIDER SETUP

var collectionExists = db.getCollectionNames().indexOf("providers") > -1;

if (!collectionExists) {
    db.createCollection("providers");
    print("providers Collection created.");
} else {
    print("providers Collection already exists.");
}

var defaultProviders = [
    {
        "_id": ObjectId("65e41d6baafcb118050aa288"),
        "number": "PRN12345677",
        "address": {
            "address1": "123 Street Rd",
            "address2": "STE 300",
            "city": "Nashville",
            "state": "TN",
            "zip": "37209"
        },
        "name": "General Hospital"
    },
    {
        "_id": ObjectId("65e62deb91d3f5efc296cae2"),
        "number": "PRN12345678",
        "address": {
            "address1": "456 Avenue Ln",
            "address2": "Suite 101",
            "city": "Memphis",
            "state": "TN",
            "zip": "38103"
        },
        "name": "City Medical Center"
    },
    {
        "_id": ObjectId("65e62deb91d3f5efc296cae3"),
        "number": "PRN12345679",
        "address": {
            "address1": "789 Boulevard St",
            "address2": "",
            "city": "Knoxville",
            "state": "TN",
            "zip": "37902"
        },
        "name": "Urban Health Clinic"
    },
    {
        "_id": ObjectId("65e62deb91d3f5efc296cae4"),
        "number": "PRN12345680",
        "address": {
            "address1": "101 Parkway Ave",
            "address2": "Suite 201",
            "city": "Chattanooga",
            "state": "TN",
            "zip": "37402"
        },
        "name": "Riverside Hospital"
    },
    {
        "_id": ObjectId("65e62deb91d3f5efc296cae5"),
        "number": "PRN12345681",
        "address": {
            "address1": "202 Main St",
            "address2": "",
            "city": "Clarksville",
            "state": "TN",
            "zip": "37040"
        },
        "name": "Sunrise Medical Group"
    },
    {
        "_id": ObjectId("65e62deb91d3f5efc296cae6"),
        "number": "PRN12345682",
        "address": {
            "address1": "303 Elm Rd",
            "address2": "Floor 2",
            "city": "Murfreesboro",
            "state": "TN",
            "zip": "37129"
        },
        "name": "Cedar Tree Family Practice"
    },
    {
        "_id": ObjectId("65e62deb91d3f5efc296cae7"),
        "number": "PRN12345683",
        "address": {
            "address1": "404 Oak Ln",
            "address2": "Suite B",
            "city": "Franklin",
            "state": "TN",
            "zip": "37067"
        },
        "name": "Health & Wellness Medical Center"
    },
    {
        "_id": ObjectId("65e62deb91d3f5efc296cae8"),
        "number": "PRN12345684",
        "address": {
            "address1": "505 Pine St",
            "address2": "STE 602",
            "city": "Murfreesboro",
            "state": "TN",
            "zip": "37129"
        },
        "name": "Pine Street Pediatrics"
    },
    {
        "_id": ObjectId("65e62deb91d3f5efc296cae9"),
        "number": "PRN12345685",
        "address": {
            "address1": "606 Maple Ave",
            "address2": "",
            "city": "Murfreesboro",
            "state": "TN",
            "zip": "37129"
        },
        "name": "Maple Avenue Family Clinic"
    },
    {
        "_id": ObjectId("65e62deb91d3f5efc296caea"),
        "number": "PRN12345686",
        "address": {
            "address1": "707 Cherry Blvd",
            "address2": "Ste 500",
            "city": "Franklin",
            "state": "TN",
            "zip": "37067"
        },
        "name": "Cherry Boulevard Surgical Center"
    }
];

db.getCollection("providers").insertMany(defaultProviders);

// MEDICALRECORDREQUESTS SETUP

var collectionExists = db.getCollectionNames().indexOf("medicalrecordrequests") > -1;

if (!collectionExists) {
    db.createCollection("medicalrecordrequests");
    print("medicalrecordrequests Collection created.");
} else {
    print("medicalrecordrequests Collection already exists.");
}

var defaultMRRs = [
    {
        "claimNumber": "123456789",
        "status": "Awaiting Response",
        "organization": "HealthCare 123",
        "date": ISODate("2024-02-24T00:00:00.000+0000"),
        "provider_id": ObjectId("65e41d6baafcb118050aa288"),
        "mrrId": "MR100000"
    },
    {
        "claimNumber": "112345678",
        "status": "Pending Letter Generation",
        "organization": "Transparency Health",
        "date": ISODate("2024-02-25T00:00:00.000+0000"),
        "provider_id": ObjectId("65e62deb91d3f5efc296cae2"),
        "mrrId": "MR100001"
    },
    {
        "claimNumber": "111234567",
        "status": "Pending Letter Generation",
        "organization": "Optimal Health",
        "date": ISODate("2024-02-25T00:00:00.000+0000"),
        "provider_id": ObjectId("65e62deb91d3f5efc296cae3"),
        "mrrId": "MR100002"
    },
    {
        "claimNumber": "111123456",
        "status": "Pending Letter Generation",
        "organization": "PI Services",
        "date": ISODate("2024-02-26T00:00:00.000+0000"),
        "provider_id": ObjectId("65e62deb91d3f5efc296cae4"),
        "mrrId": "MR100003"
    },
    {
        "claimNumber": "111112345",
        "status": "Awaiting Response",
        "organization": "HealthCare 123",
        "date": ISODate("2024-02-29T00:00:00.000+0000"),
        "provider_id": ObjectId("65e62deb91d3f5efc296cae5"),
        "mrrId": "MR100004"
    },
    {
        "claimNumber": "111112234",
        "status": "Awaiting Response",
        "organization": "HealthCare 123",
        "date": ISODate("2024-01-22T00:00:00.000+0000"),
        "provider_id": ObjectId("65e62deb91d3f5efc296cae6"),
        "mrrId": "MR100005"
    },
    {
        "claimNumber": "112122934",
        "status": "Provider Non Response",
        "organization": "Optimal Health",
        "date": ISODate("2024-02-29T00:00:00.000+0000"),
        "provider_id": ObjectId("65e62deb91d3f5efc296cae7"),
        "mrrId": "MR100006"
    },
    {
        "claimNumber": "111112239",
        "status": "Provider Non Response",
        "organization": "Optimal Health",
        "date": ISODate("2024-02-29T00:00:00.000+0000"),
        "provider_id": ObjectId("65e62deb91d3f5efc296cae8"),
        "mrrId": "MR100007"
    },
    {
        "claimNumber": "111112235",
        "status": "Provider Non Response",
        "organization": "Optimal Health",
        "date": ISODate("2024-02-29T00:00:00.000+0000"),
        "provider_id": ObjectId("65e62deb91d3f5efc296cae9"),
        "mrrId": "MR100008"
    },
    {
        "claimNumber": "121212233",
        "status": "Closed",
        "organization": "Transparency Health",
        "date": ISODate("2024-02-29T00:00:00.000+0000"),
        "provider_id": ObjectId("65e62deb91d3f5efc296caea"),
        "mrrId": "MR100009"
    }
];

db.getCollection("medicalrecordrequests").insertMany(defaultMRRs);

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
    },
    {
        "name": "dailyActiveMRRInventory",
        "query": "aggregate([{$match:{'status':{$ne:'Closed'}}},{$lookup:{from:'providers',localField:'provider_id',foreignField:'_id',as:'provider'}},{$unwind:'$provider'}])"
    },
    {
        "name": "weeklyMRRInventory",
        "query": "aggregate([{$lookup:{from:'providers',localField:'provider_id',foreignField:'_id',as:'provider'}},{$unwind:'$provider'}])"
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
            },
            {
                "name": "DailyActiveMRRInventoryProcessor",
                "configurations": {
                    "queryName": "dailyActiveMRRInventory",
                    "collectionName": "medicalrecordrequests",
                    "projection": "{}",
                    "fileNameBase": "DailyActiveMRRInventory",
                    "writerTypeKey": "DailyActiveMRRInventoryFileWriter"
                }
            },
            {
                "name": "WeeklyMRRInventoryProcessor",
                "configurations": {
                    "queryName": "weeklyMRRInventory",
                    "collectionName": "medicalrecordrequests",
                    "projection": "{}",
                    "fileNameBase": "WeeklyMRRInventory",
                    "writerTypeKey": "WeeklyMRRInventoryFileWriter"
                }
            }
        ]
    }
]

db.getCollection("intervalprocessingconfigs").insertMany(defaultIntervalProcessingConfigs);