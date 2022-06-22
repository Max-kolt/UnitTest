using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Reflection;
using UnitTestEx;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    /// <summary>
    /// Summary description for FileStorageTest
    /// </summary>
    [TestClass]
    public class FileStorageTest
    {
        public const string MAX_SIZE_EXCEPTION = "DIFFERENT MAX SIZE";
        public const string NULL_FILE_EXCEPTION = "NULL FILE";
        public const string NO_EXPECTED_EXCEPTION_EXCEPTION = "There is no expected exception";

        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = "@D:\\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public const string REPEATED_STRING = "AA";
        public const string WRONG_SIZE_CONTENT_STRING = "TEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtext";
        public const string TIC_TOC_TOE_STRING = "tictoctoe.game";

        public const int NEW_SIZE = 5;

        public FileStorage storage = new FileStorage(NEW_SIZE);

        /* ПРОВАЙДЕРЫ */

        static object[] NewFilesData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) },
            new object[] { new File(SPACE_STRING, WRONG_SIZE_CONTENT_STRING) },
            new object[] { new File(FILE_PATH_STRING, CONTENT_STRING) }
        };

        static object[] FilesForDeleteData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING), REPEATED_STRING },
            new object[] { null, TIC_TOC_TOE_STRING }
        };

        static object[] NewExceptionFileData = {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) }
        };

        /* Тестирование записи файла */
        //[Test, TestCaseSource(nameof(NewFilesData))]
        [TestMethod]
        public void WriteTest() 
        {
            File file = new File("testFile","Hello!");
            Assert.True(storage.Write(file));
            storage.DeleteAllFiles();
        }

        /* Тестирование записи дублирующегося файла */
        //[Test, TestCaseSource(nameof(NewExceptionFileData))]
        [TestMethod]
        public void WriteExceptionTest()
        {
            File file = new File("testFile", "Hello!");
            bool isException = false;   
            try
            {
                storage.Write(file);
                Assert.False(storage.Write(file));
                storage.DeleteAllFiles();
            } 
            catch (FileNameAlreadyExistsException)
            {
                isException = true;
            }
            Assert.True(isException, NO_EXPECTED_EXCEPTION_EXCEPTION);
        }

        /* Тестирование проверки существования файла */
        //[Test, TestCaseSource(nameof(NewFilesData))]
        [TestMethod]
        public void IsExistsTest()
        {
            File file = new File("testFile", "Hello!");
            string name = file.GetFilename();
            Assert.False(storage.IsExists(name));
            try {
                storage.Write(file);
            } catch (FileNameAlreadyExistsException e) {
                Console.WriteLine(String.Format("Exception {0} in method {1}", e.GetBaseException(), MethodBase.GetCurrentMethod().Name));
            }
            Assert.True(storage.IsExists(name));
            storage.DeleteAllFiles();
        }

        /* Тестирование удаления файла */
        //[Test, TestCaseSource(nameof(FilesForDeleteData))]
        [TestMethod]
        public void DeleteTest() {

            File file = new File("testFile", "Hello!");
            string fileName = file.GetFilename();
            storage.Write(file);
            Assert.True(storage.Delete(fileName));
        }

        /* Тестирование получения файлов */
        //[Test]
        [TestMethod]
        public void GetFilesTest()
        {
            foreach (File el in storage.GetFiles()) 
            {
                Assert.NotNull(el);
            }
        }

        // Почти эталонный
        /* Тестирование получения файла */
        //[Test, TestCaseSource(nameof(NewFilesData))]
        [TestMethod]
        public void GetFileTest()
        {
            File expectedFile = new File("testFile", "Hello!");
            storage.Write(expectedFile);

            File actualfile = storage.GetFile(expectedFile.GetFilename());
            bool difference = actualfile.GetFilename().Equals(expectedFile.GetFilename()) && actualfile.GetSize().Equals(expectedFile.GetSize());

            Assert.IsTrue(difference, string.Format("There is some differences in {0} or {1}", expectedFile.GetSize(), expectedFile.GetFilename()));
        }

        /*______________________ДОПОЛНИТЕЛЬНЫЕ 3 ТЕСТА______________________*/

        /* Тестирпование переполнения хранилища
         * Проверяет запишется ли файл, превышающий размер хранилища 
         */
        [TestMethod]
        public void StorageOverflowTest()
        {
            int maxSizeStorage = 2; // Размер нового хранилища
            FileStorage newStorage = new FileStorage(maxSizeStorage);
            File file = new File("testFile", "Hello World!"); // Размер файла равен 6

            bool isFileWrited = newStorage.Write(file);
            Assert.IsFalse(isFileWrited, "File size exceeds storage size");
        }

        /* Тестирование повторяющегося файла
         * Проверяет запишется ли файл, когда в хранилище подобный уже существует
         */
        [TestMethod]
        public void RepeatFilenameTest()
        {
            File file = new File("testFile", "1");
            storage.Write(file);
            try
            {
                storage.Write(file);
            }
            catch (FileNameAlreadyExistsException)
            {
                return;
            }
            Assert.Fail();
        }

        /* Тестирование удаления не существующего файла
         * 
         */
        [TestMethod]
        public void DeleteNullFileTest()
        {
            string fileName = "testFile";
            bool isDeleted = true;

            if (!storage.IsExists(fileName)) 
            {
                isDeleted = storage.Delete(fileName);
            }
            Assert.IsFalse(isDeleted);
        }
    }
}
