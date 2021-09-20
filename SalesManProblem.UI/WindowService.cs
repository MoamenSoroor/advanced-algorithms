using Microsoft.Toolkit.Mvvm.Messaging;
using SalesManProblem.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SalesManProblem
{
    public class WindowService
    {
        public WindowService()
        {
            WeakReferenceMessenger.Default.Register<WindowShowMessage<MapCreator>>(this, MapCreatorHandler);
        }

        private void MapCreatorHandler(object recipient, WindowShowMessage<MapCreator> message)
        {
            MapCreator creator = new MapCreator();
            creator.Show();
        }
    }

    public class WindowShowMessage<T> where T: Window
    {

    }
}
