using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShadow : MonoBehaviour
{
    private CharacterController cc;

    #region Shadow Variables

    [Header("Shadow")]
    [SerializeField] private Transform shadow;
    [SerializeField] private SpriteRenderer srShadow;

    #endregion

    #region Position Variables

    private Vector3 shadowPos;
    private Transform swordPos;

    #endregion

    #region Distance Variables

    private float distance;
    private float fixedDistance;
    private float distDifference;

    #endregion

    #region Alpha & Color Variables

    [Header("Alpha")]
    [SerializeField] private float alphaChange = 1;

    private Color shadowColor;
    private Color fixedShadowColor;

    #endregion

    #region Scale Variables

    [Header("Scale")]
    [SerializeField] private float scaleChange = 1;

    private Vector3 fixedShadowScale;
    private Vector3 shadowScale;

    #endregion

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        shadowColor = srShadow.GetComponent<SpriteRenderer>().color;
        fixedShadowColor = srShadow.GetComponent<SpriteRenderer>().color;
        swordPos = GetComponent<Transform>();
    }

    private void Start()
    {
        shadowPos = shadow.position;
        fixedShadowScale = shadow.localScale;
        shadowScale = fixedShadowScale;
        fixedDistance = shadowPos.y - swordPos.position.y;
    }

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            #region Keep shadow Y position

            if (shadow.position.y == shadowPos.y) // to always store the shadow position when grounded
                shadowPos = shadow.position;

            else
            {
                shadowPos = new Vector3(shadow.position.x, shadowPos.y, 0); // to change the X position, but keep the Y position when the player's on air
                shadow.position = shadowPos;
            }

            #endregion

            ///////////////////////////////////////////////////////////////////////////////////////////////////

            #region Change shadow Alpha and Shadow Scale

            distance = shadowPos.y - swordPos.position.y; // always calculate the distance between sword and shadow Y positions

            if (distance != fixedDistance && !cc.grounded) // change the values if there's difference between distances
            {
                distDifference = fixedDistance - distance; // calculate the difference between the initial distance and the actual distance

                // shadow alpha
                shadowColor.a = fixedShadowColor.a - (distDifference * alphaChange); // change the color.alpha value by the difference of the distances
                srShadow.color = shadowColor;

                // shadow scale
                shadowScale = new Vector3(shadow.localScale.x, fixedShadowScale.y + (distDifference * scaleChange), 0); // change the scale value by the difference of the distances
                shadow.localScale = shadowScale;
            }

            else // store the original values
            {
                srShadow.color = fixedShadowColor;
                shadow.localScale = fixedShadowScale;
            }

            #endregion
        }
    }
}
