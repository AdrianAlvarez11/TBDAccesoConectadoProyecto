using CampeonesLoL.Models;
using CampeonesLoL.Repositories;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace CampeonesLoL.Viewmodels
{
    public enum Carriles { Superior, Jungla, Central, Inferior, Soporte }
    public enum Roles { Asesino, Peleador, Mago, Tirador, Apoyo, Tanque }
    public enum Dificultad { Baja, Media, Alta }
    public class CampeonesViewmodel : INotifyPropertyChanged
    {
        CampeonesRepository repos = new();
        public ObservableCollection<Campeon> Campeones { get; set; } = new();
        public Campeon CampeonS { get; set; }

        public string? Error { get; set; }
        public IEnumerable<Carriles> LCarriles => (IEnumerable<Carriles>)Enum.GetValues(typeof(Carriles));
        public IEnumerable<Roles> LRoles => (IEnumerable<Roles>)Enum.GetValues(typeof(Roles));
        public IEnumerable<Dificultad> LDificultad => (IEnumerable<Dificultad>)Enum.GetValues(typeof(Dificultad));
        public string Vista { get; set; } = "Principal";
        public ICommand VerAgregarCommand { get; set; }
        public ICommand AgregarCommand { get; set; }
        public ICommand VerEstadisticasCommand { get; set; }
        public ICommand VerDetallesCommand { get; set; }
        public ICommand EliminarCommand { get; set; }
        public ICommand CancelarCommand { get; set; }

        public long ConteoTotal { get; set; }
        public long NumSuperior { get; set; }
        public long NumJungla { get; set; }
        public long NumCentral { get; set; }
        public long NumInferior { get; set; }
        public long NumSoporte { get; set; }
        public long NumAsesino { get; set; }
        public long NumPeleador { get; set; }
        public long NumMago { get; set; }
        public long NumTirador { get; set; }
        public long NumApoyo { get; set; }
        public long NumTanque { get; set; }
        public long NumFacil { get; set; }
        public long NumMedia { get; set; }
        public long NumDificil { get; set; }


        public CampeonesViewmodel()
        {
            VerAgregarCommand = new RelayCommand(VerAgregar);
            AgregarCommand = new RelayCommand(Agregar);
            VerEstadisticasCommand = new RelayCommand(VerEstadisticas);
            VerDetallesCommand = new RelayCommand<Campeon>(VerDetalles);
            EliminarCommand = new RelayCommand<Campeon>(Eliminar);
            CancelarCommand = new RelayCommand(Cancelar);
            Actualizar();
        }

        private void VerDetalles(Campeon s)
        {
            Vista = "Detalles";
            CampeonS = s;
            Actualizar();
        }

        private void Cancelar()
        {
            Vista = "Principal";
            Actualizar();
        }

        private void Eliminar(Campeon? campeon)
        {
            if (campeon != null)
            {
                repos.Eliminar(campeon);
                Vista = "Principal";
                Actualizar();
            }
        }

        private void VerEstadisticas()
        {
            Vista = "Estadisticas";
            Actualizar();
        }

        private void Agregar()
        {
            if (!repos.Validar(CampeonS, out string error))
            {
                repos.Agregar(CampeonS);
                Vista = "Principal";
                Actualizar();
            }
            Error = error;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        private void VerAgregar()
        {
            CampeonS = new();
            Vista = "Agregar";
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));

        }

        public void Actualizar()
        {
            Campeones.Clear();

            foreach (var item in repos.GetAllCampeones())
            {
                Campeones.Add(item);
            }

            ActualizarEstadisticas();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        public void ActualizarEstadisticas()
        {
            ConteoTotal = repos.GetConteoTotal();

            NumSuperior = repos.GetConteoCarriles(Carriles.Superior);
            NumJungla = repos.GetConteoCarriles(Carriles.Jungla);
            NumCentral = repos.GetConteoCarriles(Carriles.Central);
            NumInferior = repos.GetConteoCarriles(Carriles.Inferior);
            NumSoporte = repos.GetConteoCarriles(Carriles.Soporte);

            NumAsesino = repos.GetConteoRoles(Roles.Asesino);
            NumPeleador = repos.GetConteoRoles(Roles.Peleador);
            NumMago = repos.GetConteoRoles(Roles.Mago);
            NumTirador = repos.GetConteoRoles(Roles.Tirador);
            NumApoyo = repos.GetConteoRoles(Roles.Apoyo);
            NumTanque = repos.GetConteoRoles(Roles.Tanque);

            NumFacil = repos.GetConteoDificultad(Dificultad.Baja);
            NumMedia = repos.GetConteoDificultad(Dificultad.Media);
            NumDificil = repos.GetConteoDificultad(Dificultad.Alta);
        }
    }
}
