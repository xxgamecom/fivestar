using ETModel;

namespace ETHotfix
{
	[ObjectSystem]
	public class RealmGateAddressComponentAwakeSystem : StartSystem<RealmGateAddressComponent>
	{
		public override void Start(RealmGateAddressComponent self)
		{
			self.Start();
		}
	}
	
	public static class RealmGateAddressComponentSystem
	{
		public static void Start(this RealmGateAddressComponent component)
		{
			StartConfig[] startConfigs = StartConfigComponent.Instance.GetAll();
			foreach (StartConfig config in startConfigs)
			{
				if (!config.AppType.Is(AppType.Gate))
				{
					continue;
				}
				
				component.GateAddress.Add(config);
			}
		}
	}
}
