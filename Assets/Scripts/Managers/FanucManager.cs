using Rti.Dds.Subscription;
using Rti.Types.Dynamic;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FanucManager : MonoBehaviour
{
    public List<ArticulationBody> joints;
    public Transform worldPosition;
    public DDSHandler dDSHandler;
    private protected DataReader<DynamicData> reader { get; private set; }

    private bool init = false;

    public TMP_Text clock;
    public TMP_Text samplesCount;
    public TMP_Text j1;
    public TMP_Text j2;
    public TMP_Text j3;
    public TMP_Text j4;
    public TMP_Text j5;
    public TMP_Text j6;
    public TMP_Text x;
    public TMP_Text y;
    public TMP_Text z;
    public TMP_Text w;
    public TMP_Text p;
    public TMP_Text r;
    public TMP_Text alarm;

    private void Update()
    {
        if (!init)
        {
            init = true;
            var typeFactory = DynamicTypeFactory.Instance;
            StructType RobotState = typeFactory.BuildStruct()
                .WithName("RobotState")
                .AddMember(new StructMember("Clock", typeFactory.CreateString(bounds: 50)))
                .AddMember(new StructMember("Sample", typeFactory.GetPrimitiveType<int>()))
                .AddMember(new StructMember("J1", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("J2", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("J3", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("J4", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("J5", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("J6", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("X", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("Y", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("Z", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("W", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("P", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("R", typeFactory.GetPrimitiveType<double>()))
                .AddMember(new StructMember("Alarm", typeFactory.CreateString(bounds: 200)))
                .Create();

            reader = dDSHandler.SetupDataReader("RobotState_Topic", RobotState);
        }

        ProcessData(reader);
    }

    public Quaternion CreateQuaternionFromFanucWPR(float W, float P, float R)
    {
        float Wrad = W * (Mathf.PI / 180);
        float Prad = P * (Mathf.PI / 180);
        float Rrad = R * (Mathf.PI / 180);

        float qx = (Mathf.Cos(Rrad / 2) * Mathf.Cos(Prad / 2) * Mathf.Sin(Wrad / 2)) - (Mathf.Sin(Rrad / 2) * Mathf.Sin(Prad / 2) * Mathf.Cos(Wrad / 2));
        float qy = (Mathf.Cos(Rrad / 2) * Mathf.Sin(Prad / 2) * Mathf.Cos(Wrad / 2)) + (Mathf.Sin(Rrad / 2) * Mathf.Cos(Prad / 2) * Mathf.Sin(Wrad / 2));
        float qz = (Mathf.Sin(Rrad / 2) * Mathf.Cos(Prad / 2) * Mathf.Cos(Wrad / 2)) - (Mathf.Cos(Rrad / 2) * Mathf.Sin(Prad / 2) * Mathf.Sin(Wrad / 2));
        float qw = (Mathf.Cos(Rrad / 2) * Mathf.Cos(Prad / 2) * Mathf.Cos(Wrad / 2)) + (Mathf.Sin(Rrad / 2) * Mathf.Sin(Prad / 2) * Mathf.Sin(Wrad / 2));

        return new Quaternion(qx, qy, qz, qw);
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

                worldPosition.localPosition = new Vector3(-(float)data.GetValue<double>("X") / 1000, (float)data.GetValue<double>("Y") / 1000, (float)data.GetValue<double>("Z") / 1000);
                Vector3 eulerAngles = CreateQuaternionFromFanucWPR((float)data.GetValue<double>("W"), (float)data.GetValue<double>("P"), (float)data.GetValue<double>("R")).eulerAngles;
                worldPosition.localEulerAngles = new Vector3(eulerAngles.x, -eulerAngles.y, -eulerAngles.z);

                var J1drive = joints[0].xDrive;
                var J2drive = joints[1].xDrive;
                var J3drive = joints[2].xDrive;
                var J4drive = joints[3].xDrive;
                var J5drive = joints[4].xDrive;
                var J6drive = joints[5].xDrive;

                J1drive.target = (float)data.GetValue<double>("J1");
                J2drive.target = (float)data.GetValue<double>("J2");
                J3drive.target = (float)data.GetValue<double>("J3") + (float)data.GetValue<double>("J2");
                J4drive.target = (float)data.GetValue<double>("J4");
                J5drive.target = (float)data.GetValue<double>("J5");
                J6drive.target = (float)data.GetValue<double>("J6");

                joints[0].xDrive = J1drive;
                joints[1].xDrive = J2drive;
                joints[2].xDrive = J3drive;
                joints[3].xDrive = J4drive;
                joints[4].xDrive = J5drive;
                joints[5].xDrive = J6drive;

                clock.text = data.GetValue<string>("Clock");
                samplesCount.text = $"Samples sent: {data.GetValue<int>("Sample")}";
                j1.text = $"J1: {Math.Round(J1drive.target, 2)}";
                j2.text = $"J2: {Math.Round(J2drive.target, 2)}";
                j3.text = $"J3: {Math.Round((J3drive.target + J2drive.target), 2)}";
                j4.text = $"J4: {Math.Round(J4drive.target, 2)}";
                j5.text = $"J5: {Math.Round(J5drive.target, 2)}";
                j6.text = $"J6: {Math.Round(J6drive.target, 2)}";
                x.text = $"X: {Math.Round(worldPosition.localPosition.x, 2)}";
                y.text = $"Y: {Math.Round(worldPosition.localPosition.y, 2)}";
                z.text = $"Z: {Math.Round(worldPosition.localPosition.z, 2)}";
                w.text = $"W: {Math.Round(worldPosition.localEulerAngles.x, 2)}";
                p.text = $"P: {Math.Round(worldPosition.localEulerAngles.y, 2)}";
                r.text = $"R: {Math.Round(worldPosition.localEulerAngles.z, 2)}";
                if (data.GetValue<string>("Alarm") != "")
                {
                    alarm.text = $"Alarm: {data.GetValue<string>("Alarm")}";
                }
            }
        }
    }
}