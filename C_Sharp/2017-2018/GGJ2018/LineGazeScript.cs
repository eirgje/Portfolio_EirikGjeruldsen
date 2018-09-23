using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tobii.Gaming;

[RequireComponent(typeof(GazeAware))]
[RequireComponent(typeof(MeshRenderer))]
public class LineGazeScript : MonoBehaviour {

    public Color selectionColor;

    private GazeAware mGazeAwareComponent;
    private MeshRenderer mMeshRenderer;

    private Color mDeselectionColor;
    private Color mLerpColor;
    private float mFadeSpeed = 0.1f;

    void Start ()
    {
        mGazeAwareComponent = GetComponent<GazeAware>();
        mMeshRenderer = GetComponent<MeshRenderer>();
        mLerpColor = mMeshRenderer.material.color;
        mDeselectionColor = Color.black;
    }
	
	void Update () {

        if (mMeshRenderer.material.color != mLerpColor)
            mMeshRenderer.material.color = Color.Lerp(mMeshRenderer.material.color, mLerpColor, mFadeSpeed);

        if (mGazeAwareComponent.HasGazeFocus)
            SetLerpColor(selectionColor);
        else
            SetLerpColor(mDeselectionColor);
    }

    public void SetLerpColor(Color lerpColor)
    {
        this.mLerpColor = lerpColor;
    }
}
