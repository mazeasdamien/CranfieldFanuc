using Rti.Dds.Subscription;
using Rti.Types.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ImageManager : MonoBehaviour
{
    public GUISkin skin;
    private bool init = false;
    private DDSHandler dDSHandler;
    private protected DataReader<DynamicData> reader { get; private set; }
    public TMP_Text clock;
    public TMP_Text samplesCount;
    public RawImage rawTexture;
    private Texture2D texture2D;

    void Start()
    {
        texture2D = new Texture2D(1, 1);
        dDSHandler = gameObject.AddComponent<DDSHandler>();
    }

    void Update()
    {
        if (!init)
        {
            init = true;

            var typeFactory = DynamicTypeFactory.Instance;
            StructType RobotImage = typeFactory.BuildStruct()
                .WithName("RobotImage")
                .AddMember(new StructMember("Clock", typeFactory.CreateString(bounds: 50)))
                .AddMember(new StructMember("Sample", typeFactory.GetPrimitiveType<int>()))
                .AddMember(new StructMember("sequence_Memory", typeFactory.CreateSequence(typeFactory.GetPrimitiveType<byte>(), 600000)))
                .Create();

            reader = dDSHandler.SetupDataReader("RobotImage_Topic", RobotImage);
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

                clock.text = data.GetValue<string>(memberName: "Clock");
                samplesCount.text = $"Samples sent: {data.GetValue<int>("Sample")}";
                texture2D.LoadImage(data.GetValue<byte[]>("sequence_Memory"));
                texture2D.Apply();
                rawTexture.texture = texture2D;
            }
        }
    }
}
