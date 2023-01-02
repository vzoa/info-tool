// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Octokit;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Page = Microsoft.UI.Xaml.Controls.Page;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ZoaInfoTool.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class StartupPage : Page
{
    public StartupPage()
    {
        this.InitializeComponent();
        Loaded += (_, _) => CheckForUpdate();
    }

    public Version AssemblyVersion => Assembly.GetEntryAssembly().GetName().Version;
    
    public async Task<Release?> GetLatestRelease()
    {
        var client = new GitHubClient(new ProductHeaderValue("ZoaInfoTool"));
        var releases = await client.Repository.Release.GetAll("vzoa", "info-tool");

        if (releases.Count > 0)
        {
            var latest = releases[0];
            var latestVersion = Version.Parse(latest.TagName.Replace("v", ""));
            return latestVersion > AssemblyVersion ? latest : null;
        }

        return null;
    }

    public async Task ShowUpdateDialog(Release latest)
    {
        var versionString = latest.TagName.Replace("v", "");
        ContentDialog updateDialog = new()
        {
            Title = "Update Available",
            Content = $"Version {versionString} is now available",
            PrimaryButtonText = "Downloads Page",
            CloseButtonText = "Don't Update",
            PrimaryButtonCommand = OpenUrlCommand,
            PrimaryButtonCommandParameter = latest.HtmlUrl
        };
        updateDialog.XamlRoot = this.Content.XamlRoot;

        await updateDialog.ShowAsync();
    }

    public async void CheckForUpdate()
    {
        var latest = await GetLatestRelease();
        if (latest is not null) await ShowUpdateDialog(latest);
    }

    [RelayCommand]
    private static void OpenUrl(string url)
    {
        Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
    }
}
