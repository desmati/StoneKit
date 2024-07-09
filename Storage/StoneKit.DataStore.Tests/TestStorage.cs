using System.Data;

namespace StoneKit.DataStore.Tests
{
    [TestClass]
    public class TestStorage
    {
        [TestMethod]
        public void TestStorageEncrypted()
        {
            var options = new DataStoreOptions()
            {
                EncryptionKey = "ksgdjaf7346t96r&$e6%4r85R6e$86%R875Rksgdjaf7346t96r&$e6%4r85R6e$86%R875Rksgdjaf7346t96r&$e6%4r85R6e$86%R875Rksgdjaf7346t96r&$e6%4r85R6e$86%R875Rksgdjaf7346t96r&$e6%4r85R6e$86%R875R"
            };

            AesEncryption.Init(options.EncryptionKey);

            var dataStore = new System.Data.DataStore(options.DirectoryPath, options.FileLifetime, options.CleanupInterval, !string.IsNullOrEmpty(options.EncryptionKey));

            System.Data.DataStore.StaticStorage = dataStore;

            var data = new SampleData()
            {
                Id =1,
                Name = "NameIt_ENC",
                SubData = new SampleSubData()
                {
                    Id=2,
                    Name = "NameIt2_ENC"
                }
            };

            System.Data.DataStore.StaticStorage.SaveAsync(data, "CustomFileNameAsId_ENC").GetAwaiter().GetResult();

            var fetchedData = System.Data.DataStore.StaticStorage.LoadAsync<SampleData>("CustomFileNameAsId_ENC").GetAwaiter().GetResult();

            Assert.IsTrue(fetchedData.HasValue);
            Assert.IsNotNull(fetchedData);
            Assert.IsNotNull(fetchedData.Value);
            Assert.IsTrue(fetchedData.Value.SubData.Name == data.SubData.Name);
        }

        [TestMethod]
        public void TestStorageUnsecured()
        {
            var options = new DataStoreOptions();

            AesEncryption.Init(options.EncryptionKey);

            var dataStore = new System.Data.DataStore(options.DirectoryPath, options.FileLifetime, options.CleanupInterval, !string.IsNullOrEmpty(options.EncryptionKey));

            System.Data.DataStore.StaticStorage = dataStore;

            var data = new SampleData()
            {
                Id =1,
                Name = "NameIt",
                SubData = new SampleSubData()
                {
                    Id=2,
                    Name = "NameIt2"
                }
            };

            System.Data.DataStore.StaticStorage.SaveAsync(data, "CustomFileNameAsId").GetAwaiter().GetResult();

            var fetchedData = System.Data.DataStore.StaticStorage.LoadAsync<SampleData>("CustomFileNameAsId").GetAwaiter().GetResult();

            Assert.IsTrue(fetchedData.HasValue);
            Assert.IsNotNull(fetchedData);
            Assert.IsNotNull(fetchedData.Value);
            Assert.IsTrue(fetchedData.Value.SubData.Name == data.SubData.Name);
        }
    }
}