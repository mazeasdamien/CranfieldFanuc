using Rti.Dds.Subscription;
using Rti.Types.Dynamic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ImageManager : MonoBehaviour
{
    private bool init = false;
    public DDSHandler dDSHandler;
    private protected DataReader<DynamicData> reader { get; private set; }
    public TMP_Text clock;
    public TMP_Text samplesCount;
    public RawImage rawTexture;
    private Texture2D texture2D;

    void Start()
    {
        texture2D = new Texture2D(1, 1);
    }

    void Update()
    {
        if (!init)
        {
            init = true;

            var typeFactory = DynamicTypeFactory.Instance;
            StructType RobotImage = typeFactory.BuildStruct()
                .WithName("Video")
                .AddMember(new StructMember("Clock", typeFactory.CreateString(bounds: 50)))
                .AddMember(new StructMember("Sample", typeFactory.GetPrimitiveType<int>()))
                .AddMember(new StructMember("Memory", typeFactory.CreateSequence(typeFactory.GetPrimitiveType<byte>(), 1000000)))
                .Create();

            reader = dDSHandler.SetupDataReader("Video_Topic", RobotImage);
        }

        using var samples = reader.Take();
        foreach (var sample in samples)
        {
            if (sample.Info.ValidData)
            {
                DynamicData data = sample.Data;

                clock.text = data.GetValue<string>(memberName: "Clock");
                samplesCount.text = $"Samples sent: {data.GetValue<int>("Sample")}";
                texture2D.LoadImage(data.GetValue<byte[]>("Memory"));
                texture2D.Apply();
                rawTexture.texture = texture2D;
            }
        }
    }
}
