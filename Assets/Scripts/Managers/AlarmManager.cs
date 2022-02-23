using Rti.Dds.Subscription;
using Rti.Types.Dynamic;
using TMPro;
using UnityEngine;

public class AlarmManager : MonoBehaviour
{
    private DDSHandler dDSHandler;
    private protected DataReader<DynamicData> reader { get; private set; }

    private bool init = false;

    public TMP_Text clock;
    public TMP_Text samplesCount;
    public TMP_Text alarm;

    // Start is called before the first frame update
    void Start()
    {
        dDSHandler = gameObject.AddComponent<DDSHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!init)
        {
            init = true;
            var typeFactory = DynamicTypeFactory.Instance;
            StructType RobotState = typeFactory.BuildStruct()
                .WithName("RobotAlarm")
                .AddMember(new StructMember("Clock", typeFactory.CreateString(bounds: 50)))
                .AddMember(new StructMember("Sample", typeFactory.GetPrimitiveType<int>()))
                .AddMember(new StructMember("Alarm", typeFactory.CreateString(bounds: 200)))
                .Create();

            reader = dDSHandler.SetupDataReader("RobotAlarm_Topic", RobotState);
        }

        ProcessData(reader);
    }

    void ProcessData(AnyDataReader anyReader)
    {
        var reader = (DataReader<DynamicData>)anyReader;
        using var samples = reader.Take();
        foreach (var sample in samples)
        {
            if (sample.Info.ValidData)
            {
                DynamicData data = sample.Data;

                clock.text = data.GetValue<string>("Clock");
                samplesCount.text = $"Samples sent: {data.GetValue<int>("Sample")}";
                alarm.text = $"Alarm: {data.GetValue<string>("Alarm")}";
            }
        }
    }
}
