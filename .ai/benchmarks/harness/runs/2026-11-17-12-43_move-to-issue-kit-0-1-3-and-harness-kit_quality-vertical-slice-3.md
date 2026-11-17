**Коротко:** Email добавляется сквозным полем от БД до обоих фронтендов (Blazor и Vue). Опциональность повторяет `Birthday`: `string?` в контрактах и доменной модели, nullable-колонка в БД. Файлы я не менял.

## Сначала нужно подтвердить решения

Проектное правило требует согласовать эти пункты до реализации. Мои рекомендации:

| Вопрос | Рекомендация |
|---|---|
| Максимальная длина | 254 символа (предел RFC), константа `ContactRules.EmailMaxLength` |
| Проверка формата | Да, простая: FluentValidation `EmailAddress()` на API и `[EmailAddress]` в Web/Vue. Это не полная проверка RFC. |
| Пустая строка | Считать «нет email»: после `Trim()` пустое значение сохраняется как `null` |
| Уникальность | Нет, два контакта могут иметь один email |
| Поиск | Да, добавить Email в фильтр `SearchText` |
| Показ в списке | Да, отдельная колонка в таблице контактов (в Blazor и Vue) |
| Существующие данные | У старых контактов Email будет `null`, значения по умолчанию не нужны |
| Регистр | Хранить как введён, без приведения к нижнему регистру |

## Что менять, по слоям

```
Contracts → API (домен, БД, валидаторы, хендлеры) → Web (Blazor) → Vue → тесты → документация
```

**1. `AddressBook.Contracts`**
- Добавить `string? Email` в `CreateContactCommand` и `UpdateContactCommand`.
- Добавить `string? Email` в конец записи `ContactModel`. Это меняет конструктор, поэтому нужно поправить все места, где он вызывается (тестовые билдеры, `TestIds`).
- В `ContactRules` добавить `EmailMaxLength` и общее сообщение об ошибке формата. Так клиент и сервер используют одни правила.

**2. `AddressBook.Api`**
- `Domain/Contact.cs`: свойство `Email`.
- `DataAccess/ContactConfiguration.cs`: `HasMaxLength(ContactRules.EmailMaxLength)` и `IsRequired(false)`.
- Миграция `AddEmailToContact`: создать через `dotnet ef migrations add`, снимок `ApplicationDbContextModelSnapshot.cs` не править вручную.
- `AddressBookRepository.cs`: добавить `.SetProperty(c => c.Email, item.Email)` в `UpdateAsync` и условие по Email в `RetrieveManyAsync`.
- `CreateContactCommandValidator` и `UpdateContactCommandValidator`: `MaximumLength` и проверка формата, применяются только когда значение непустое.
- Хендлеры `Create`, `Update`, `GetById`, `GetFiltered`: обрезать пробелы и превращать пустое значение в `null` (create/update). Передавать Email в `ContactModel` (get).

**3. `AddressBook.Web` (Blazor)**
- `Models/CreateContactModel.cs`: `[StringLength]` и `[EmailAddress]` с общими константами.
- `Pages/CreateContact.razor` и `Pages/EditContact.razor`: поле `MudTextField` для Email.
- `Pages/Contacts.razor` и `Contacts.razor.cs`: колонка Email.
- `AddressBookApiService.cs`: проверить, что Email попадает в запросы.

**4. `AddressBook.Vue`**
- `rules/contactRules.ts`: те же длина и формат.
- `components/ContactForm.vue` и `contactFormErrors.ts`: поле Email и показ ошибок.
- `api/contactsApi.ts`: тип контакта и payload.
- `pages/ContactsPage.vue`: колонка Email.

**5. Тесты**
- `src/ApiTests` (обязательно по правилам проекта): `create.spec.ts` и `update.spec.ts` с happy path, границей 254/255, неверным форматом, пустым значением, `null`. Нужны правки `contact.schema.ts` и `contact.factory.ts`. Тест поиска по Email.
- bUnit (`AddressBook.Web.Tests`): `ContactBuilder`, `ContactFormHarness`, `ContactsTableHarness`, тесты Create/Edit/List.
- Vitest: `contactRules.spec.ts`, `ContactForm.spec.ts`, `contactFormErrors.spec.ts`, `ContactsPage.spec.ts`.
- `src/UiTests`: компоненты формы и таблицы для обоих фронтендов, `testids.ts`, `contact.factory.ts`, сценарии create/edit.

**6. Документация (в том же изменении)**
- `docs/specs/AddressBook.Api.md`, `AddressBook.Contracts.md`, `AddressBook.Web.md`, `AddressBook.Vue.md`.
- `src/AddressBook.Vue/README.md`.
- Выдержки в `docs/guides/vue-for-blazor-developers.md`, если они цитируют изменённые файлы.

## Дополнительно
- Работа идёт в текущей ветке `main`. Рабочее дерево уже содержит много чужих незакоммиченных правок (harness), и я бы их не трогал.
- Коммит, ветку и PR я сам не создаю. Если вы это попросите, коммит пойдёт через `issue-kit:git-commit`, PR через `issue-kit:open-pr`, с `Closes #<n>`.
- После реализации я бы запустил `security-reviewer`. Меняется ввод пользователя и схема БД.

Подтвердите или поправьте таблицу решений. После этого я начну реализацию.
