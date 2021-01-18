// Copyright (c) 2021 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Microsoft.MixedReality.Toolkit.Input;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class IndexTipsTracer : MonoBehaviour
{
    private void Update()
    {
        //ハンドトラッキングの情報から人差し指の先の空間座標を取得する。
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Any, out var pose))
        {
            //コンポーネントを設定したGameObjectの座標を指先に合わせる
            //この座標をElasticSystemの終点情報として利用することで、指を中心にオブジェクトが動き回ります。
            transform.position = pose.Position;
        }
    }
}