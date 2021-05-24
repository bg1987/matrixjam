using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MatrixJam.TeamMeta
{
    public class TmpFader : MonoBehaviour
    {
        [SerializeField] TextMeshPro text;
        private Color32[] newVertexColors;
        private Color32[] newVertexColorsCopy = new Color32[0];
        private Coroutine[] characterFadeRoutines= new Coroutine[0];
        private Coroutine FadeOverallAlpha;
        Material fontMaterial;
        // Start is called before the first frame update
        void Awake()
        {
            text.ForceMeshUpdate(true);
            fontMaterial = text.fontMaterial;
            RestoreVertexColors();
            //newVertexColors = text.textInfo.meshInfo[0].colors32;
            //string name = gameObject.name;
            //newVertexColorsCopy = new Color32[newVertexColors.Length];
            //for (int i = 0; i < newVertexColors.Length; i++)
            //    newVertexColorsCopy[i] = newVertexColors[i];
            //characterFadeRoutines = new Coroutine[text.textInfo.characterCount];
            //FadeOut(0f, 0f);
            //FadeOutOverallAlpha(0f);
            //yield return new WaitForSeconds(1);
            //FadeIn(0.5f, 0.1f);
            //FadeInOverallAlpha(0.1f);
            //FadeIn(0.1f, 0.1f);


        }
        // Update is called once per frame
        void Update()
        {
            if (text.havePropertiesChanged)
            {
                text.ForceMeshUpdate(true);
                RestoreVertexColors();
            }
        }
        private void OnEnable()
        {
            //Debug.Log("On enable was called");
            text.ForceMeshUpdate(true);
            RestoreVertexColors();
        }

        public void FadeOut(float totalDuration, float characterDuration)
        {
            if(totalDuration == 0)
            {
                StopAllCoroutines();
                FadeOutInstantly();
                return;
            }
            StartCoroutine(AlphaChangeRoutine(0, totalDuration, characterDuration));
        }
        public void FadeOutLines(float totalDuration, float characterDuration)
        {
            if (totalDuration == 0)
            {
                StopAllCoroutines();
                FadeOutInstantly();
                return;
            }
            StartCoroutine(LinesAlphaChangeRoutine(0, totalDuration, characterDuration));
        }
        public void FadeIn(float totalDuration, float characterDuration)
        {
            if (totalDuration == 0)
            {
                StopAllCoroutines();
                FadeInInstantly();
                return;
            }
            StartCoroutine(AlphaChangeRoutine(1, totalDuration, characterDuration));
        }
        public void FadeInLines(float totalDuration, float characterDuration)
        {
            if (totalDuration == 0)
            {
                StopAllCoroutines();
                FadeInInstantly();
                return;
            }
            StartCoroutine(LinesAlphaChangeRoutine(1, totalDuration, characterDuration));
        }
        IEnumerator AlphaChangeRoutine(float targetAlpha, float duration, float characterDuration)
        {
            targetAlpha *= 255;
            byte targetAlphaByte = (byte)targetAlpha;
            text.ForceMeshUpdate(true);
            RestoreVertexColors();

            var textInfo = text.textInfo;
            var characterCount = textInfo.characterCount;
            double delayBetweenCharacters = duration / characterCount;

            int i = 0;

            double delayBetweenCharactersT = 0;

            if (textInfo.characterInfo[i].isVisible)
            {
                int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                if (characterFadeRoutines[i]!=null)
                {
                    StopCoroutine(characterFadeRoutines[i]);
                }
                CharacterAlphaChange(i, vertexIndex, targetAlphaByte, characterDuration);
            }
            while (i < characterCount)
            {
                delayBetweenCharactersT = delayBetweenCharactersT - Mathf.Floor((float)delayBetweenCharactersT);
                while (delayBetweenCharactersT < 1)
                {
                    text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    delayBetweenCharactersT += Time.deltaTime / delayBetweenCharacters;

                    if (delayBetweenCharacters == 0)
                        delayBetweenCharactersT = 1;
                    else
                        yield return null;
                }
                while (delayBetweenCharactersT >= 1)
                {
                    delayBetweenCharactersT--;
                    i++;
                    if (i == characterCount)
                        break;
                    text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                    if (textInfo.characterInfo[i].isVisible)
                    {
                        int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                        CharacterAlphaChange(i, vertexIndex, targetAlphaByte, characterDuration);
                    }

                }
            }
            int lastVisibleLineIndex = textInfo.lineCount;
            while (lastVisibleLineIndex > 0)
            {
                lastVisibleLineIndex--;
                if (textInfo.lineInfo[lastVisibleLineIndex].visibleCharacterCount > 0)
                {
                    break;
                }
            }
            int lastVisibleCharacterIndex = textInfo.lineInfo[lastVisibleLineIndex].lastVisibleCharacterIndex;
            float lastCharacterAlpha = (newVertexColors[textInfo.characterInfo[lastVisibleCharacterIndex].vertexIndex].a / 255f);
            float remainingDuration = characterDuration * Mathf.Abs(targetAlpha - lastCharacterAlpha);
            while (remainingDuration > 0)
            {
                text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                remainingDuration -= Time.deltaTime;

                yield return null;

            }
            //yield return null;

            text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            yield break;
        }
        IEnumerator LinesAlphaChangeRoutine(float targetAlpha, float duration, float characterDuration)
        {
            targetAlpha *= 255;
            byte targetAlphaByte = (byte)targetAlpha;
            text.ForceMeshUpdate(true);
            RestoreVertexColors();

            var textInfo = text.textInfo;
            double delayBetweenCharacters;
            int maxLineCharacterCount = 0;
            int maxLineLastIndex = 0;
            for (int i = 0; i < textInfo.lineCount; i++)
            {
                if (textInfo.lineInfo[i].visibleCharacterCount > maxLineCharacterCount)
                {
                    maxLineCharacterCount = textInfo.lineInfo[i].visibleCharacterCount;
                    maxLineLastIndex = textInfo.lineInfo[i].lastVisibleCharacterIndex;
                }
            }
            delayBetweenCharacters = duration / maxLineCharacterCount;

            for (int i = 0; i < textInfo.lineCount; i++)
            {
                StartCoroutine(LineAlphaChangeRoutine(targetAlphaByte, duration, characterDuration, i, delayBetweenCharacters));
            }
            float t = 0;
            while (t < 1)
            {

                text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                yield return null;
                t += Time.deltaTime / duration;
            }
            int maxLineCharacterVertexIndex = textInfo.characterInfo[maxLineLastIndex].vertexIndex;

            float lastCharacterAlpha = (newVertexColors[maxLineCharacterVertexIndex].a / 255f);
            float remainingDuration = characterDuration * Mathf.Abs(targetAlpha - lastCharacterAlpha);
            while (remainingDuration> 0)
            {
                text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                remainingDuration -= Time.deltaTime;

                yield return null;

            }
            //yield return null;

            text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

        }
        void CharacterAlphaChange(int index, int vertexIndex, byte targetAlpha, float duration)
        {
            if (characterFadeRoutines[index] != null)
            {
                StopCoroutine(characterFadeRoutines[index]);
            }
            if (targetAlpha == newVertexColors[vertexIndex].a)
                return;
            characterFadeRoutines[index] = StartCoroutine(CharacterAlphaChangeRoutine(targetAlpha, vertexIndex, duration, 0));
        }
        IEnumerator LineAlphaChangeRoutine(byte targetAlpha, float duration, float characterDuration, int lineIndex, double delayBetweenCharacters)
        {
            //RestoreVertexColors();

            var textInfo = text.textInfo;
            var characterCount = textInfo.lineInfo[lineIndex].characterCount;

            int startIndex = textInfo.lineInfo[lineIndex].firstVisibleCharacterIndex;
            int endIndex = textInfo.lineInfo[lineIndex].lastVisibleCharacterIndex;

            double delayBetweenCharactersT = 0;

            if (textInfo.characterInfo[startIndex].isVisible)
            {
                int vertexIndex = textInfo.characterInfo[startIndex].vertexIndex;
                
                CharacterAlphaChange(startIndex, vertexIndex, targetAlpha, characterDuration);
            }
            var i = startIndex;
            while (i <= endIndex)
            {
                delayBetweenCharactersT = delayBetweenCharactersT - Mathf.Floor((float)delayBetweenCharactersT);
                while (delayBetweenCharactersT < 1)
                {
                    delayBetweenCharactersT += Time.deltaTime / delayBetweenCharacters;

                    if (delayBetweenCharacters == 0)
                        delayBetweenCharactersT = 1;
                    else
                        yield return null;
                }
                while (delayBetweenCharactersT >= 1)
                {
                    delayBetweenCharactersT--;
                    i++;
                    if (i > endIndex)
                        break;
                    if (textInfo.characterInfo[i].isVisible)
                    {
                        int vertexIndex = textInfo.characterInfo[i].vertexIndex;
                        CharacterAlphaChange(i, vertexIndex, targetAlpha, characterDuration);
                    }
                    
                }
            }


            yield return null;
        }
        IEnumerator CharacterAlphaChangeRoutine(byte targetAlpha, int vertexIndex, float duration, float delay)
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);
            float t = 0;

            int vertexIndex0 = vertexIndex + 0;
            int vertexIndex1 = vertexIndex + 1;
            int vertexIndex2 = vertexIndex + 2;
            int vertexIndex3 = vertexIndex + 3;

            var startAlpha = newVertexColors[vertexIndex0].a;
            if (duration == 0)
                t = 1;
            duration *= Mathf.Abs((targetAlpha - startAlpha) / 255f);
            while (t < 1)
            {
                float alpha = Mathf.Lerp(startAlpha, targetAlpha, t);
                newVertexColors[vertexIndex0].a = (byte)alpha;
                newVertexColors[vertexIndex1].a = (byte)alpha;
                newVertexColors[vertexIndex2].a = (byte)alpha;
                newVertexColors[vertexIndex3].a = (byte)alpha;

                newVertexColorsCopy[vertexIndex0] = newVertexColors[vertexIndex0];
                newVertexColorsCopy[vertexIndex1] = newVertexColors[vertexIndex1];
                newVertexColorsCopy[vertexIndex2] = newVertexColors[vertexIndex2];
                newVertexColorsCopy[vertexIndex3] = newVertexColors[vertexIndex3];

                yield return null;
                t += Time.deltaTime / duration;

            }
            newVertexColors[vertexIndex0].a = targetAlpha;
            newVertexColors[vertexIndex1].a = targetAlpha;
            newVertexColors[vertexIndex2].a = targetAlpha;
            newVertexColors[vertexIndex3].a = targetAlpha;

            newVertexColorsCopy[vertexIndex0] = newVertexColors[vertexIndex0];
            newVertexColorsCopy[vertexIndex1] = newVertexColors[vertexIndex1];
            newVertexColorsCopy[vertexIndex2] = newVertexColors[vertexIndex2];
            newVertexColorsCopy[vertexIndex3] = newVertexColors[vertexIndex3];

        }
        public void FadeInOverallAlpha(float totalDuration)
        {
            if (FadeOverallAlpha != null)
                StopCoroutine(FadeOverallAlpha);

            if (totalDuration == 0)
            {
                FadeInOverallAlphaInstantly();
                return;
            }
            FadeOverallAlpha = StartCoroutine(ChangeOverallAlpha(totalDuration, 0, 1));
        }
        public void FadeOutOverallAlpha(float totalDuration)
        {
            if (FadeOverallAlpha != null)
                StopCoroutine(FadeOverallAlpha);

            if (totalDuration == 0)
            {
                
                FadeOutOverallAlphaInstantly();
                return;
            }
            FadeOverallAlpha = StartCoroutine(ChangeOverallAlpha(totalDuration,1, 0));
        }
        IEnumerator ChangeOverallAlpha(float duration, float startAlpha, float targetAlpha)
        {

            float t = 0;
            Color startFaceColor = fontMaterial.GetColor("_FaceColor");
            Color glowColor = fontMaterial.GetColor("_GlowColor");
            Color outlineColor = fontMaterial.GetColor("_OutlineColor");

            float currentAlpha = startFaceColor.a;
            t = 1 - Mathf.Abs(targetAlpha - currentAlpha);
            if (duration == 0)
                t = 1;
            var faceColor = startFaceColor;
            while (t < 1)
            {
                var alpha = Mathf.SmoothStep(startAlpha, targetAlpha, t);
                faceColor.a = alpha;
                glowColor.a = alpha;
                outlineColor.a = alpha;

                fontMaterial.SetColor("_FaceColor", faceColor);
                fontMaterial.SetColor("_GlowColor", glowColor);
                fontMaterial.SetColor("_OutlineColor", outlineColor);


                yield return null;
                t += Time.deltaTime / duration;
            }
            faceColor.a = targetAlpha;
            glowColor.a = targetAlpha;
            outlineColor.a = targetAlpha;

            fontMaterial.SetColor("_FaceColor", faceColor);
            fontMaterial.SetColor("_GlowColor", glowColor);
            fontMaterial.SetColor("_OutlineColor", outlineColor);


            yield return null;

        }
        void RestoreVertexColors()
        {
            newVertexColors = text.textInfo.meshInfo[0].colors32;

            if (newVertexColorsCopy.Length != newVertexColors.Length)
            {
                var copyPreviousLength = newVertexColorsCopy.Length;
                System.Array.Resize(ref newVertexColorsCopy, newVertexColors.Length);
                for (int i = copyPreviousLength; i < newVertexColorsCopy.Length; i++)
                {
                    newVertexColorsCopy[i] = newVertexColors[i];
                }
            }
            for (int i = 0; i < newVertexColorsCopy.Length; i++)
                newVertexColors[i] = newVertexColorsCopy[i];

            text.mesh.colors32 = newVertexColors;
            text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
            if (text.textInfo.characterCount != characterFadeRoutines.Length)
                System.Array.Resize(ref characterFadeRoutines, text.textInfo.characterCount);
        }
        void FadeOutInstantly()
        {
            ChangeCharactersAlphaInstantly(0);
        }
        void FadeInInstantly()
        {
            ChangeCharactersAlphaInstantly(255);
        }
        void ChangeCharactersAlphaInstantly(byte alpha)
        {
            text.ForceMeshUpdate(true);
            RestoreVertexColors();

            for (int i = 0; i < text.textInfo.characterCount; i++)
            {
                var characterInfo = text.textInfo.characterInfo[i];
                if (characterInfo.isVisible)
                {
                    int vertexIndex = text.textInfo.characterInfo[i].vertexIndex;

                    int vertexIndex0 = vertexIndex + 0;
                    int vertexIndex1 = vertexIndex + 1;
                    int vertexIndex2 = vertexIndex + 2;
                    int vertexIndex3 = vertexIndex + 3;

                    newVertexColors[vertexIndex0].a = alpha;
                    newVertexColors[vertexIndex1].a = alpha;
                    newVertexColors[vertexIndex2].a = alpha;
                    newVertexColors[vertexIndex3].a = alpha;

                    newVertexColorsCopy[vertexIndex0] = newVertexColors[vertexIndex0];
                    newVertexColorsCopy[vertexIndex1] = newVertexColors[vertexIndex1];
                    newVertexColorsCopy[vertexIndex2] = newVertexColors[vertexIndex2];
                    newVertexColorsCopy[vertexIndex3] = newVertexColors[vertexIndex3];
                }
            }
            text.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
        void FadeOutOverallAlphaInstantly()
        {
            ChangeOverallAlphaInstantly(0);
        }
        void FadeInOverallAlphaInstantly()
        {
            ChangeOverallAlphaInstantly(1);
        }
        void ChangeOverallAlphaInstantly(float alpha)
        {
            Color faceColor = fontMaterial.GetColor("_FaceColor");
            Color glowColor = fontMaterial.GetColor("_GlowColor");
            Color outlineColor = fontMaterial.GetColor("_OutlineColor");
            faceColor.a = alpha;
            glowColor.a = alpha;

            fontMaterial.SetColor("_FaceColor", faceColor);
            fontMaterial.SetColor("_GlowColor", glowColor);
            fontMaterial.SetColor("_OutlineColor", outlineColor);

        }
        public void StopOngoingFades()
        {
            StopAllCoroutines();
        }
    }
}
