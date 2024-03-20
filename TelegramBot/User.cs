using DataLayer;

namespace TelegramBot;

public class User
{
    public long ChatId { get; set; }
    public long Id { get; set; }
    public UserState State { get; set; }
    public Dictionary<string, List<Hockey>> Files { get; set; }
    public MessageSender Sender { get; set; }
    public List<Hockey> SelectedFile { get; set; }
    public string SelectedFileName { get; set; }
    public List<string> SelectedFields { get; set; }
    public List<string> SelectedValues { get; set; }

    public User() : this(0, 0) { }

    public User(long chatId, long id)
    {
        ChatId = chatId;
        Id = id;
        State = UserState.Start;
        Files = new Dictionary<string, List<Hockey>>();
        Sender = new MessageSender(this);
    }
}