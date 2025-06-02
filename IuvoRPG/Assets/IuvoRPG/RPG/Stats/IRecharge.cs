using UnityEngine;

public interface IRecharge 
{
    public float _RechargeAmount { get; set; }
    public float _RechargeSpeed { get; set; }


    public void Recharge();

}
