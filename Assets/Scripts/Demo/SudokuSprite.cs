using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SudokuSprite : MonoBehaviour
{
    public int ID, Row, Col;
    public Color HighlightColor, TextColor;
    public TextMeshPro Number, Possiblilities;

    Color colorBuffer;
    SpriteRenderer spriteRenderer;

    bool highlighted;

    public void Start() {
        spriteRenderer ??= gameObject.GetComponent<SpriteRenderer>();
        colorBuffer = spriteRenderer.color;
    }

    public void ToggleHighlight() {
        if (!highlighted) EnableHighlight(); else DisableHighlight();
    }

    public void EnableHighlight() {
        spriteRenderer ??= gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer.color != HighlightColor) colorBuffer = spriteRenderer.color;
        spriteRenderer.color = HighlightColor;
        highlighted = true;
    }

    public void DisableHighlight() {
        spriteRenderer ??= gameObject.GetComponent<SpriteRenderer>();
        if (highlighted) spriteRenderer.color = colorBuffer;
        highlighted = false;
    }

    public void ShowNumber(uint number) {
        Number.text = number.ToString();
        Number.color = TextColor;
        Number.gameObject.SetActive(true);
    }

    public void ShowPossies(List<uint> possibilities) {
        var possieText = "";
        foreach (var possie in possibilities)
        {
            possieText += possie.ToString();
            possieText += " ";
        }
        Possiblilities.text = possieText.Remove(possieText.Length - 1);
        Possiblilities.color = TextColor;
        Possiblilities.gameObject.SetActive(true);
    }
}
