using UnityEngine;
using System.Collections;

public class ScreenshotTaker : MonoBehaviour
{
    private int screenshotCount = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeScreenshot();
        }
    }

    private void TakeScreenshot()
    {
        StartCoroutine(CaptureScreenshot());
    }

    private IEnumerator CaptureScreenshot()
    {
        yield return new WaitForEndOfFrame();

        string screenshotFilename = string.Format("Screenshot_{0}.png", screenshotCount);
        screenshotCount++;

        ScreenCapture.CaptureScreenshot(screenshotFilename);
        
        Debug.Log($"Screenshot saved to: {screenshotFilename}");
    }
}