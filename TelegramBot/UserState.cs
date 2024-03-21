namespace TelegramBot;
/// <summary>
/// Определяет возможные состояния пользователя.
/// </summary>
public enum UserState
{
    Start,
    MainMenu,
    SendingFile,
    SelectFile,
    ActionWithFile,
    SelectFieldsForFilter,
    SelectValuesForFilter,
    SelectFieldForSort,
    SelectNameForSaving,
    SelectExtensionForDownloading
}