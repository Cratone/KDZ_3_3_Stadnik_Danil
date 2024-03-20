namespace TelegramBot;

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