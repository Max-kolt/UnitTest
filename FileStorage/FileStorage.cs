using System;
using System.Collections.Generic;
using System.Text;
using UnitTestEx;

namespace UnitTestEx
{
    public class FileStorage
    {
        private List<File> files = new List<File>();
        private double availableSize = 100;
        private double maxSize = 100;

        /**
         * Construct object and set max storage size and available size according passed values
         * @param size FileStorage size
         */
        public FileStorage(int size) {
            maxSize = size;
            availableSize += maxSize;
        }

        /**
         * Construct object and set max storage size and available size based on default value=100
         */
        public FileStorage() {
        }


        /**
         * Write file in storage if filename is unique and size is not more than available size
         * @param file to save in file storage
         * @return result of file saving
         * @throws FileNameAlreadyExistsException in case of already existent filename
         */
        public bool Write(File file) {
            // Проверка существования файла
            if (IsExists(file.GetFilename())) {
                //Если файл уже есть, то кидаем ошибку
                throw new FileNameAlreadyExistsException();
            }

            //Проверка того, размер файла не привышает доступный объем памяти
            if (file.GetSize() >= availableSize) {
                return false;
            }

            // Добалвяем файл в лист
            files.Add(file);
            // Добалвяем файл в лист
            availableSize -= file.GetSize();

            return true;
        }

        /**
         * Check is file exist in storage
         * @param fileName to search
         * @return result of checking
         */
        public bool IsExists(String fileName) {
            // Для каждого элемента с типом File из Листа files
            foreach (File file in files) {
                // Проверка имени
                if (file.GetFilename().Contains(fileName)) {
                    return true;
                }
            }
            return false;
        }

        /**
         * Delete file from storage
         * @param fileName of file to delete
         * @return result of file deleting
         */
        public bool Delete(String fileName) {
            return files.Remove(GetFile(fileName));
        }

        /**
         * Get all Files saved in the storage
         * @return list of files
         */
        public List<File> GetFiles() {
            return files;
        }

        /**
         * Get file by filename
         * @param fileName of file to get
         * @return file
         */
        public File GetFile(String fileName) {  
            if (IsExists(fileName)) {
                foreach (File file in files) {
                    if (file.GetFilename().Contains(fileName)) {
                        return file;
                    }
                }
            }
            return null;
        }

        /**
         * Delete all files from files list
         * @return bool
         */
        public bool DeleteAllFiles()
        {
            files.RemoveRange(0, files.Count - 1);
            return files.Count == 0;
        }

    }
}
