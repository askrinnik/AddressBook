# Повторное создание базы данных Azure SQL

Это руководство заново создаёт базу данных `AddressBook` на существующем логическом сервере
Azure SQL с бесплатным предложением Azure SQL Database (free offer). Используйте его, если базу
данных удалили, а сервер, App Service и Static Web App остались.

Выполняйте команды в PowerShell 7. Azure CLI должен быть авторизован (`az login`).

## Заполнители

| Заполнитель | Значение |
|-------------|----------|
| `<resource-group>` | группа ресурсов с SQL-сервером и приложениями |
| `<sql-server>` | имя логического сервера без `.database.windows.net` |
| `<api-app>` | App Service, в котором работает `AddressBook.Api` |

```powershell
$Rg     = '<resource-group>'
$Server = '<sql-server>'
$ApiApp = '<api-app>'
$Db     = 'AddressBook'   # Database= in ConnectionStrings:DefaultConnection
```

## Перед началом: не запускайте API первым

API применяет миграции EF Core при каждом запуске (`ExecuteDatabaseMigration`), а
`Database.Migrate()` создаёт базу данных, если её нет. В Azure SQL такая база получает платный
уровень по умолчанию, а не бесплатное предложение. Поэтому сначала создайте базу данных вручную
(шаг 2), а потом запускайте API.

Если API работает, остановите его до конца шага 2:

```powershell
az webapp stop -g $Rg -n $ApiApp
```

## 1. Проверьте квоту бесплатного предложения

Лимит бесплатного предложения действует на подписку. Его делят все регионы и проекты.
В некоторых подписках доступна только одна бесплатная база данных.

```powershell
$sub      = az account show --query id -o tsv
$location = az sql server show -g $Rg -n $Server --query location -o tsv
az rest --method get `
  --url "https://management.azure.com/subscriptions/$sub/providers/Microsoft.Sql/locations/$location/usages" `
  --uri-parameters api-version=2021-11-01 `
  --query "value[?name=='FreeLimitQuota'].{used:properties.currentValue, limit:properties.limit}" -o table
```

Ожидаемый результат: `Used` меньше `Limit`.

Если `Used` равно `Limit`, бесплатное предложение занимает другая база данных. Найдите её:

```powershell
foreach ($s in az sql server list -o json | ConvertFrom-Json) {
  az sql db list -g $s.resourceGroup -s $s.name `
    --query "[?useFreeLimit].{server:'$($s.name)', group:'$($s.resourceGroup)', database:name}" -o table
}
```

Удалите эту базу данных или создайте `AddressBook` без бесплатного предложения (платно).

## 2. Создайте базу данных

Эти параметры повторяют исходную базу данных: General Purpose serverless, Gen5, 0,5–2 vCore,
бесплатное предложение. Когда месячный бесплатный лимит исчерпан, база приостанавливается,
а не начинает списывать деньги.

```powershell
az sql db create -g $Rg -s $Server -n $Db `
  --edition GeneralPurpose --family Gen5 --capacity 2 --min-capacity 0.5 `
  --compute-model Serverless `
  --use-free-limit --free-limit-exhaustion-behavior AutoPause `
  --auto-pause-delay 60 --backup-storage-redundancy Local
```

Ожидаемый результат: JSON базы данных с `"status": "Online"`.

Проверьте:

```powershell
az sql db show -g $Rg -s $Server -n $Db `
  --query "{status:status, sku:currentSku.name, free:useFreeLimit, onLimit:freeLimitExhaustionBehavior}" -o table
```

Ожидаемый результат: `GP_S_Gen5`, `True`, `AutoPause`.

Если `az sql db create` завершается с ошибкой `InternalServerError`, обычно это значит, что квота
бесплатного предложения исчерпана. Вернитесь к шагу 1.

## 3. Запустите API и примените миграции

API подключается с SQL-аутентификацией. Он читает `Database__Server`, `Database__User` и
`Database__Password` из настроек App Service. Имя базы данных берётся из
`ConnectionStrings:DefaultConnection`. Эти настройки хранятся в App Service и не пропадают при
удалении базы данных, поэтому там ничего менять не нужно.

```powershell
az webapp start -g $Rg -n $ApiApp
az webapp restart -g $Rg -n $ApiApp
```

Откройте любой эндпоинт API один раз. При запуске API применяет все миграции, включая
начальные данные (seed). В потоке логов App Service найдите строку
`The database migration has been completed`.

App Service подключается к серверу через исключение брандмауэра
**Allow Azure services and resources to access this server** (правило `AllowAllWindowsAzureIps`).
Проверьте, что оно на месте:

```powershell
az sql server firewall-rule list -g $Rg -s $Server -o table
```

Чтобы подключаться со своего компьютера, добавьте свой IP-адрес:

```powershell
$myIp = (Invoke-RestMethod https://api.ipify.org).Trim()
az sql server firewall-rule create -g $Rg -s $Server -n my-ip --start-ip-address $myIp --end-ip-address $myIp
```

## Альтернатива: восстановите удалённую базу данных

Azure позволяет восстановить удалённую базу данных в течение срока хранения резервных копий
(по умолчанию 7 дней). Восстановление возвращает данные, а не только схему и начальные данные.

```powershell
az sql db list-deleted -g $Rg -s $Server -o table
```

Восстановите её на портале через **SQL server → Data management → Deleted databases** или командой
`az sql db restore --deleted-time <deletionDate> --time <deletionDate>`. Проверьте результат
командой `az sql db show` из шага 2: восстановленная база данных может потерять бесплатное
предложение.
