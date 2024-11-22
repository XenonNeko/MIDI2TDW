using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;


public class Diagnostics : MonoBehaviour
{
    [SerializeField]
    private string discordUrl;

    [Serializable]
    private class Snapshot
    {
        public string relativePath;
        public string description;
        public DateTime dateCreated;
        [JsonIgnore]
        public byte[] contents;

        public static Snapshot CreateFromFile(string path, string description)
        {
            return new()
            {
                relativePath = Path.GetRelativePath(Application.streamingAssetsPath, path),
                description = description,
                dateCreated = DateTime.Now,
                contents = File.ReadAllBytes(path)
            };
        }

        public static Snapshot CreateFromText(string text, string description)
        {
            return new()
            {
                relativePath = "(in memory)",
                description = description,
                dateCreated = DateTime.Now,
                contents = Encoding.ASCII.GetBytes(text)
            };
        }

        public byte[] GetFile()
        {
            return contents;
        }

        public byte[] Serialize()
        {
            MemoryStream stream = new();
            BinaryWriter serializer = new(stream);
            serializer.Write(relativePath);
            serializer.Write(description);
            serializer.Write(dateCreated.ToFileTimeUtc());
            serializer.Close();
            return stream.ToArray();
        }

        public static Snapshot Deserialize(byte[] bytes)
        {
            MemoryStream stream = new(bytes);
            BinaryReader deserializer = new(stream);
            Snapshot snapshot = new()
            {
                relativePath = deserializer.ReadString(),
                description = deserializer.ReadString(),
                dateCreated = DateTime.FromFileTimeUtc(deserializer.ReadInt64())
            };
            deserializer.Close();
            return snapshot;
        }
    }

    private readonly List<Snapshot> snapshots = new();

    public void SnapshotFile(string path, string description)
    {
        snapshots.Add(Snapshot.CreateFromFile(path, description));
    }

    [SerializeField]
    private Logger logger;
    private void SnapshotLog()
    {
        snapshots.Add(Snapshot.CreateFromText(logger.GetLogText(), "session-log.log"));
    }

    private byte[] SerializePackage()
    {
        #region Serialize (binary)
        MemoryStream stream = new();
        BinaryWriter serializer = new(stream);

        serializer.Write(snapshots.Count);
        foreach (Snapshot snapshot in snapshots)
        {
            byte[] metadata = snapshot.Serialize();
            serializer.Write(metadata.Length);
            serializer.Write(metadata);
            byte[] contents = snapshot.GetFile();
            serializer.Write(contents.Length);
            serializer.Write(contents);
        }

        serializer.Close();
        byte[] pkg = stream.ToArray();
        #endregion

        #region Compress (GZip)
        using var gzippedStream = new MemoryStream();
        using var gzStream = new GZipStream(gzippedStream, System.IO.Compression.CompressionLevel.Optimal);
        gzStream.Write(pkg);
        gzStream.Close();
        byte[] gzipped_pkg = gzippedStream.ToArray();
        #endregion

        // FINAL WRITE
        stream = new();
        serializer = new(stream);

        serializer.Write(gzipped_pkg);

        serializer.Close();

        return stream.ToArray();
    }

#if UNITY_EDITOR
    [Header("Editor Only")]
    [SerializeField]
    private bool decrypt;
    [SerializeField]
    private string package;

    private void Update()
    {
        if (decrypt)
        {
            decrypt = false;
            string path = Path.Combine(Application.streamingAssetsPath, "![unpkg]", "dpkg", $"{package}.dpkg");
            DeserializePackage(File.ReadAllBytes(path));
        }
    }

    private void DeserializePackage(byte[] bytes)
    {
        #region Decompress (GZip)
        using var gzippedStream = new MemoryStream(bytes);
        using var gzStream = new GZipStream(gzippedStream, CompressionMode.Decompress);
        using var pkgStream = new MemoryStream();
        gzStream.CopyTo(pkgStream);
        #endregion

        BinaryReader deserializer = new BinaryReader(pkgStream);
        pkgStream.Seek(0, SeekOrigin.Begin);

        int snapshots_count = deserializer.ReadInt32();
        for (int i = 0; i < snapshots_count; i++)
        {
            int metadata_length = deserializer.ReadInt32();
            byte[] metadata = deserializer.ReadBytes(metadata_length);

            int contents_length = deserializer.ReadInt32();
            byte[] contents = deserializer.ReadBytes(contents_length);

            Snapshot snapshot = Snapshot.Deserialize(metadata);
            snapshot.contents = contents;

            string json = JsonConvert.SerializeObject(snapshot);
            string path = Path.Combine(Application.streamingAssetsPath, "![unpkg]", "snapinfo", $"{i}.json");
            File.WriteAllText(path, json);

            path = Path.Combine(Application.streamingAssetsPath, "![unpkg]", "snapfile", $"{i}_{snapshot.description}");
            File.WriteAllBytes(path, contents);
        }
    }
#endif

    public void CreateDiagnosticsPackage()
    {
        Debug.Log("Creating diagnostics package...");

        SnapshotLog();

        byte[] package = SerializePackage();

        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }
        string diagDirectory = Path.Combine(Application.streamingAssetsPath, "diagnostics");
        if (!Directory.Exists(diagDirectory))
        {
            Directory.CreateDirectory(diagDirectory);
        }
        DateTime now = DateTime.Now;
        string filename = Logger.TimeToFilename(now);
        string diagFilePath = Path.Combine(diagDirectory, $"{filename}.dpkg");
        File.WriteAllBytes(diagFilePath, package);

        gameObject.SetActive(true);
    }

    public void Upload()
    {
        Application.OpenURL(discordUrl);
    }
}
