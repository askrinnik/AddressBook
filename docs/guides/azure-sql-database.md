# Recreate the Azure SQL database

This guide creates the `AddressBook` database again on an existing Azure SQL logical server,
with the Azure SQL Database free offer. Use it when the database was deleted and the server,
the App Service and the Static Web App still exist.

Run the commands in PowerShell 7 with the Azure CLI signed in (`az login`).

## Placeholders

| Placeholder | Meaning |
|-------------|---------|
| `<resource-group>` | resource group with the SQL server and the apps |
| `<sql-server>` | logical server name, without `.database.windows.net` |
| `<api-app>` | App Service that hosts `AddressBook.Api` |

```powershell
$Rg     = '<resource-group>'
$Server = '<sql-server>'
$ApiApp = '<api-app>'
$Db     = 'AddressBook'   # Database= in ConnectionStrings:DefaultConnection
```

## Before you start: do not start the API first

The API applies EF Core migrations on every start (`ExecuteDatabaseMigration`), and
`Database.Migrate()` creates a missing database. On Azure SQL it creates it with the default
paid tier, not with the free offer. Create the database by hand first (step 2), then start the API.

If the API is running, stop it until step 2 is done:

```powershell
az webapp stop -g $Rg -n $ApiApp
```

## 1. Check the free offer quota

The free offer limit is per subscription and shared by all regions and projects.
Some subscriptions have only one free database.

```powershell
$sub      = az account show --query id -o tsv
$location = az sql server show -g $Rg -n $Server --query location -o tsv
az rest --method get `
  --url "https://management.azure.com/subscriptions/$sub/providers/Microsoft.Sql/locations/$location/usages" `
  --uri-parameters api-version=2021-11-01 `
  --query "value[?name=='FreeLimitQuota'].{used:properties.currentValue, limit:properties.limit}" -o table
```

Expected result: `Used` is less than `Limit`.

If `Used` equals `Limit`, another database uses the free offer. Find it:

```powershell
foreach ($s in az sql server list -o json | ConvertFrom-Json) {
  az sql db list -g $s.resourceGroup -s $s.name `
    --query "[?useFreeLimit].{server:'$($s.name)', group:'$($s.resourceGroup)', database:name}" -o table
}
```

Delete that database, or create `AddressBook` without the free offer (paid).

## 2. Create the database

These settings repeat the original database: General Purpose serverless, Gen5, 0.5–2 vCores,
free offer, pause instead of billing when the monthly free limit is used up.

```powershell
az sql db create -g $Rg -s $Server -n $Db `
  --edition GeneralPurpose --family Gen5 --capacity 2 --min-capacity 0.5 `
  --compute-model Serverless `
  --use-free-limit --free-limit-exhaustion-behavior AutoPause `
  --auto-pause-delay 60 --backup-storage-redundancy Local
```

Expected result: JSON of the database with `"status": "Online"`.

Check:

```powershell
az sql db show -g $Rg -s $Server -n $Db `
  --query "{status:status, sku:currentSku.name, free:useFreeLimit, onLimit:freeLimitExhaustionBehavior}" -o table
```

Expected result: `GP_S_Gen5`, `True`, `AutoPause`.

If `az sql db create` fails with `InternalServerError`, the free offer quota is usually used up.
Go back to step 1.

## 3. Start the API and apply migrations

The API connects with SQL authentication. It reads `Database__Server`, `Database__User` and
`Database__Password` from the App Service settings; the database name comes from
`ConnectionStrings:DefaultConnection`. These settings live on the App Service and survive the
database deletion, so nothing changes there.

```powershell
az webapp start -g $Rg -n $ApiApp
az webapp restart -g $Rg -n $ApiApp
```

Open any API endpoint once. On start the API applies all migrations, including the seed data.
In the App Service log stream look for `The database migration has been completed`.

The App Service reaches the server through the firewall exception
**Allow Azure services and resources to access this server** (rule `AllowAllWindowsAzureIps`).
Check that it is still there:

```powershell
az sql server firewall-rule list -g $Rg -s $Server -o table
```

To connect from your own computer, add your IP:

```powershell
$myIp = (Invoke-RestMethod https://api.ipify.org).Trim()
az sql server firewall-rule create -g $Rg -s $Server -n my-ip --start-ip-address $myIp --end-ip-address $myIp
```

## Alternative: restore the deleted database

Azure keeps a deleted database restorable for the backup retention period (7 days by default).
A restore brings back the data, not only the schema and the seed.

```powershell
az sql db list-deleted -g $Rg -s $Server -o table
```

Restore it with **SQL server → Data management → Deleted databases** in the portal, or with
`az sql db restore --deleted-time <deletionDate> --time <deletionDate>`. Check the result with
the `az sql db show` command from step 2: a restored database may not keep the free offer.
