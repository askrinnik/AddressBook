**Коротко:** Email добавляется как необязательная строка по цепочке Contracts → Api (домен, БД, валидатор, обработчики) → Blazor Web → Vue → тесты → документация. Перед реализацией мне нужны от вас ответы на вопросы из последнего раздела. Проектные правила требуют не выбирать их молча. Файлы я не менял.

## Что изменить

```
Contracts → Api (Domain, DataAccess, Application) → Migration
         → Web (Blazor) → Vue → тесты → docs
```

**1. `AddressBook.Contracts`**
- `ContactRules.cs`: добавить `EmailMaxLength` и, возможно, общее сообщение об ошибке формата. Так API и клиенты берут один и тот же лимит.
- `CreateContactCommand.cs` и `UpdateContactCommand.cs`: добавить `string? Email { get; set; }`.
- `Models/ContactModel.cs`: добавить `string? Email` в конец record. Это ломает все места, где вызывается конструктор. Их найдёт компилятор.

**2. `AddressBook.Api`**
- `Domain/Contact.cs`: добавить `string? Email { get; set; }`.
- `DataAccess/ContactConfiguration.cs`: `builder.Property(e => e.Email).HasMaxLength(ContactRules.EmailMaxLength).IsRequired(false);`
- `DataAccess/AddressBookRepository.cs`: добавить `.SetProperty(c => c.Email, item.Email)` в `ExecuteUpdateAsync` (около строки 48). Если его забыть, PUT молча не сохранит Email.
- `Application/CreateContactCommandValidator.cs` и `UpdateContactCommandValidator.cs`: правило для Email, которое срабатывает только когда поле заполнено. Проверяются максимальная длина и формат.
- `CreateContactCommandHandler.cs` и `UpdateContactCommandHandler.cs`: обрезать пробелы по `.Trim()` после валидации. Пустую строку сохранять как `null`.
- `GetContactByIdQueryHandler.cs` и `GetFilteredContactsQueryHandler.cs`: передать Email в `ContactModel`.
- Миграция: `dotnet ef migrations add ContactEmail`. Она добавит nullable-колонку `Email nvarchar(N)`. Старые строки получат `NULL`, данные не теряются.

**3. Blazor (`AddressBook.Web`)**
- `Models/CreateContactModel.cs`: поле с `[StringLength]` и `[EmailAddress]`, как для имён.
- `Pages/CreateContact.razor` и `EditContact.razor`: `MudTextField` с `data-testid`.
- `AddressBookApiService.cs`: передать поле в команды.
- `Pages/Contacts.razor` и `Contacts.razor.cs`: колонка в таблице, если вы этого хотите.

**4. Vue (`AddressBook.Vue`)**
- `src/api/contactsApi.ts`, `src/rules/contactRules.ts`, `src/components/ContactForm.vue`, `src/components/contactFormErrors.ts`, `src/pages/ContactsPage.vue`.
- Правило формата и лимит должны совпадать с C#. В проекте уже есть тесты на дрейф валидации (issue 204).

**5. Тесты**
- `src/ApiTests` (Playwright): `contact.schema.ts`, `contact.factory.ts`, `create.spec.ts`, `update.spec.ts`, `schema.spec.ts`. Проверяем счастливый путь, границы длины, неверный формат, пустое значение и обрезку пробелов. Это обязательное правило для нового поведения API.
- `src/AddressBook.Web.Tests` (bUnit): `ContactBuilder`, `ContactFormHarness`, `TestIds`, тесты страниц Create, Edit и списка.
- `src/AddressBook.Vue/tests`: `ContactForm.spec.ts`, `contactRules.spec.ts`, `contactFormErrors.spec.ts`, `ContactsPage.spec.ts`.
- `src/UiTests`: `contact.factory.ts`, компоненты формы (blazor и vue), `testids.ts` и сценарии create/edit.

**6. Документация** (в том же изменении, по `update-docs-on-code-change.md`)
- `docs/specs/AddressBook.Api.md`, `AddressBook.Contracts.md`, `AddressBook.Web.md`, `AddressBook.Vue.md`.
- При необходимости `src/AddressBook.Vue/README.md` и выдержки в `docs/guides/vue-for-blazor-developers.md`.

## Что нужно подтвердить

1. **Формат.** Достаточно ли простой проверки, как у `[EmailAddress]` и `MailAddress`, или нужен строгий regex? Я рекомендую простую.
2. **Максимальная длина.** Я предлагаю 254 символа, это предел из RFC.
3. **Уникальность.** Нужна ли уникальность Email среди контактов? Я рекомендую нет: у членов семьи бывает общий адрес. Уникальность потребовала бы индекса и ответа 409.
4. **Регистр.** Хранить как введено или приводить к нижнему? Я рекомендую хранить как введено.
5. **Таблица.** Показывать ли Email в списке контактов, в Blazor и Vue?
6. **Поиск.** Должен ли фильтр `GetFilteredContacts` искать по Email?
7. **Пустое значение.** Пустую строку и пробелы считать «нет значения» и хранить `NULL`? Я рекомендую да.
8. **Старые данные.** Колонка nullable, значения по умолчанию не нужны. Заполнять seed-данные тестовыми адресами нужно?
9. **Объём.** Делаем API, Blazor и Vue сразу (так требует проектное правило про полноту) или только API и Blazor?

Дальше ничего не коммичу и не пушу без вашей команды. Когда ответите на вопросы, могу начать реализацию.
