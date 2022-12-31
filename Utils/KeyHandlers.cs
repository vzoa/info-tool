using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Input;
using Windows.System;

namespace ZoaInfoTool.Utils;

public static class KeyHandlers
{
    public static KeyEventHandler NewOnEnterCommandHandler(IRelayCommand command)
    {
        return new KeyEventHandler((sender, e) =>
        {
            if (e.Key == VirtualKey.Enter)
            {
                command.Execute(null);
            }
        });
    }
}
