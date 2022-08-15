using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;
public class Installer : MonoInstaller<Installer>
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private AbstractInventory[] inventories;
    [SerializeField] private AssetDatabase assetDatabase;
    [SerializeField] private ConsumableManagerConfig consumableManagerConfig;

    public override void InstallBindings()
    {
        InstallSystem();

        Container.Bind<AssetDatabase>().FromInstance(assetDatabase);
        Container.Bind<ConsumableManagerConfig>().FromInstance(consumableManagerConfig);

        InstallSignals();
    }

    private void InstallSystem()
    {
        Container.BindInterfacesAndSelfTo<TradeDialogHandler>().AsSingle();
        Container.BindInterfacesAndSelfTo<DragAndDropSystem>().AsSingle();
        Container.BindInterfacesAndSelfTo<AbstractInventory[]>().FromInstance(inventories);

        var coinView = new CoinsView(coinText);
        Container.BindInterfacesAndSelfTo<CoinsView>().FromInstance(coinView).AsSingle();
    }

    private void InstallSignals()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<OnDropItem>();
    }
}
