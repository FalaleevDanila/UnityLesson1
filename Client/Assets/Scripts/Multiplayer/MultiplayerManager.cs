using Colyseus;

public class MultiplayerManager : ColyseusManager<MultiplayerManager> {
    private ColyseusRoom<State> _room;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        Instance.InitializeClient();
        Connect();
    }
    private async void Connect()
    {
        _room = await Instance.client.JoinOrCreate<State>("state_handler");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _room.Leave();
    }

}
