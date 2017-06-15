using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Einkaufslisten_Template10.Models.Objects;
using Windows.Storage;
using System.Diagnostics;
using System.Collections.ObjectModel;

namespace Einkaufslisten_Template10.Models.Source
{
    class FileReaderEinkaufslisten : ISource <Einkaufsliste>
    {
        
        private StorageFolder folder;
        //muss Files einlesen! 
        public FileReaderEinkaufslisten()
        {
            folder = Windows.Storage.ApplicationData.Current.LocalFolder;
        }

        public async Task<Einkaufsliste> Create(Einkaufsliste obj)
        {
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            int id = files.Count;
            await folder.CreateFileAsync(id + "_" + obj.Name);
            obj.Id = id;
            return obj;
        }

        public async void Delete(Einkaufsliste einkaufsliste)
        {
            StorageFile f = await folder.GetFileAsync(einkaufsliste.Id + "_" + einkaufsliste.Name);
            await f.DeleteAsync();
        }

        public async Task<Collection<Einkaufsliste>> GetRepo()
        {
            Collection<Einkaufsliste> list = new Collection<Einkaufsliste>();
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                string fileName = file.Name;
                if (fileName.Contains(".tmp")) {
                    fileName = fileName.Replace(".tmp", "");
                    string[] splittedNames = fileName.Split('_');
                    int id = Int32.Parse(splittedNames[0]);
                    Einkaufsliste e = new Einkaufsliste(id, splittedNames[1], new DateTime());
                    list.Add(e);
                }
            }
            return list;
        }

        public async void Update(Einkaufsliste einkaufsliste)
        {
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                string fileName = file.Name;
                string[] splittedNames = fileName.Split('_');
                int id = Int32.Parse(splittedNames[0]);
                if (id == einkaufsliste.Id)
                {
                    await file.MoveAsync(folder, id + "_" + einkaufsliste.Name);
                }
            }
        }
    }
}
