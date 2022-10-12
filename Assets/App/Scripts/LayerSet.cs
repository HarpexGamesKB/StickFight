using UnityEngine;

public class LayerSet : Singleton<LayerSet>
{
    public LayerMask ObstacleLayer;
    public LayerMask GroundLayer;
    public LayerMask EnemyLayer;
    public LayerMask GroupMemberLayer;
    public LayerMask TaxiCustomerLayer;
}