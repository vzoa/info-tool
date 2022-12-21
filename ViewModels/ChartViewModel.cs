using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using ZoaInfoTool.Models;
using ZoaInfoTool.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace ZoaInfoTool.ViewModels
{
    public partial class ChartViewModel : ObservableObject
    {       
        [ObservableProperty]
        private string airport;

        public ObservableGroupedCollection<ChartType, Chart> ChartsGrouped { get; private set; }

        private IChartService ChartFetcher { get; set; }

        public ChartViewModel(IChartService chartFetcher)
        {
            ChartFetcher = chartFetcher;
            ChartsGrouped = new ObservableGroupedCollection<ChartType, Chart>();
        }

        [RelayCommand]
        private async void FetchCharts()
        {   
            try
            {   
                // Fetch charts
                List<Chart> newCharts = await ChartFetcher.FetchChartsAsync(Airport);

                // Group the new charts by Chart Type
                var grouped = newCharts.GroupBy(static x => x.Type).OrderBy(static g => g.Key);
                ChartsGrouped = new ObservableGroupedCollection<ChartType, Chart>(grouped);
                OnPropertyChanged(nameof(ChartsGrouped));
            }
            catch (Exception)
            {
                // Do nothing on exception right now
                // Todo: maybe create popup?
            }
        }
    }
}
