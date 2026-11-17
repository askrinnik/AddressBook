**Коротко:** Email добавляется сквозным полем по всем слоям: Contracts → API (домен, валидация, БД, миграция) → Web (Blazor) → Vue → тесты → документация. Образец — поле `Birthday`: оно необязательное, и оно проходит через те же файлы. Файлы я не менял.

Перед реализацией мне нужно, чтобы вы подтвердили несколько решений. Этого требует CLAUDE.md. Мои предложения указаны в каждом пункте.

## Решения, которые нужно подтвердить

1. **Длина.** Предлагаю 254 символа. Это практический максимум адреса по RFC. Значение будет в `ContactRules.EmailMaxLength`.
2. **Формат.** Предлагаю `EmailAddress()` из FluentValidation на стороне API. В Blazor и Vue та же проверка, сообщение об ошибке общее (константа в `ContactRules`, как `BirthdayInFutureMessage`).
3. **Необязательность.** Email может быть `null`. Пустую или пробельную строку я превращаю в `null` после `Trim()`, чтобы в БД не было двух видов «пусто».
4. **Уникальность.** Предлагаю не требовать уникальности: у двух контактов может быть один адрес (например, общий семейный). Уникальный индекс потребовал бы ещё и ответ 409.
5. **Регистр.** Предлагаю хранить адрес как введён, без приведения к нижнему регистру.
6. **Показ и поиск.** Предлагаю показывать Email в форме создания и редактирования и в таблице контактов. Поиск `SearchText` по Email не расширять. Если нужен поиск, скажите.
7. **Существующие данные.** Колонка будет nullable, у старых контактов Email будет `null`. Заполнять ничего не нужно.

## Что меняется

```
Contracts ──> Api ──> БД (миграция)
   │
   ├──> Web (Blazor) ──> bUnit-тесты
   ├──> Vue ──────────> Vitest-тесты
   └──> ApiTests / UiTests (Playwright)
```

**Contracts** (`src/AddressBook.Contracts`)
- `ContactRules.cs`: добавить `EmailMaxLength` и общее сообщение об ошибке формата.
- `CreateContactCommand.cs` и `UpdateContactCommand.cs`: добавить `string? Email`.
- `Models/ContactModel.cs`: добавить `string? Email` в конец record. Это ломает все вызовы конструктора, компилятор их покажет.

**API** (`src/AddressBook.Api`)
- `Domain/Contact.cs`: свойство `string? Email`.
- `DataAccess/ContactConfiguration.cs`: `HasMaxLength(ContactRules.EmailMaxLength)` и `IsRequired(false)`.
- `Application/CreateContactCommandValidator.cs` и `UpdateContactCommandValidator.cs`: правило `When(x => !string.IsNullOrWhiteSpace(x.Email))` с `MaximumLength` и `EmailAddress()`.
- `CreateContactCommandHandler.cs` и `UpdateContactCommandHandler.cs`: нормализация (`Trim()`, пустое → `null`).
- `DataAccess/AddressBookRepository.cs`: добавить `.SetProperty(c => c.Email, item.Email)` в `UpdateAsync`. Если забыть, обновление молча не сохранит Email, потому что используется `ExecuteUpdateAsync`.
- `GetContactByIdQueryHandler.cs` и `GetFilteredContactsQueryHandler.cs`: передать Email в `ContactModel`.
- Миграция `AddEmailToContact` через `dotnet ef migrations add`. Она добавляет nullable `nvarchar(254)`. Обновится `ApplicationDbContextModelSnapshot.cs`.

**Web, Blazor** (`src/AddressBook.Web`)
- `Models/CreateContactModel.cs`: `[StringLength]` и `[EmailAddress]`.
- `Pages/CreateContact.razor`, `EditContact.razor`: поле `MudTextField` с `data-testid` по принятым в проекте правилам.
- `Pages/Contacts.razor`: колонка в таблице.
- `AddressBookApiService.cs`: проверить маппинг модели в команду.

**Vue** (`src/AddressBook.Vue`)
- `src/rules/contactRules.ts`: те же длина и формат.
- `src/components/ContactForm.vue`, `contactFormErrors.ts`: поле формы и разбор ошибок API.
- `src/api/contactsApi.ts`: тип `Contact`.
- `src/pages/ContactsPage.vue`: колонка в таблице.

**Тесты** (по правилам из CLAUDE.md)
- `src/ApiTests`: Playwright-тесты на создание и обновление. Нужны успешный случай, граница 254/255, неверный формат, `null` и пустая строка, чтение в `GET`. Обновить `contact.schema.ts` и `contact.factory.ts`.
- `src/AddressBook.Web.Tests`: bUnit-тесты форм и `ContactBuilder`.
- `src/AddressBook.Vue/tests`: тесты `ContactForm`, `contactRules`, `ContactsPage`.
- `src/UiTests`: компоненты форм и таблицы, фабрика данных, `testids.ts`, при необходимости сценарии CRUD.

**Документация** (по `update-docs-on-code-change.md`)
- `docs/specs/AddressBook.Api.md`, `AddressBook.Contracts.md`, `AddressBook.Web.md`, `AddressBook.Vue.md`.
- Если изменился файл, который цитирует `docs/guides/vue-for-blazor-developers.md`, обновить и его цитату.

## Порядок работы
1. Contracts и API, затем миграция.
2. Тесты ApiTests.
3. Blazor и Vue, затем их тесты.
4. UiTests и документация.
5. Проверка: сборка, запуск всех наборов тестов.

Ещё два замечания. Рабочее дерево сейчас сильно изменено (переезд на плагин `issue-kit`), поэтому реализацию лучше вести отдельно от этих правок. Коммит, ветку и PR я без вашей явной команды не создаю.

Подтвердите пункты 1–7 или поправьте их, и я начну.
