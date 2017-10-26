using System;
using UnityEngine;

namespace X.Engine
{
    [Serializable]
    public class XControllerParameters
    {
        [Header("重力")]
        public  float Gravity = 2000f;
        [Header("速度")]
        public  float Velocity = 500.0f;
        [Header("弹跳高度")]
        public  float Jump_Height = 50.0f;
        [Header("弹跳向上推进的高度")]
        public  float Jump_Boost_Height = 55.0f;
        [Header("下落速度")]
        public  float Fall_Velocity = 900.0f;

        //public  float AccelerationOnGroundTime = 0.1f;
        //public  float AccelerationOnGround = Velocity / AccelerationOnGroundTime;
        [Header("地面加速度")]
        public float AccelerationOnGround = 10000f;

        //public  float AccelerationInAirTime = 0.25f;
        //public  float AccelerationInAir = Velocity / AccelerationInAirTime;
        [Header("空中加速度")]
        public  float AccelerationInAir = 4000f;
    }
}
