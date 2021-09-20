using AdvancedDSA.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using SalesManProblem.Views;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SalesManProblem.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {
        private int citiesCount = 10;

        private Map mapGraph;
        private string pathLengthString;
        private string avgPathLengthString;

        // set from Canvas
        private int mapWidth;
        private int mapHeight;

        public MainWindowViewModel()
        {
            GenerateRandomMap = new RelayCommand(GenerateRandomMapHandler);
            RunAlgorithm = new AsyncRelayCommand(RunAlgorithmAsync);
            GenerateCircularCities = new RelayCommand(PerformGenerateCircularCities);
            LoadedCommand = new RelayCommand(WindowLoadedHandler);
        }

        private void WindowLoadedHandler()
        {
            GenerateRandomMapHandler();
        }

        public int CitiesCount { get => citiesCount; set => SetProperty(ref citiesCount, value); }
        public Map MapGraph { get => mapGraph; set => SetProperty(ref mapGraph, value); }
        public int MapWidth { get => mapWidth; set => mapWidth = value; }
        public int MapHeight { get => mapHeight; set => mapHeight = value; }


        public IRelayCommand GenerateRandomMap { get; }

        public IAsyncRelayCommand RunAlgorithm { get; }

        public IRelayCommand GenerateCircularCities { get; }


        public IRelayCommand LoadedCommand { get; }

        public string PathLengthString { get => pathLengthString; set => SetProperty(ref pathLengthString, value); }

        public string AvgPathLengthString { get => avgPathLengthString; set => SetProperty(ref avgPathLengthString, value); }


        private string logs = "reports here";

        public string Logs { get => logs; set => SetProperty(ref logs, value); }


        private async Task RunAlgorithmAsync()
        {
            //var options = new GNAOptions(iterations,
            //    populationSize,
            //    crossoverPercentage / 100D,
            //    mutationPercentage / 100D,
            //    elitismPercentage / 100D,
            //    numberOfGenerationsPerIeration
            //    );
            //var choices = new GNAChoices(SelectedFitnessChoice, SelectedElitismChoice, SelectedSelectionChoice, SelectedCrossOverChoice, SelectedMutaionChoice);
            //GNAlgorithm algorithm = new GNAlgorithm(choices, options);
            //AvgPathLengthString = $"wait ...";
            //PathLengthString = $"";
            //var results = await Task.Run(() => algorithm.Execute(Map.Create(Cities)));
            //AvgPathLengthString = $"Avg Path Length: {results.AveragePathLength}";
            //PathLengthString = $"Best Path Length: {(int)results.MapPath.PathLength}";
            //
            //
            //LogInfo(results);
            //
            //WeakReferenceMessenger.Default.Send(results.MapPath);

        }

//        public void LogInfo(object results)
//        {
//            string logstr = $@"Algorihm Results.
//--------------------------------------------------------
//Best Path Length-----------:{results.MapPath.PathLength}
//Average Path Length--------:{results.AveragePathLength}
//Best Execution Time--------:{results.IterationElapsed}
//Total Execution Time-------:{results.TotalElapsed}
//Path-----------------------:
//{string.Join($"        {Environment.NewLine}", results.MapPath.Positions.Select(p => $"{Cities.FirstOrDefault(c => c.Position == p).Name ?? "C"}:({p.X} , {p.Y})"))}

//";
//            Logs = logstr;
//        }

        private void PerformGenerateCircularCities()
        {
            int radius = (int)((double)Math.Min(MapWidth, MapHeight) / 2.2);
            int centerX = MapWidth / 2;
            int centerY = MapHeight / 2;
            double step = (2 * Math.PI) / citiesCount;

            var points = Enumerable.Range(0, citiesCount)
                .Select((v, i) =>
                {
                    int x = centerX + (int)(Math.Cos(i * step) * radius);
                    int y = centerY + (int)(Math.Sin(i * step) * radius);
                    return City.Create($"{i + 1}", new System.Drawing.Point(x, y));
                }).ToList();
            
            //WeakReferenceMessenger.Default.Send(points);
        }

        private void GenerateRandomMapHandler()
        {
            MapGraph = Map.CreateRandomMap(mapWidth, mapHeight, CitiesCount, 30,citiesCount/4);
            WeakReferenceMessenger.Default.Send(MapGraph);
        }

        private RelayCommand openMapCreator;
        public ICommand OpenMapCreator => openMapCreator ??= new RelayCommand(PerformOpenMapCreator);

        private void PerformOpenMapCreator()
        {
            WeakReferenceMessenger.Default.Send(new WindowShowMessage<MapCreator>());


        }



    }
}
