using FileScanner.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace FileScanner.ViewModels
{
    public class Items : BaseViewModel
    {

        private string folderItems;

        private string image;

        public string FolderItems
        {
            get => folderItems;
            set
            {
                folderItems = value;
                OnPropertyChanged();
            }
        }


        public string Image
        {
            get => image;
            set
            {
               image = value;
                OnPropertyChanged();
            }
        }



    }
    public class MainViewModel : BaseViewModel
    {

        public DelegateCommand<string> OpenFolderCommand { get; private set; }
        public DelegateCommand<string> ScanFolderCommand { get; private set; }

        private string selectedFolder;

        private ObservableCollection<Items> itemsF = new ObservableCollection<Items>();


        public ObservableCollection<Items> ItemsF
        {
            get => itemsF;
            set
            {
                itemsF = value;
                OnPropertyChanged();
            }
        }


      

        public string SelectedFolder
          {
              get => selectedFolder;
              set
              {
                  selectedFolder = value;
                  OnPropertyChanged();
                  ScanFolderCommand.RaiseCanExecuteChanged();
              }
          }

        public MainViewModel()
        {
            OpenFolderCommand = new DelegateCommand<string>(OpenFolder);
            ScanFolderCommand = new DelegateCommand<string> (ScanFolderAsync, CanExecuteScanFolder);
          
        }

        private bool CanExecuteScanFolder(string obj)
        {
            return !string.IsNullOrEmpty(SelectedFolder);
        }

        private void OpenFolder(string obj)
        {
         
                using (var fbd = new FolderBrowserDialog())
                {
                    if (fbd.ShowDialog() == DialogResult.OK)
                    {
                        SelectedFolder = fbd.SelectedPath;
                    }
                }
         
        }

     
       
        private async void ScanFolderAsync(string dir)
        {
             
            try
            {
               
                ItemsF = new ObservableCollection<Items>(await Task.Run(() => GetDirs(dir)));

               

                foreach (var item in Directory.EnumerateFiles(dir, "*"))
                {
                    Items it = new Items();
                    it.FolderItems = item;
                    it.Image = "/Image/file.png";
                    ItemsF.Add(it);
                    Console.Write(it.FolderItems);
                }
            }
            catch(Exception) {

                Items it = new Items();
                it.FolderItems = "you don't have the permissions to scan this file.";
                it.Image = "";
                itemsF.Clear();
                ItemsF.Add(it);
            }
            
        }

      

          
        

         IEnumerable<Items> GetDirs(string dir)
        {

          
            foreach (var d in Directory.EnumerateDirectories(dir, "*"))
            {
                Items it = new Items();
                it.FolderItems = d;
                it.Image = "/Image/dossier.png";
                yield return it;
               
            }
        }

     

        ///TODO : Tester avec un dossier avec beaucoup de fichier
        ///TODO : Rendre l'application asynchrone
        ///TODO : Ajouter un try/catch pour les dossiers sans permission


    }
}
