using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Einkaufslisten_Template10.Models.Objects;
using Windows.Storage;
using System.Collections.ObjectModel;

namespace Einkaufslisten_Template10.Models.Source
{
    class FileReaderProdukt : ISource<Produkt>
    {
        private StorageFile file;
        private Collection<Produkt> produkte;
        public FileReaderProdukt(int id)
        {
            produkte = new Collection<Produkt>();
            GetFile(id);
        }

        private async void GetFile(int id)
        {
            StorageFolder folder = Windows.Storage.ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> files = await folder.GetFilesAsync();
            foreach (StorageFile file in files)
            {
                string fileName = file.Name;
                string[] splittedNames = fileName.Split('_');
                if (id == Int32.Parse(splittedNames[0])){
                    this.file = file;
                    break;
                }
                
            }
        }
        private async void FillRepository()
        {
            String fileContent = await Windows.Storage.FileIO.ReadTextAsync(file);
            String[] rows = fileContent.Split('\n');
            foreach (String row in rows)
            {
                String[] attributes = row.Split(';');
                /*
                 *  attributes[0] = id
                 *  attributes[1] = name
                 *  attributes[2] = anzahl
                 *  attributes[3] = mengenBezeichnung
                 */
                Produkt produkt = new Produkt(Int32.Parse(attributes[0]), attributes[1], Int32.Parse(attributes[2]), attributes[3]);
                produkte.Add(produkt);
            }
        }
        public async Task<Produkt> Create(Produkt produkt)
        {
            if (!produkte.Contains(produkt)) { 
            int id = produkte.Count;
            produkt.id_item = id;
            produkte.Add(produkt);
            } else
            {
                return produkte.FirstOrDefault(x => x.id_item == produkt.id_item && x.name.Equals(produkt.name));
            }
            return produkt;

        }

        public void Delete(Produkt produkt)
        {
            if (produkte.Contains(produkt))
            {
                produkte.Remove(produkt);
            }
        }

        public async Task<Collection<Produkt>> GetRepo()
        {
            return produkte;
        }

        public void Update(Produkt produkt)
        {
            if (produkte.Contains(produkt))
            {
                Produkt oldProdukt = produkte.FirstOrDefault(x => x.id_item == produkt.id_item);
                produkte.Remove(oldProdukt);
                produkte.Add(produkt);
            } else
            {
                Create(produkt);
            }
        }
    }
}
