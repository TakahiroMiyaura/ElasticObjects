// Copyright (c) 2021 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using Microsoft.MixedReality.Toolkit.Experimental.Physics;
using UnityEngine;

public class ElasticObjects : MonoBehaviour
{
    [SerializeField]
    // The elastic properties of our springs.
    private ElasticProperties elasticProperties = new ElasticProperties
    {
        Mass = 0.4f,
        HandK = 4.0f,
        EndK = 4.0f,
        SnapK = 1.0f,
        Drag = 0.6f
    };

    private VolumeElasticSystem elasticSystem;

    public Transform IndexTipObject;
    private bool isCollision;

    [SerializeField]
    public float lefrectAmplify = 1.0f;

    [SerializeField]
    public float reflectionTime = 1.0f;

    private Rigidbody rigidbodyComp;
    private float timer;

    // The elastic extent for our backplate scaling.
    private VolumeElasticExtent volumeExtent = new VolumeElasticExtent
    {
        SnapPoints = new Vector3[0]
    };

    // Start is called before the first frame update
    private void Start()
    {
        rigidbodyComp = GetComponent<Rigidbody>();
        // Vector3の場合は以下の実装。初期位置と速度を設定し、ComputeIterationメソッドで次の時間の状態を計算する。
        // floatの場合は LinearElasticSystem、　Quaternionの場合はQuaternionElasticSystemを使用する。パラメータなどは大きな違いはないです。
        elasticSystem = new VolumeElasticSystem(new Vector3(0, 0, .5f), Vector3.zero, volumeExtent, elasticProperties);
    }

    private void Update()
    {
        if (!isCollision)
        {
            transform.position = elasticSystem.ComputeIteration(IndexTipObject.position, Time.deltaTime);
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > reflectionTime)
            {
                timer = 0;
                transform.position = elasticSystem.ComputeIteration(transform.position, Time.deltaTime);
                isCollision = false;
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!isCollision && other.gameObject.layer == 31)
        {
            isCollision = true;
            var norm = other.contacts[0].normal;
            var vel = rigidbodyComp.velocity.normalized;
            vel += new Vector3(norm.x * 2, norm.y * 2, norm.z * 2);
            rigidbodyComp.velocity = vel * lefrectAmplify;
        }
    }
}